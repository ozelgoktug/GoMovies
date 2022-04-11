using GoMovies.Entity;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoMovies.Data
{
    public class DataSeeding
    {
        //Bu class Developer aşamasında DB'e statik olarak veri eklemek amacıyla oluşturulmuştur.

        public static void Seed(IApplicationBuilder app)
        {
            var scope = app.ApplicationServices.CreateScope();
            var context = scope.ServiceProvider.GetService<GoMovieContext>();
            

            context.Database.Migrate(); // dotnet ef database update'in karşılığı bu fonksiyondur. database update etmeye gerek yok bu fonksiyon çalıştıkça...

            var genres = new List<Genre>()
                        {
                                new Genre {
                                    Name="Bilimkurgu"
                                },
                                new Genre {
                                    Name="Romantik", 
                                },
                                new Genre {
                                    Name="Gerilim"
                                },
                                new Genre {
                                    Name="Animasyon",
                                    //Biz aşağıdaki gibi bir türe bağlı filmleri de kodla ilgili Fİlm tablosuna ekleyebiliyoruz.
                                     Movies = new List<Movie>()
                                     {
                                            new Movie
                                            {
                                                Title = "Soul",
                                                Description = "Başrollerde Ryan Reynolds efsanesinin yanı sıra Jodie Corner, Joe Keery ve Taika Waititi gibi birçok ismi bir arada görüyoruz. Guy adında bir adam var; her gün aynı şeyleri yapıyor. Yaşamında bir sürprizle karşılaşma olasılığı neredeyse yok gibidir diyebiliriz bu adam için. Ama bir gün Guy kendi kendine bundan daha fazlasını dilediğini itiraf eder ve olaylar bir anda gelişmeye başlar. Millie adlı kadınla bir araya geldiği an itibarıyla hayatı değişime uğrayan ve gerçeklikle alakalı bildikleri sarsıntıya uğrayan genç adam, bu manada The Truman Show filmine benzer bir durumun içerisinde gibidir. Zak Penn ve Matt Lieberman gibi usta isimler filmin senaristliğini üstleniyor. Guy’ın yaşamakta olduğu kendi bilincine ulaşma serüveni varoluşsal bir boyut alır ve genç adam aydınlanma yaşar. Filmde bir aile sıcaklığı hissederken öteki yandan dramatik ögelerle de ana fikri anlamış oluyorsunuz. Ryan Reynolds her zamanki gibi oyunculuğunu resmen konuşturuyor ve karşımıza bambaşka bir karakterle beraber çıkmış oluyor, kesinlikle şans vermeniz gereken özel bir film diyebiliriz.",
                                                Director = "David Jackson",
                                                imageUrl = "soul.jpg",
                                                MovieEmbededVideoLink = "https://www.youtube.com/embed/X2m-08cOAbc",
                                            },
                                            new Movie
                                            {
                                                Title = "Sing",
                                                Description = "This is a wider card with supporting text below as a natural lead-in to additional content. This content is a little bit longer.",
                                                Director = "Peter David",
                                                imageUrl = "sing.jpg",
                                                MovieEmbededVideoLink = "https://www.youtube.com/embed/ngiK1gQKgK8",
                                            }
                                     } 
                                },
                                new Genre {

                                    Name="Fantastik"
                                },
                                new Genre {
                                    Name="Komedi"
                                },
                                new Genre {
                                    Name="Dram"
                                },
                                new Genre {
                                     Name="Aksiyon"
                                 }
                       };
            var appUserRoles = new List<AppUserRole>()
            {
                new AppUserRole(){RoleName="Admin", RoleDefination="Bu bir admin kullanıcısıdır." },
                new AppUserRole(){RoleName="Yazar", RoleDefination="Bu bir yazar kullanıcısıdır."},
                new AppUserRole(){RoleName="Kullanıcı", RoleDefination="Bu bir kullanıcıdır."}

            };
            var movies = new List<Movie>()
                         {


                            new Movie
                            {
                                Title = "Dont Look Up",
                                Description = "İki astronom rotasından çıkan bir meteoru fark eder. Meteorun yeni rotası Dünya ile kesişmektedir ve bu çarğışma Dünya'daki canlı hayatını sonlandırabilir. Medyayı ve dünyanın geri kalanını kendilerine inandırmak için uğraşan bu ikili başarılı olabilecek midir? 2021 yılının en beklenen yapımlarından olan \"Don't Look Up\", Leonardo DiCaprio, Jennifer Lawrence, Meryl Streep, Timothee Chalamet, Jonah Hill ve Cate Blanchett gibi yıldızları oyuncu kadrosunda barındırıyor.",
                                Director = "Steven Spielberg",
                                imageUrl = "DontLookUp.jpg",
                                MovieEmbededVideoLink = "https://www.youtube.com/embed/1oUfDr2R54Y",
                                Genres = new List<Genre>(){ genres[0], genres[5] }// burada Movie içindeki navigation property olan Genre propertysine, daha önce oluşturulan genres listinin [0]ıncı tür objesini atayabiliriz
                                                    // böylece bu filmin türü direkt olarak genres içinden alınır ve DBde olası hataların önüne geçebiliriz. Yani 0'ıncı elemanın o an DB'deki id'si
                                                    // neyse gelir buradaki GENRE propuna o id'yi atar. Veya manuel de  aşağıdaki gibi id verebiliriz.  GenreId = genres[0].GenreId olarak da kullanabilirdik.
                                                    //Buraya Komedi ve Bilim Kurgu türü atanmış oldu.
                            },
                            new Movie
                            {
                                Title = "Free Guy",
                                Description = "Başrollerde Ryan Reynolds efsanesinin yanı sıra Jodie Corner, Joe Keery ve Taika Waititi gibi birçok ismi bir arada görüyoruz. Guy adında bir adam var; her gün aynı şeyleri yapıyor. Yaşamında bir sürprizle karşılaşma olasılığı neredeyse yok gibidir diyebiliriz bu adam için. Ama bir gün Guy kendi kendine bundan daha fazlasını dilediğini itiraf eder ve olaylar bir anda gelişmeye başlar. Millie adlı kadınla bir araya geldiği an itibarıyla hayatı değişime uğrayan ve gerçeklikle alakalı bildikleri sarsıntıya uğrayan genç adam, bu manada The Truman Show filmine benzer bir durumun içerisinde gibidir. Zak Penn ve Matt Lieberman gibi usta isimler filmin senaristliğini üstleniyor. Guy’ın yaşamakta olduğu kendi bilincine ulaşma serüveni varoluşsal bir boyut alır ve genç adam aydınlanma yaşar. Filmde bir aile sıcaklığı hissederken öteki yandan dramatik ögelerle de ana fikri anlamış oluyorsunuz. Ryan Reynolds her zamanki gibi oyunculuğunu resmen konuşturuyor ve karşımıza bambaşka bir karakterle beraber çıkmış oluyor, kesinlikle şans vermeniz gereken özel bir film diyebiliriz.",
                                Director = "David Fincher",
                                imageUrl = "FreeGuy.jpg",
                                MovieEmbededVideoLink = "https://www.youtube.com/embed/X2m-08cOAbc",
                                Genres = new List<Genre>(){ genres[0], genres[5] }  //Komedi ve Bilim Kurgu türü atandı
                            },
                            new Movie
                            {
                                Title = "Wonder",
                                Description = "This is a wider card with supporting text below as a natural lead-in to additional content. This content is a little bit longer.",
                                Director = "Peter Jackson",
                                imageUrl = "Wonder.jpg",
                                MovieEmbededVideoLink = "https://www.youtube.com/embed/ngiK1gQKgK8",
                                Genres = new List<Genre>(){ genres[6] } //Dram türü atandı.
                            },
                            new Movie
                            {
                                Title = "Whiplash",
                                Description = "This is a wider card with supporting text below as a natural lead-in to additional content. This content is a little bit longer.",
                                Director = "Damien Chazelle",
                                imageUrl = "whiplash.jpg",
                                MovieEmbededVideoLink = "https://www.youtube.com/embed/7d_jQycdQGo",
                                Genres = new List<Genre>(){  genres[6] }
                            },
                            new Movie
                            {
                                Title = "The Fault in Our Stars",
                                Description = "This is a wider card with supporting text below as a natural lead-in to additional content. This content is a little bit longer.",
                                Director = "Josh Boone",
                                imageUrl = "FaultStars.jpg",
                                MovieEmbededVideoLink = "https://www.youtube.com/embed/9ItBvH5J6ss",
                               Genres = new List<Genre>(){ genres[1], genres[6] }
                            },
                             new Movie
                            {
                                Title = "CODA",
                                Description = "Bir Coda(Child of Deaf Adults/Sağır olan ebeveynlerin çocuğu) olan Ruby, ailesinde duyma kabiliyeti olan tek kişidir. Ailesinin başında durduğu balıkçılık işi zora girince Ruby bir anda kendini ailesini terk etme korkusu ve müzik tutkusunun peşinden gitme olmak üzere iki kritik kararın arasında bulur.",
                                Director = "Martin Scorsese",
                                imageUrl = "Coda.jpg",
                                MovieEmbededVideoLink = "https://www.youtube.com/embed/0pmfrE1YL4I",
                                Genres = new List<Genre>(){ genres[1], genres[6] }
                            },
                         };
            var appusers = new List<AppUser>()
            {
                new AppUser
                    {
                    userName="ozelgoktug",
                    password="1111",
                    Name="Göktuğ",
                    Surname="Özel",
                    Description="Bu bir admin kullanıcısıdır.",
                    isOnline=false,
                    isDeleted=false,
                    UserRoleId=0,
                    AppUserRoles = appUserRoles[0]

                    },
                    new AppUser
                    {
                    userName="bahar",
                    password="1111",
                    Name="Bahar",
                    Surname="Özel",
                    Description="Bu bir admin kullanıcısıdır.",
                    isOnline=false,
                    isDeleted=false,
                    UserRoleId=1,
                    
                    AppUserRoles = appUserRoles[1]                    
                    },
                    new AppUser
                    {
                    userName="hozel",
                    password="1111",
                    Name="Haşmet",
                    Surname="Özel",
                    Description="Bu bir admin kullanıcısıdır.",
                    isOnline=false,
                    isDeleted=false,
                    UserRoleId=1,
                    AppUserRoles = appUserRoles[2]
                    }
            };
            var people = new List<Person>() 
            {
                new Person() //Bu kullanıcı için bir Film Personeli oluşturduk. böylece bu kullanıcı aynı zamanda film oyuncusu veya yönetmeni oalrak da referans edilebilir.
                {
                    PlaceOfBirth = "Türkiye",
                    Biography = "Türk yönetmen, yazar, oyuncu, ressam...",
                    ImdbLink = "https://www.imdb.com/name/nm1587232/?ref_=fn_nm_nm_1",
                    AppUser= appusers[1]
                },
                new Person() //Bu kullanıcı için bir Film Personeli oluşturduk. böylece bu kullanıcı aynı zamanda film oyuncusu veya yönetmeni oalrak da referans edilebilir.
                {
                    PlaceOfBirth = "Türkiye",
                    Biography = "Türk yönetmen, yazar, oyuncu, ressam...",
                    ImdbLink = "https://www.imdb.com/name/nm0136797/?ref_=nmls_hd",
                    AppUser= appusers[2]
                }
            };
            var crews = new List<Crew>()
            {
                new Crew(){Movie=movies[0], Person=people[0], Job="Yönetmen"},
                new Crew(){Movie=movies[0], Person=people[1], Job="Yönetmen Yard."},
            };
            var casts = new List<Cast>()
            {
                new Cast() {Movie=movies[0], Person=people[0], Name="Oyuncu Adı 1", Character="Karakter 1"},
                new Cast() {Movie=movies[0], Person=people[1], Name="Oyuncu Adı 2", Character="Karakter 2"},
            };

            //Eğer Migration klasöründe pending edilecek bir migration varsa
            //ki DB'yi komple silersek migrationa bakacak ve ilgili tablolar olmadığından
            //// yeni DB ve tabloları oluşturacak migration ile gerekli bilgileri oluşturacaktır.
            if (context.Database.GetPendingMigrations().Count() == 0)
            {
                
                //Eğer Genres tablosu boş ise aşağıdaki bilgileri tabloya yaz
                if (context.Genres.Count() == 0)
                {
                    //Genre listesi oluştur.
                    context.Genres.AddRange(genres);
                }

                //Eğer Genres tablosu boş ise aşağıdaki bilgileri tabloya yaz
                if (context.AppUserRoles.Count() == 0)
                {
                    //Genre listesi oluştur.
                    context.AppUserRoles.AddRange(appUserRoles);
                }

                //Eğer Movie tablosu boş ise aşağıdaki bilgileri tabloya yaz
                if (context.Movies.Count() == 0)
                {
                    //Movies listesi oluştur. 
                    context.Movies.AddRange(movies);

                    //context.Movies.Add(new Movie() // bir tane obje oluşturup yollamak için...
                    //{
                    //    MovieId = 1,
                    //    Title = "Dont Look Up",
                    //    Description = "İki astronom rotasından çıkan bir meteoru fark eder. Meteorun yeni rotası Dünya ile kesişmektedir ve bu çarğışma Dünya'daki canlı hayatını sonlandırabilir. Medyayı ve dünyanın geri kalanını kendilerine inandırmak için uğraşan bu ikili başarılı olabilecek midir? 2021 yılının en beklenen yapımlarından olan \"Don't Look Up\", Leonardo DiCaprio, Jennifer Lawrence, Meryl Streep, Timothee Chalamet, Jonah Hill ve Cate Blanchett gibi yıldızları oyuncu kadrosunda barındırıyor.",
                    //    Director = "Steven Spielberg",
                    //    imageUrl = "DontLookUp.jpg",
                    //    MovieEmbededVideoLink = "https://www.youtube.com/embed/1oUfDr2R54Y",
                    //    GenreId = 1
                    //});


                }

                //Eğer AppUsers tablosu boş ise aşağıdaki bilgileri tabloya yaz
                if (context.AppUsers.Count() == 0)
                {
                    context.AppUsers.AddRange(appusers);
                }

                // Eğer People tablosu boş ise aşağıdaki bilgileri tabloya yaz
                if (context.People.Count() == 0)
                {
                    context.People.AddRange(people);
                }

                // Eğer Casts tablosu boş ise aşağıdaki bilgileri tabloya yaz
                if (context.Casts.Count() == 0)
                {
                    context.Casts.AddRange(casts);
                }

                // Eğer Crews tablosu boş ise aşağıdaki bilgileri tabloya yaz
                if (context.Crews.Count() == 0)
                {
                    context.Crews.AddRange(crews);
                }

                // tüm değişiklikleri DB'de kaydet
                context.SaveChanges();


            }






        }
    }
}
