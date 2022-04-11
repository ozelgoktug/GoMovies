using GoMovies.Validators;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GoMovies.Models
{
    public class UserModel
    { 
        public int UserId { get; set; }

        [Required(ErrorMessage ="Bir kullanıcı adı girmelisiniz.")]
        [Display(Name="Kullanıcı Adı")]
        [StringLength(15, ErrorMessage ="{0} karakter uzunluğu {2}-{1} arasında olmalıdır.", MinimumLength =3)]
      [Remote(action:"VerifyUserNameIsExist", controller:"User")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "İsim")]
        [StringLength(15, ErrorMessage = "{0} karakter uzunluğu {2}-{1} arasında olmalıdır.", MinimumLength = 3)]
        public string Name { get; set; }

        [Required]
        [StringLength(15, ErrorMessage = "{0} karakter uzunluğu {2}-{1} arasında olmalıdır.", MinimumLength = 3)]
        [Display(Name = "Soyisim")]
        public string Surname { get; set; }

        [Display(Name = "Hakkınızda")]
        [StringLength(5000, ErrorMessage = "{0} karakter uzunluğu en fazla {1} olmalıdır.")]
        public string Description { get; set; }

        [Display(Name = "Profil Fotoğrafı")]
        public string UserImage { get; set; }

        [Required]
        [EmailAddress]
        [EmailProviders]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Parola")]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Parolanızı Yeniden Giriniz")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password),ErrorMessage ="Girdiğiniz parola bilgileri eşleşmedi.")]
        public string RePassword { get; set; }

        [Url]
        public string Url { get; set; }

        [BirthDate(ErrorMessage = "Kayıt için 18 yaşınızı doldurmanız gerekmektedir.")]
        [Display(Name = "Doğum Yılınız")]
        [DataType(DataType.Date)]
        //[Range(1930, 2004, ErrorMessage = "Doğum yılınız {1}-{2} arasında olmalıdır.")]
        public DateTime BirthDate { get; set; } = DateTime.Now;

        public int UserRoleId { get; set; }
        public bool isActive { get; set; }
        public bool isOnline { get; set; }
        public int[] favoriteMoviesId { get; set; }
    }
}
