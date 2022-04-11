using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoMovies.Entity
{
    public class AppUserRole
    {
        public int AppUserRoleId { get; set; }
        public string RoleName { get; set; }
        public string RoleDefination { get; set; }
        public List<AppUser> AppUsers { get; set; }

    }
}
