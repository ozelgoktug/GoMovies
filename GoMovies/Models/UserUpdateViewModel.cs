using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GoMovies.Models
{
    public class UserUpdateViewModel
    {

        

        [Display(Name ="Ad")]
        [Required(ErrorMessage ="Lütfen isminizi giriniz")]
        public string Name{ get; set; }

        [Display(Name = "Soyad")]
        [Required(ErrorMessage = "Lütfen soyadınızı giriniz")]
        public string Surname { get; set; }

        [Display(Name = "Kullanıcı adı")]
        [Required(ErrorMessage = "Lütfen kullanıcı adınızı giriniz")]
        public string UserName { get; set; }


        [Display(Name = "Mail adresi")]
        [Required(ErrorMessage = "Lütfen mail giriniz")]
        public string Mail { get; set; }

        [Display(Name = "Parolanız")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Gücenlleme işlemini onaylamak için parolanızı giriniz.")]
        public string CurrentPassword { get; set; }

        [Display(Name = "Yeni Parola")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Yeni Parola Tekrarı")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Yeni parola ve parola tekrarı alanları aynı olmalıdır.")]
        public string ConfirmPassword { get; set; }

        public List<Comment> Comments { get; set; }

        public string  ImageUrl { get; set; }


        public List<RoleAssignViewModel> RoleListModel { get; set; }

        public string existingUserName { get; set; }
    }
}
