using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GoMovies.Validators
{
    public class EmailProvidersAttribute: ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string email = "";

            if (value!=null)
            {
                email = value.ToString();
            }

            //bu validasyon şartlarını örnek olsun diye yaptım
            //eğer mail aşağıdaki gibi bitmiyorsa validasyona bir hata mesajı göndereceğiz
            if (email.EndsWith("@gmail.com") || email.EndsWith("@hotmail.com") || email.EndsWith("@outlook.com"))
            {
                return ValidationResult.Success;
            }
            return new ValidationResult("Hatalı eposta sunucusu.");
  
        }
    }
}
