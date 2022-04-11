using GoMovies.Entity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GoMovies.Models
{
    public class MovieAdminPanelViewModel
    {
        public Movie _Movie { get; set; }
        public List<Genre> Genres { get; set; }
        public List<AdminMovieModel> Movies { get; set; }
    }

    public class AdminMovieModel
    {
        public int MovieId { get; set; }
        public string Title { get; set; }
        public string imageUrl { get; set; }
        public List<Genre> Genres { get; set; }
    }

    public class AdminUpdateMovieModel
    {
        public int MovieId { get; set; }
        [Display(Name = "Film adı")]
        [Required(ErrorMessage = "Film adı yazılmalıdır.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Film adı 2-50 karakter arasında olmalıdır...")]
        public string Title { get; set; }
        public string imageUrl { get; set; }
        [Display(Name = "Film özeti")]
        [Required(ErrorMessage = "Film özeti yazılmalıdır.")]
        [StringLength(10000, MinimumLength = 200, ErrorMessage = "Film özeti 500-10.000 karakter arasında olmalıdır...")]
        public string Description { get; set; }
        public string Director { get; set; }
        public string MovieEmbededVideoLink { get; set; }
        [Required(ErrorMessage = "En az bir tür seçmelisiniz.")]
        public int[] GenreIds { get; set; }

    }

    public class AdminCreateMovieModel
    {
        public int MovieId { get; set; }
        [Display(Name ="Film adı")]
        [Required(ErrorMessage ="Film adı yazılmalıdır.")]
        [StringLength(50, MinimumLength =2, ErrorMessage ="Film adı 2-50 karakter arasında olmalıdır...")]
        [Remote(action: "VerifyMovieExist", controller: "MovieAdminPanel")]
        public string Title { get; set; }
        public string imageUrl { get; set; }
        [Display(Name = "Film özeti")]
        [Required(ErrorMessage = "Film özeti yazılmalıdır.")]
        [StringLength(10000, MinimumLength = 500, ErrorMessage = "Film özeti 500-10.000 karakter arasında olmalıdır...")]
        public string Description { get; set; }
        public string Director { get; set; }
        public string MovieEmbededVideoLink { get; set; }
        public List<Genre> Genres { get; set; }
        public int[] GenreIds { get; set; }

    }
}
