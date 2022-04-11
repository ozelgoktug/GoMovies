using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GoMovies.Validators
{
    public class BirthDateAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            DateTime datetime = Convert.ToDateTime(value);

            var year18 = (-365 * 18) - 5.50; //18 yıl gün olarak hesaplandı

            // Bu günün tarhinden 18 yıllık hesaplanan gün çıkarıldı. anlık saat dk ve saniye de çıkarıldı.
            //sonuç olarak mustBeYear değerine 18 yıl önceki günün tarihi atandı.
            DateTime mustBeYear = DateTime.Now.AddDays(year18) 
                .AddHours(-DateTime.Now.Hour)
                .AddMinutes(-DateTime.Now.Minute)
                .AddSeconds(-DateTime.Now.Second);

            //iki tarihi çıkarıp gün sayısıı gösterir.
            var result = (DateTime.Now - datetime).TotalDays; //girilen doğum tarihinin bu günden farkının gün sayısı olarak sonucunu verir

          //  return datetime < mustBeYear; // bunu da kullanabilirdik direkt. iki yıl arasında bir koşul sorguladık.
            return year18*-1<result; // 18 yıllık gün sayısı, girilen doğum tarihnin gün sayısından küçük ise 18 yaşından büyük olduğu için true dönecek.
          
            
        }
    }
}
