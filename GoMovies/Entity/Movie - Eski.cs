using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GoMovies.Entity
{
    public class Eskimovie
    {
        public int MovieId { get; set; }
        [DisplayName("Film adı")] //ComponenModel kütüphanesini yukarıya eklememiz gerekir.
        [Required(ErrorMessage = "Film başlığı girilmelidir.")] // gerekli olarak tanımladık. DataAnotation kütüphanesi eklenmeli
        public string Title { get; set; }
        [DisplayName("Açıklama")]
        [Required(ErrorMessage = "Film açıklaması girilmelidir.")]
        [StringLength(5000, MinimumLength = 100, ErrorMessage = "100 ile 5000 arasında bir açıklama girilmelidir")]
        public string Description { get; set; }
        [DisplayName("Yönetmen")]
        [Required(ErrorMessage = "Film yönetmen bilgisi girilmelidir.")]
        public string Director { get; set; }
        public string[] Players { get; set; }
        [DisplayName("Film görseli")]
        [Required(ErrorMessage = "Film görseli girilmelidir.")]
        public string imageUrl { get; set; }
        public string MovieEmbededVideoLink { get; set; }
        [DisplayName("Film Türü")]
        [Required(ErrorMessage = "Bir film türü seçilmelidir.")]
        public int? GenreId { get; set; } //Fİlm türünün id'si bu değeri bilerek null verdik. Böylece Required validasyonuna
        //null değer atanır ve required masajı kullanıcıya verilir. AKsi halde nullable yapmasaydık. select nesnesi default olarak
        //0 (sıfır) değerini alır ve 0 null lmadığı için required attiribute'ı çalışmaz. Ve böylece tür seçmeden form submit edilir.
        //Bu istenmeyen bir durum ortaya çıkarır.
        public int[] GenreMultipleId { get; set; } //deneme olarak film türünü birden fazla yapmak iin ekledim
        public int CommentId { get; set; }
    }

}
