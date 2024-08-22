using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace ModernizationDemo.Wcf.Security
{
    public class CustomRoleProvider : RoleProvider
    {
        // only ValidateUser method is implemented - we don't need the rest of the methods

        public override bool IsUserInRole(string username, string roleName)
        {
            return username == "test" && roleName == "admin";
        }

        public override string[] GetRolesForUser(string username)
        {
            return username == "test" ? new[] { "admin" } : new string[] { };
        }

        public override bool RoleExists(string roleName)
        {
            return roleName == "admin";
        }

        public override string[] GetAllRoles()
        {
            return new[] { "admin" };
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string ApplicationName { get; set; }
    }
}