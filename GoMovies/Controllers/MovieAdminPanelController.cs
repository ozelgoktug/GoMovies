using GoMovies.Data;
using GoMovies.Entity;
using GoMovies.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GoMovies.Controllers
{
    [Authorize(Roles ="Admin")]
    public class MovieAdminPanelController : Controller
    {

      
        private readonly GoMovieContext _context;
        public MovieAdminPanelController(GoMovieContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CreateMovie()
        {
            ViewBag.Genres = _context.Genres.ToList();

            return View(new AdminCreateMovieModel()); //createmovie sayafsı açılışta bir model bekliyor ve bazı validasyon bilgilerine bakıyor bu yüzden boş da olsa bir model yolladık.
        }

        [HttpPost]
        public async Task<IActionResult> CreateMovie(AdminCreateMovieModel movie, IFormFile fileImg)
        {

            if (movie.GenreIds == null)
            {
                ModelState.AddModelError("GenreIds", "En az bir tür seçmelisiniz."); //Bu validasyonu AdminCereateMoieModel içinde GenreIds propertySi için [Required] diyerek de yapabiliriz
                                                                                     // ancak örnek olsun diye buradan da yapılabildiğini göstermek istedim. Bu arada GenreIds UpdateMovies içinde
                                                                                     // model içerisinden değil direkt post action metodu içinden ayrıca bir int[] dizisi içerisinden alınıyor
                                                                                     // onu da farklı bir metod olsun diye o şekilde bıraktım.
            }

            if (ModelState.IsValid)
            {
                var entity = new Movie
                {
                    Title = movie.Title,
                    Description = movie.Description,
                    MovieEmbededVideoLink = movie.MovieEmbededVideoLink,
                    Director = movie.Director,
                };



                foreach (var id in movie.GenreIds)
                {
                    entity.Genres.Add(_context.Genres.FirstOrDefault(i => i.GenreId == id));
                }

                //_context.Movies.Add(m);
                if (fileImg != null)
                {
                    var extension = Path.GetExtension(fileImg.FileName); // dosya uzantısını alır .jpg , .png gibi
                    var uniqFileName = string.Format($"{Guid.NewGuid()}{extension}"); //benzersiz bir resim ismi oluşturduk. NewGuid random benzersiz sayı üretir
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img", uniqFileName);
                    entity.imageUrl = uniqFileName;

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await fileImg.CopyToAsync(stream); //asenkron yaptık bu yüzden action medodu da Taske çevirdik
                                                           // fileImg.CopyTo(stream); //Olarak da kullanabilirdik
                    }

                }
                if (fileImg == null)
                {
                    entity.imageUrl = "no-image.png";
                }

                _context.Movies.Add(entity);
                _context.SaveChanges();
                return RedirectToAction("ListMovieAsTable", "MovieAdminPanel", new
                {
                    id = entity.MovieId,
                    CreatingStatus = "Success",
                    CreatedMovie = $"{entity.Title}"
                });
            }

            ViewBag.Genres = _context.Genres.ToList();
            return View(movie);
        }

        public IActionResult CreateMovie2()
        {
            //ViewBag.Genres = new SelectList(GenreRepository.Genres, "GenreId", "Name");
            ViewBag.Genres = new SelectList(_context.Genres.ToList(), "GenreId", "Name");
            return View();
        }

        //public IActionResult Create(string Title, string Description, string Director, string imageUrl, int GenreId)
        //Create action metodunda tek tek Movie parametrelerini yazarak almak yerine aşağıdaki gibi direkt Movie movie olarak aldık.
        [HttpPost]
        public IActionResult CreateMovie2(Movie movie)
        {
            // movie.MovieId = MovieRepository.Movies.Count() + 1; //id yi şimdilik bu şekilde artırdık.
            //movie.MovieEmbededVideoLink = "https://www.youtube.com/embed/UhVjp48U2Oc"; //bunları şimdilik böyle ekliyoruz sonra formdan gönderteceğiz

            var entity = _context.Movies.Find(movie.MovieId);

            if (movie != null) //Movie modelimiz içindeki validasyon attiribute gereksinimleri tamamen karşılanmış ise
            {
                //MovieRepository.Add(movie);

                _context.Movies.Add(movie);
                _context.SaveChanges();
                return RedirectToAction("List", "Movies");
                // return RedirectToAction("Index","Home");  //olarak da kullanılabilir
            }

            //ViewBag.Genres = new SelectList(GenreRepository.Genres, "GenreId", "Name");
            ViewBag.Genres = new SelectList(_context.Genres.ToList(), "GenreId", "Name"); //validasyon geçmediğinde create form yeniden
            //oluşturulacak ve bu oluştururken Tür bilgilerini de göndermek gerekiyor. aslında bu satır Create() get metodunda var
            //ama validasyon hata verip sayfa bu Get() post metodundan geri dönünce sayfa ihtiyacı olan film tür değerlerini bulamadığından
            //tür option nesnesinin içi boş geliyor. bu yüzden buraya da ekledik.
            return View();

        }

        public IActionResult ListMovieAsTable(string? q)
        {
            var movies = _context.Movies.AsQueryable();
            if (!string.IsNullOrEmpty(q))
            {
                movies = movies.Include(m => m.Genres).Where //movies objesine atanan tüm filmler içerisinde
                (i =>
                    i.Title.ToLower().Contains(q.ToLower()) ||     //Title içerisinde q değeri varsa veya
                    i.Description.ToLower().Contains(q.ToLower())  // DEscription içerisinde q değeri varsa
                );  //tümünü listeye çevir cünkü movie objesi bir List<Movie> türündedir. //tüm değerleri ToLower diyerek küçük harflere çevirdik ki büyük küçük kelime farkı olmasın arama yaparken

                MovieAdminPanelViewModel m = new MovieAdminPanelViewModel()
                {
                    Movies = movies
                                .Select(m => new AdminMovieModel
                                {
                                    MovieId = m.MovieId,
                                    Title = m.Title,
                                    imageUrl = m.imageUrl,
                                    Genres = m.Genres.ToList()
                                }).ToList()
                };

                return View(m);
            }
            else
            {
                MovieAdminPanelViewModel m = new MovieAdminPanelViewModel()
                {
                    Movies = _context.Movies.Include(m => m.Genres)
                    .Select(m => new AdminMovieModel
                    {
                        MovieId = m.MovieId,
                        Title = m.Title,
                        imageUrl = m.imageUrl,
                        Genres = m.Genres.ToList()
                    })
                    .ToList()
                };

                return View(m);
            }


        }


        public IActionResult ListForEditing(int? id, string q)
        {
            // var movies = MovieRepository.Movies; //movies değerine Movies objesindeki tüm filmleri atadık
            var movies = _context.Movies.AsQueryable();

            //var searchTextValue = HttpContext.Request.Query["q"].ToString(); //bu fonksiyon direkt route parametresi içindeki q değerini getirir.
            //  var searchTextValue = q; //böyle de kullanabilirdik. Bu kullanımda, yukarıda IActionResult List(string q) değerini yazmak gerek
            /*Diğer Örnekler:
            var controller = RouteData.Values["controller"];
            var action = RouteData.Values["action"];
            var id = RouteData.Values["id"];
             */


            if (id != null) //Eğer id null değilse
            {
                movies = movies
                    .Include(m => m.Genres)
                        .Where(m => m.Genres.Any(g => g.GenreId == id));
                //movies objesi içerisinde GenreId'si dışarıdan gelen id değerine eşit olanları model içinde göndereceğiz
            }


            if (!string.IsNullOrEmpty(q))
            {

                movies = movies.Where //movies objesine atanan tüm filmler içerisinde
                (i =>
                    i.Title.ToLower().Contains(q.ToLower()) ||     //Title içerisinde q değeri varsa veya
                    i.Description.ToLower().Contains(q.ToLower())  // DEscription içerisinde q değeri varsa
                );  //tümünü listeye çevir cünkü movie objesi bir List<Movie> türündedir.
            } //tüm değerleri ToLower diyerek küçük harflere çevirdik ki büyük küçük kelime farkı olmasın arama yaparken

            var model = new MoviesViewModel()
            {
                Movies = movies.ToList()
            };

            return View(model);
        }


        public IActionResult UpdateMovie(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            //var movie = MovieRepository.GetById(id);
            var movie = _context.Movies.Select(m => new AdminUpdateMovieModel
            {
                MovieId = m.MovieId,
                Title = m.Title,
                Description = m.Description,
                imageUrl = m.imageUrl,
                Director = m.Director,
                MovieEmbededVideoLink = m.MovieEmbededVideoLink,
                GenreIds = m.Genres.Select(g => g.GenreId).ToArray()

            }).FirstOrDefault(m => m.MovieId == id);

            if (movie == null)
            {
                return NotFound();
            }

            ViewBag.Genres = _context.Genres.ToList();

            return View(movie);
            // return RedirectToAction("Index","Home");  //olarak da kullanılabilir

        }

        [HttpPost]
        public async Task<IActionResult> UpdateMovie(AdminUpdateMovieModel m, IFormFile fileImg)
        {


            if (ModelState.IsValid)
            {


                var movie = _context.Movies.Include(mov => mov.Genres).FirstOrDefault(mov => mov.MovieId == m.MovieId);
                // var movie = _context.Movies.Include("Genres").FirstOrDefault(mov => mov.MovieId == m.MovieId);

                if (movie == null)
                {
                    return NotFound();
                }

                if (movie != null)
                {
                    //  movie.Genres = new List<Genre> { movie.Genres[2] };
                    movie.Title = m.Title;
                    movie.Description = m.Description;
                    movie.Director = m.Director;

                    // movie.imageUrl = m.imageUrl;
                    if (fileImg != null) //resim upload işlemleri
                    {
                        var extension = Path.GetExtension(fileImg.FileName); // dosya uzantısını alır .jpg , .png gibi
                        var uniqFileName = string.Format($"{Guid.NewGuid()}{extension}"); //benzersiz bir resim ismi oluşturduk. NewGuid random benzersiz sayı üretir
                        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img", uniqFileName);
                        movie.imageUrl = uniqFileName;

                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await fileImg.CopyToAsync(stream); //asenkron yaptık bu yüzden action medodu da Task'e çevirdik
                                                               // fileImg.CopyTo(stream); //Olarak da kullanabilirdik
                        }

                    }

                    movie.MovieEmbededVideoLink = m.MovieEmbededVideoLink;
                    movie.Genres = m.GenreIds
                         .Select(id => _context.Genres.FirstOrDefault(i => i.GenreId == id)).ToList();
                    _context.SaveChanges();
                    return RedirectToAction("ListMovieAsTable", "MovieAdminPanel", new
                    {
                        id = movie.MovieId,
                        UpdateStatus = "Success",
                        UpdatedMovie = $"{movie.Title}"
                    });
                    // return RedirectToAction("Index","Home");  //olarak da kullanılabilir

                }

            }

            ViewBag.Genres = _context.Genres.ToList();
            return View(m);

        }

        [HttpPost]
        public IActionResult DeleteMovie(int MovieId)
        {
            //MovieRepository.Delete(MovieId);
            var movie = _context.Movies.Find(MovieId);
            if (movie != null)
            {
                _context.Movies.Remove(movie);
                _context.SaveChanges();
            }
            return RedirectToAction("ListForEditing");
        }

        [HttpPost]
        public IActionResult DeleteMovie2(int movieId)
        {
            var entity = _context.Movies.Find(movieId);
            if (entity != null)
            {
                _context.Movies.Remove(entity);
                _context.SaveChanges();
            }
            return RedirectToAction("ListMovieAsTable");
        }

        public IActionResult ListGenresAsTable()
        {
            var genres = new MovieAdminPanelGenresViewModel
            {
                Genres = _context.Genres.Select(g => new AdminGenreViewModel
                {
                    GenreId = g.GenreId,
                    Name = g.Name,
                    Count = g.Movies.Count
                }).ToList()
            };

            return View(genres);
        }


        [HttpPost]
        public IActionResult CreateGenre(MovieAdminPanelGenresViewModel genre)
        {

            if (ModelState.IsValid)
            {

                Genre g = new Genre
                {
                    Name = genre.Name
                };

                _context.Genres.Add(g);
                _context.SaveChanges();

                return RedirectToAction("ListGenresAsTable", "MovieAdminPanel", new
                {
                    CreatingStatus = "Success",
                    CreatedGenre = $"{g.Name}"
                });

            }


            else
            {
                var genres = new MovieAdminPanelGenresViewModel
                {
                    Genres = _context.Genres.Select(g => new AdminGenreViewModel
                    {
                        GenreId = g.GenreId,
                        Name = g.Name,
                        Count = g.Movies.Count
                    }).ToList()
                };
                return View("ListGenresAsTable", genres);
            }

        }

        public IActionResult UpdateGenre(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entity = _context
                .Genres
                .Select(g => new AdminGenreUpdateViewModel
                {
                    GenreId = g.GenreId,
                    Name = g.Name,
                    Movies = g.Movies.Select(i => new AdminMovieModel
                    {
                        MovieId = i.MovieId,
                        Title = i.Title,
                        imageUrl = i.imageUrl
                    }).ToList()
                })
                .FirstOrDefault(g => g.GenreId == id);

            if (entity == null)
            {
                return NotFound();
            }

            return View(entity);
        }

        [HttpPost]
        public IActionResult UpdateGenre(AdminGenreUpdateViewModel model, int[] movieIds)
        {
            if (ModelState.IsValid)
            {
                var entity = _context.Genres.Include("Movies").FirstOrDefault(i => i.GenreId == model.GenreId);

                if (entity == null)
                {
                    return NotFound();
                }

                entity.Name = model.Name;

                foreach (var id in movieIds)
                {
                    entity.Movies.Remove(entity.Movies.FirstOrDefault(m => m.MovieId == id));
                }

                _context.SaveChanges();

                return RedirectToAction("ListGenresAsTable");
            }

            return View(model);  

        }

        /*   Aşağıda else boğu içindeki kodlar   yukarıdaki metodda yalnlaştırıldı ve
         *  UpgradeGenre.cshtml içinde (frontedde) aynı işe yarayan işlemler yapıldı. farklı senaryoları da denemek için bu şekilde yaptım. aslında
         *  bu Post metodu da yukarıdaki ile aynı işi yapıyor
         [HttpPost]
         public IActionResult UpdateGenre(AdminGenreUpdateViewModel model, int[] movieIds)
        {

            if (ModelState.IsValid)
            {
                var entity = _context.Genres.Include("Movies").FirstOrDefault(i => i.GenreId == model.GenreId);

                if (entity == null)
                {
                    return NotFound();
                }

                entity.Name = model.Name;

                foreach (var i in movieIds)
                {
                    entity.Movies.Remove(entity.Movies.FirstOrDefault(m => m.MovieId == i));
                }

                _context.SaveChanges();

                return RedirectToAction("ListGenresAsTable");
            }

            else
            {

                var m = _context
                   .Genres
                   .Select(g => new AdminGenreUpdateViewModel
                   {
                       GenreId = g.GenreId,
                       Name = model.Name,
                       Movies = g.Movies.Select(i => new AdminMovieModel
                       {
                           MovieId = i.MovieId,
                           Title = i.Title,
                           imageUrl = i.imageUrl
                       }).ToList()
                   })
                   .FirstOrDefault(g => g.GenreId == model.GenreId);
                foreach (var i in movieIds)
                {
                    m.Movies.Remove(m.Movies.FirstOrDefault(m => m.MovieId == i));
                }

                return View(m);
            }

        }
        */


        [HttpPost]
        public IActionResult DeleteGenre(int genreId)
        {
            var entity = _context.Genres.Find(genreId);
            if (entity != null)
            {
                _context.Genres.Remove(entity);
                _context.SaveChanges();
            }
            return RedirectToAction("ListGenresAsTable");
        }

        public IActionResult VerifyMovieExist(string Title)
        {
            var titles = _context.Movies.Select(m => m.Title).ToList();

            if (titles.Any(u => u == Title))
            {
                return Json($" \" {Title} \" başlıklı film daha önce zaten oluşturulmuş.");
            }
            return Json(true);
        }



        


    }
}
