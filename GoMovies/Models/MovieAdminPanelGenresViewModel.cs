using GoMovies.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GoMovies.Models
{
    public class MovieAdminPanelGenresViewModel
    {
        [Required(ErrorMessage = "Tür adı girmelisiniz.")]
        [Display(Name = "Tür adı")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Tür adı 2-50 karakter arasında olmalıdır...")]
        public string Name { get; set; }
        public List<AdminGenreViewModel> Genres { get; set; }
    }

    public class AdminGenreViewModel
    {
        public int GenreId { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
    }

    public class AdminGenreUpdateViewModel 
    {
       
        public int GenreId { get; set; }
        [Required(ErrorMessage = "Tür adı girmelisiniz.")]
        [Display(Name = "Tür adı")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Tür adı 2-50 karakter arasında olmalıdır...")]
        public string Name { get; set; }
        public List<AdminMovieModel> Movies { get; set; } //bunu MovieAdminPanelViewModel'den aldık zaten orada vardı
    }

    public class AdminGenreCreateViewModel
    {
        [Required(ErrorMessage ="Tür ismi girmelisiniz.")]
        [Display(Name = "Tür adı")]
        public string Name { get; set; }
    }

}
