using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Configuration;
using System.Data.Entity;
using System.Threading.Tasks;

namespace ModernizationDemo.IdentityTests
{
    [TestClass]
    public class LoginTests
    {
        private AppUserManager userManager;

        [TestInitialize]
        public async Task Init()
        {
            DatabaseHelpers.RecreateDatabase(ConfigurationManager.ConnectionStrings["DB"].ConnectionString);
            await DatabaseHelpers.SeedData();

            userManager = new AppUserManager();
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

                if (await userManager.IsLockedOutAsync(user.Id))
                {
                    return false;
                }

                return await userManager.CheckPasswordAsync(user, password);
            }
        }

        [TestMethod]
        public async Task RetrieveUsers()
        {
            var users = await userManager.Users.ToListAsync();
            Assert.AreEqual(1, users.Count(u => u.UserName == "testuser" && u.LockoutEndDateUtc == null));
            Assert.AreEqual(1, users.Count(u => u.UserName == "testadmin" && u.LockoutEndDateUtc == null));
            Assert.AreEqual(1, users.Count(u => u.UserName == "testunapproved" && u.LockoutEndDateUtc != null));
        }

        [TestMethod]
        public async Task ResetPassword()
        {
            var user = await userManager.FindByNameAsync("testuser");
            Assert.IsNotNull(user);

            // change the password
            var result = await userManager.ChangePasswordAsync(user.Id, "TestPassword1234+", "NewTestPassword1234+");
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

            Assert.AreEqual(expectedResult, await userManager.IsInRoleAsync(user.Id, role));
        }
    }
}
