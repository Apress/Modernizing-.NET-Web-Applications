using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ModernizationDemo.IdentityCoreIdentityTests
{
    [TestClass]
    public class LoginTests
    {
        private ServiceProvider provider;
        private UserManager<AppUser> userManager;

        [TestInitialize]
        public async Task Init()
        {
            // these tests only read this database - they do not change it
            var oldConnectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=IdentityTests;Integrated Security=True;Trust Server Certificate=true";

            // CAUTION: the tests will drop and recreate this database - make sure this connection string points to an empty or non-existent database
            var newConnectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=IdentityMigratedTests;Integrated Security=True;Trust Server Certificate=true";


            // register services
            var services = new ServiceCollection();
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(newConnectionString);
            });
            services
                .AddIdentityCore<AppUser>()
                .AddRoles<AppRole>()
                .AddUserManager<UserManager<AppUser>>()
                .AddRoleManager<RoleManager<AppRole>>()
                .AddEntityFrameworkStores<AppDbContext>();
            provider = services.BuildServiceProvider();

            // migrate the database
            var migrator = ActivatorUtilities.CreateInstance<IdentityMigrator>(provider);
            await migrator.MigrateAccounts(oldConnectionString, newConnectionString);

            // create services
            userManager = provider.GetRequiredService<UserManager<AppUser>>();
        }

        [DataTestMethod]
        [DataRow("testuser", "TestPassword1234+", true)]
        [DataRow("testuser", "TestPasswordABCD+", false)]
        [DataRow("testunapproved", "UnapprovedPassword1234+", false)]
        [DataRow("nonexistent", "TestPassword", false)]
        public async Task ValidateCredentials(string username, string password, bool expectedResult)
        {
            Assert.AreEqual(expectedResult, await CheckPassword(username, password));

            async Task<bool> CheckPassword(string username, string password)
            {
                // UserManager is a low-level class so the validation logic is a bit complicated
                // We need to check if the user exists and whether it has been locked out before checking the password
                // The SignInManager class provides a high-level API with PasswordSignInAsync method

                var user = await userManager.FindByNameAsync(username);
                if (user == null)
                {
                    return false;
                }

                if (await userManager.IsLockedOutAsync(user))
                {
                    return false;
                }

                return await userManager.CheckPasswordAsync(user, password);
            }
        }

        [TestMethod]
        public async Task ValidateCredentialsAndTestRehashing()
        {
            // get the user and remember the original password hash
            var user = await userManager.FindByNameAsync("testuser");
            Assert.IsNotNull(user);
            var originalPasswordHash = user.PasswordHash;
            Assert.AreEqual((byte)0, Convert.FromBase64String(originalPasswordHash)[0]);

            // check the password - it should work and it should rehash the password
            var result = await userManager.CheckPasswordAsync(user, "TestPassword1234+");
            Assert.IsTrue(result);

            // reload the user
            var user2 = await userManager.FindByNameAsync("testuser");
            Assert.IsNotNull(user2);

            // verify that the password hash has changed
            Assert.AreNotEqual(originalPasswordHash, user2.PasswordHash);
            Assert.AreNotEqual((byte)0, Convert.FromBase64String(user2.PasswordHash)[0]);

            // verify the new password works
            result = await userManager.CheckPasswordAsync(user, "TestPassword1234+");
            Assert.IsTrue(result);
        }


        [TestMethod]
        public async Task RetrieveUsers()
        {
            var users = await userManager.Users.ToListAsync();
            Assert.AreEqual(1, users.Count(u => u.UserName == "testuser" && u.LockoutEnd == null));
            Assert.AreEqual(1, users.Count(u => u.UserName == "testadmin" && u.LockoutEnd == null));
            Assert.AreEqual(1, users.Count(u => u.UserName == "testunapproved" && u.LockoutEnd != null));
        }

        [TestMethod]
        public async Task ResetPassword()
        {
            var user = await userManager.FindByNameAsync("testuser");
            Assert.IsNotNull(user);

            // change the password
            var result = await userManager.ChangePasswordAsync(user, "TestPassword1234+", "NewTestPassword1234+");
            Assert.IsTrue(result.Succeeded);

            // verify the new password works
            user = await userManager.FindByNameAsync("testuser");
            Assert.IsTrue(await userManager.CheckPasswordAsync(user, "NewTestPassword1234+"));

            // verify the old password does not
            user = await userManager.FindByNameAsync("testuser");
            Assert.IsFalse(await userManager.CheckPasswordAsync(user, "TestPassword1234+"));
        }

        [DataTestMethod]
        [DataRow("testuser", "admin", false)]
        [DataRow("testuser", "user", true)]
        [DataRow("testadmin", "admin", true)]
        [DataRow("testadmin", "user", true)]
        [DataRow("testunapproved", "admin", false)]
        [DataRow("testunapproved", "user", false)]
        public async Task IsInRole(string username, string role, bool expectedResult)
        {
            var user = await userManager.FindByNameAsync(username);
            Assert.IsNotNull(user);

            Assert.AreEqual(expectedResult, await userManager.IsInRoleAsync(user, role));
        }
    }
}