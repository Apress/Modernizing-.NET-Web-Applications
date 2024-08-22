using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ModernizationDemo.IdentityTests
{
    public class AppDbContext : IdentityDbContext<AppUser, AppRole, Guid, AppUserLogin, AppUserRole, AppUserClaim>
    {
        public AppDbContext() : base("DB")
        {
        }
    }

    public class AppUser : IdentityUser<Guid, AppUserLogin, AppUserRole, AppUserClaim>
    {
    }

    public class AppRole : IdentityRole<Guid, AppUserRole>
    {
    }

    public class AppUserLogin : IdentityUserLogin<Guid>
    {
    }

    public class AppUserRole : IdentityUserRole<Guid>
    {
    }

    public class AppUserClaim : IdentityUserClaim<Guid>
    {
    }
}
