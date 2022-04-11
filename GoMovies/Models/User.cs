using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoMovies.Models
{
    public class User
    {
        public int Id { get; set; }
        public string userName { get; set; }
        public string password { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Description { get; set; }
        public bool isOnline { get; set; }
        public bool isDeleted { get; set; }
        public int UserRoleId { get; set; }
        public int[] favoriteMoviesId { get; set; }
      
    }




}
