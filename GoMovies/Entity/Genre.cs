using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GoMovies.Entity
{
    public class Genre
    {
        public int GenreId { get; set; }
        [Required]
        public string Name { get; set; }

        public int MovieId { get; set; }
        public List<Movie> Movies { get; set; }




    }
}
