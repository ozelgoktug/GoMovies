using GoMovies.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GoMovies.Models
{
    public class MovieDetailsViewModel
    {
        public MovieDetailsViewModel()
        {
            Genres = new List<Genre>(); //model içindeki Genres listesi null olmasın diye boş bir referans verdik bekli sorun olur diye. 
                                        //bu yüzden ne zaman movie objesi oluşturulursa constructor içinde yeni bir Genres'e reefrans verecek bir kod yazdık.
            Comments = new List<Entity.Comment>();
            ApplicationUsers = new List<Entity.ApplicationUser>();


        }
        public int MovieId { get; set; } //MovieId veya Id olarak da tanımladığımızda DB bunun bir primaryKey olduğunu biliyor ve yukarıdaki KEY attiribute'unu yazmaya gerek kalmıyor.
        public string Title { get; set; }
        public string Description { get; set; }
        public string Director { get; set; }
        // public string[] Players { get; set; } bununla ilgili ayrı tablo oluşturulacak
        public string imageUrl { get; set; }
        public string MovieEmbededVideoLink { get; set; }
        public List<Genre> Genres { get; set; } //Bu bir navigation property'dir. DB'de context aracılığıyla GenreId'nin bağlı olduğu uzak bir tablo olduğunu anlayacaktır bu sayede uygulamamız
        public List<Entity.Comment> Comments { get; set; }

        public List<ApplicationUser> ApplicationUsers { get; set; }

        public int CommentId  { get; set; }
        [Display(Name = "Yorum Başlığı")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Yorum başlığı 2-50 karakter arasında olmalıdır...")]
        public string CommentTittle { get; set; }
        [Display(Name = "Yorumunuz")]
        [StringLength(5000, MinimumLength = 2, ErrorMessage = "Yorum başlığı 2-5000 karakter arasında olmalıdır...")]
        public string CommentText { get; set; }
        public int  CommentUserId { get; set; }
    }
}
