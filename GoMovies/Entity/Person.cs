using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoMovies.Entity
{
    public class Person
    {
        public int PersonId { get; set; }
        public string Biography { get; set; }
        public string ImdbLink { get; set; }
        public string PlaceOfBirth { get; set; }
        public AppUser AppUser { get; set; }
        public int? AppUserId { get; set; } // bu değer Unique olabilir çünkü Yukarıda User classında Person Person olarak tanımladık. bir liste değil bu yüzden tek ve uniq bir id alabilir

    }
}
