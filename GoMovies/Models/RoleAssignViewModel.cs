using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoMovies.Models
{
    public class RoleAssignViewModel
    {

        public int RoleID { get; set; }
        public string Name { get; set; }
        public bool IsRoleAlreadyAssignedToUser { get; set; }
    }
}
