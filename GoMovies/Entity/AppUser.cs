using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GoMovies.Entity
{
    public class AppUser
    {
        public int AppUserId { get; set; }
        public string userName { get; set; }
        public string password { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Description { get; set; }
        public bool isOnline { get; set; }
        public bool isDeleted { get; set; }
        public int UserRoleId { get; set; }
        public Person Person { get; set; }
        public AppUserRole AppUserRoles { get; set; }
        public int? AppUserRoleId { get; set; }
    }

    



}
