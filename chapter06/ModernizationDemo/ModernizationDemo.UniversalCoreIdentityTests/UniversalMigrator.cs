using System.Collections.Specialized;
using System.Data;
using System.Globalization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;

namespace ModernizationDemo.UniversalCoreIdentityTests;

public class UniversalMigrator
{
    private readonly UserManager<AppUser> userManager;
    private readonly RoleManager<AppRole> roleManager;
    private readonly AppDbContext dbContext;

    public string ApplicationName { get; set; } = "ModernizationDemo";

    public UniversalMigrator(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, AppDbContext dbContext)
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
        await MigrateProfiles(connection);
    }

    private async Task MigrateUsers(SqlConnection connection)
    {
        var users = ReadDataRows(connection, command =>
        {
            command.CommandText =
                """
                SELECT m.*, u.UserName, u.IsAnonymous, u.LastActivityDate
                  FROM dbo.Users u
                  JOIN dbo.Memberships m ON m.UserId = u.UserId
                  JOIN dbo.Applications a ON a.ApplicationId = u.ApplicationId
                  WHERE a.ApplicationName = @applicationName
                    AND u.IsAnonymous = 0
                """;
            command.Parameters.AddWithValue("@applicationName", ApplicationName);
        });
        foreach (var user in users)
        {
            // NOTES:
            // There is no email confirmation in the old system - we assume all e-mails are confirmed
            // IsApproved flag was migrated to LockoutEnd with a future date 9999-12-31
            // We have no Comment in the new system
            // We have no LastActivityDate and LastPasswordChangedDate in the new system
            // We do not migrate security question and answer
            // We do not migrate lockouts from the old system

            var migratedUser = new AppUser
            {
                Id = (Guid)user["UserId"],
                UserName = (string)user["UserName"],
                Email = (string)user["Email"],
                EmailConfirmed = true,
                TwoFactorEnabled = false,
                LockoutEnabled = true,
                LockoutEnd = (bool)user["IsApproved"] ? null : new DateTime(9999, 12, 31),
                CreatedDate = (DateTime)user["CreateDate"],
            };

            // encode the legacy password hash as a new password
            // this will protect it using the new stronger format
            var passwordHasher = (UniversalAwarePasswordHasher)userManager.PasswordHasher;
            migratedUser.PasswordHash = passwordHasher.BuildDoubleHashedPassword(
                migratedUser,
                (string)user["PasswordSalt"],
                (string)user["Password"]);

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
                SELECT r.*
                  FROM dbo.Roles r
                  JOIN dbo.Applications a ON a.ApplicationId = r.ApplicationId
                  WHERE a.ApplicationName = @applicationName
                """;
            command.Parameters.AddWithValue("@applicationName", ApplicationName);
        });
        foreach (var role in roles)
        {
            // NOTES:
            // We have no Description in the new system
            
            var migratedRole = new AppRole
            {
                Id = (Guid)role["RoleId"],
                Name = role["RoleName"].ToString()
            };
            var result = await roleManager.CreateAsync(migratedRole);
            if (!result.Succeeded)
            {
                Assert.Fail($"Failed to migrate role {role["RoleName"]}");
            }
        }
    }

    private async Task MigrateUserRoles(SqlConnection connection)
    {
        var usersRoles = ReadDataRows(connection, command =>
        {
            command.CommandText =
                """
                SELECT ur.UserId, r.RoleName
                  FROM dbo.UsersInRoles ur
                  JOIN dbo.Roles r ON r.RoleId = ur.RoleId
                  JOIN dbo.Applications a ON a.ApplicationId = r.ApplicationId
                  WHERE a.ApplicationName = @applicationName
                """;
            command.Parameters.AddWithValue("@applicationName", ApplicationName);
        });
        foreach (var userRole in usersRoles)
        {
            // TODO: this can be optimized by direct inserts into the new UserRoles table to prevent loading users and roles

            var user = await userManager.FindByIdAsync(((Guid)userRole["UserId"]).ToString());
            Assert.IsNotNull(user);

            var result = await userManager.AddToRoleAsync(user, (string)userRole["RoleName"]);
            if (!result.Succeeded)
            {
                Assert.Fail($"Failed to migrate role {userRole["RoleName"]}");
            }
        }
    }

    private async Task MigrateProfiles(SqlConnection connection)
    {
        var profiles = ReadDataRows(connection, command =>
        {
            command.CommandText =
                """
                SELECT *
                  FROM dbo.Profiles p
                """;
        });
        foreach (var profile in profiles)
        {
            var user = await userManager.FindByIdAsync(((Guid)profile["UserId"]).ToString());
            Assert.IsNotNull(user);

            // parse properties
            var properties = ProfileParser.ParseProperties(
                (string)profile["PropertyNames"],
                (string)profile["PropertyValueStrings"]);
            foreach (var property in properties)
            {
                switch (property.Name)
                {
                    case "FirstName":
                        user.FirstName = property.StringValue;
                        break;
                    case "LastName":
                        user.LastName = property.StringValue;
                        break;
                    case "FavoriteNumber":
                        user.FavoriteNumber = int.Parse(property.StringValue!, CultureInfo.InvariantCulture);
                        break;
                    case "BirthDate":
                        user.BirthDate = ProfileParser.XmlDeserialize<DateTime>(property.StringValue!);
                        break;
                    case "ImageUrls":
                        user.ImageUrls = ProfileParser.XmlDeserialize<StringCollection>(property.StringValue!).OfType<string>().Select(u => new AppUserImage() { Url = u }).ToList();
                        break;
                    case "ShoppingCart":
                        user.ShoppingCart = ProfileParser.DeserializeShoppingCart(property.StringValue!);
                        break;
                    default:
                        throw new NotSupportedException();
                }
            }

            await userManager.UpdateAsync(user);
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