using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ModernizationDemo.IdentityTests
{
    public class AppUserManager : UserManager<AppUser, Guid>
    {
        public AppUserManager()
            : base(new UserStore<AppUser, AppRole, Guid, AppUserLogin, AppUserRole, AppUserClaim>(new AppDbContext()))
        {
        }
    }

    public class AppRoleManager : RoleManager<AppRole, Guid>
    {
        public AppRoleManager()
            : base(new RoleStore<AppRole, Guid, AppUserRole>(new AppDbContext()))
        {
        }
    }
}
