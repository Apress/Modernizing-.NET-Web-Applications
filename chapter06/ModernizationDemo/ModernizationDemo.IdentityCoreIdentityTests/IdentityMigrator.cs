using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModernizationDemo.IdentityCoreIdentityTests
{
    public class IdentityMigrator
    {
        private readonly UserManager<AppUser> userManager;
        private readonly RoleManager<AppRole> roleManager;
        private readonly AppDbContext dbContext;

        public string ApplicationName { get; set; } = "ModernizationDemo";

        public IdentityMigrator(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, AppDbContext dbContext)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.dbContext = dbContext;
        }

        public async Task MigrateAccounts(string oldConnectionString, string newConnectionString)
        {
            DatabaseHelpers.RecreateDatabase(newConnectionString);
            await using var connection = new SqlConnection(oldConnectionString);

            // init the new database and migrate data
            await dbContext.Database.EnsureCreatedAsync();

            // migrate data from the old database
            await MigrateUsers(connection);
            await MigrateRoles(connection);
            await MigrateUserRoles(connection);

            // TODO: migrate user claims and logins if used in the old system
        }

        private async Task MigrateUsers(SqlConnection connection)
        {
            var users = ReadDataRows(connection, command =>
            {
                command.CommandText =
                    """
                SELECT *
                  FROM dbo.AspNetUsers u
                """;
            });
            foreach (var user in users)
            {
                var migratedUser = new AppUser
                {
                    Id = (Guid)user["Id"],
                    UserName = (string)user["UserName"],
                    Email = (string)user["Email"],
                    EmailConfirmed = (bool)user["EmailConfirmed"],
                    TwoFactorEnabled = (bool)user["TwoFactorEnabled"],
                    LockoutEnabled = (bool)user["LockoutEnabled"],
                    LockoutEnd = (DateTime?)user["LockoutEndDateUtc"].HandleDbNull(),
                    PasswordHash = (string)user["PasswordHash"],
                    SecurityStamp = (string)user["SecurityStamp"],
                    PhoneNumber = (string?)user["PhoneNumber"].HandleDbNull(),
                    PhoneNumberConfirmed = (bool)user["PhoneNumberConfirmed"],
                    AccessFailedCount = (int)user["AccessFailedCount"]

                    // TODO: migrate other properties added to the user
                };

                // save the user
                var result = await userManager.CreateAsync(migratedUser);
                if (!result.Succeeded)
                {
                    Assert.Fail($"Failed to migrate user {user["UserName"]}");
                }
            }
        }

        private async Task MigrateRoles(SqlConnection connection)
        {
            var roles = ReadDataRows(connection, command =>
            {
                command.CommandText =
                    """
                    SELECT *
                      FROM dbo.AspNetRoles r
                    """;
            });
            foreach (var role in roles)
            {
                var migratedRole = new AppRole
                {
                    Id = (Guid)role["Id"],
                    Name = (string)role["Name"]
                };
                var result = await roleManager.CreateAsync(migratedRole);
                if (!result.Succeeded)
                {
                    Assert.Fail($"Failed to migrate role {role["Name"]}");
                }
            }
        }

        private async Task MigrateUserRoles(SqlConnection connection)
        {
            var usersRoles = ReadDataRows(connection, command =>
            {
                command.CommandText =
                    """
                    SELECT ur.UserId, r.Name
                      FROM dbo.AspNetUserRoles ur
                      JOIN dbo.AspNetRoles r ON r.Id = ur.RoleId
                    """;
            });
            foreach (var userRole in usersRoles)
            {
                // TODO: this can be optimized by direct inserts into the new UserRoles table to prevent loading users and roles

                var user = await userManager.FindByIdAsync(((Guid)userRole["UserId"]).ToString());
                Assert.IsNotNull(user);

                var result = await userManager.AddToRoleAsync(user, (string)userRole["Name"]);
                if (!result.Succeeded)
                {
                    Assert.Fail($"Failed to migrate role {userRole["Name"]}");
                }
            }
        }

        private IEnumerable<DataRow> ReadDataRows(SqlConnection connection, Action<SqlCommand> configureCommand)
        {
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }

            using var command = connection.CreateCommand();
            configureCommand(command);
            using var reader = command.ExecuteReader();

            var dataTable = new DataTable();
            dataTable.Load(reader);
            return dataTable.Rows.OfType<DataRow>();
        }
    }
}
