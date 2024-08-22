using System.Configuration;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Security;

namespace ModernizationDemo.MembershipTests
{
    [TestClass]
    public class LoginTests
    {

        [TestInitialize]
        public void Init()
        {
            DatabaseHelpers.RecreateDatabase(ConfigurationManager.ConnectionStrings["DB"].ConnectionString);
            DatabaseHelpers.SeedData();
        }

        [DataTestMethod]
        [DataRow("test-user", "TestPassword1234+", true)]
        [DataRow("test-user", "TestPasswordABCD+", false)]
        [DataRow("test-unapproved", "UnapprovedPassword1234+", false)]
        [DataRow("nonexistent", "TestPassword", false)]
        public void ValidateCredentials(string username, string password, bool expectedResult)
        {
            Assert.AreEqual(expectedResult, Membership.ValidateUser(username, password));
        }

        [TestMethod]
        public void RetrieveUsers()
        {
            var users = Membership.GetAllUsers().OfType<MembershipUser>();
            Assert.AreEqual(1, users.Count(u => u.UserName == "test-user" && u.IsApproved));
            Assert.AreEqual(1, users.Count(u => u.UserName == "test-admin" && u.IsApproved));
            Assert.AreEqual(1, users.Count(u => u.UserName == "test-unapproved" && !u.IsApproved));
        }

        [TestMethod]
        public void ResetPassword()
        {
            var user = Membership.GetUser("test-user");
            user.ChangePassword("TestPassword1234+", "NewTestPassword1234+");
            
            Assert.IsTrue(Membership.ValidateUser("test-user", "NewTestPassword1234+"));
            Assert.IsFalse(Membership.ValidateUser("test-user", "TestPassword1234+"));
        }

        [DataTestMethod]
        [DataRow("test-user", "admin", false)]
        [DataRow("test-user", "user", true)]
        [DataRow("test-admin", "admin", true)]
        [DataRow("test-admin", "user", true)]
        [DataRow("test-unapproved", "admin", false)]
        [DataRow("test-unapproved", "user", false)]
        public void IsInRole(string username, string role, bool expectedResult)
        {
            Assert.AreEqual(expectedResult, Roles.IsUserInRole(username, role));
        }
    }
}
