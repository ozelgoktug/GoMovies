using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoMovies.Entity
{
    public class ApplicationUser: IdentityUser<int>
    {
        public string Name { get; set; }
        public string SurName { get; set; }
        public string imageUrl { get; set; }
        public List<Movie> Movies { get; set; } //Bu bir navigation property'dir. DB'de context aracılığıyla GenreId'nin bağlı olduğu uzak bir tablo olduğunu anlayacaktır bu sayede uygulamamız

        public int MyProperty { get; set; }

        public List<Comment> Comments { get; set; }

    }
}
