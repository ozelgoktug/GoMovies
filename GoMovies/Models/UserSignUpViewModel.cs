using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GoMovies.Models
{
    public class UserSignUpViewModel
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
        
        [Display(Name = "Parola")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Lütfen şifre giriniz")]
        public string Password { get; set; }

        [Display(Name = "Parola Tekrarı")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Parola ve Parola tekrarı alanları aynı olmalıdır.")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Mail adresi")]
        [Required(ErrorMessage = "Lütfen mail giriniz")]
        public string Mail { get; set; }

        public int RoleId { get; set; }

        public string RoleName { get; set; }

       public List<RoleAssignViewModel> RoleListModel { get; set; }
    }
}
