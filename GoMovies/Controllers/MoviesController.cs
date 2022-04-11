using GoMovies.Data;
using GoMovies.Entity;
using GoMovies.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Comment = GoMovies.Entity.Comment;

namespace GoMovies.Controllers
{
    public class MoviesController : Controller
    {
        private readonly GoMovieContext _context;
        private readonly RoleManager<ApplicationRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;

        public MoviesController(RoleManager<ApplicationRole> _roleManager, UserManager<ApplicationUser> _userManager, GoMovieContext context)
        {
            roleManager = _roleManager;
            userManager = _userManager;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        // localhost:44328/movies/list/
        // localhost:44328/movies/list/id
        public IActionResult List(int? id, string q) /* id parametresi nullable yaptık. eğer null gelirse tüm filmler listelenecek. 
            id gelirse ise ilgili türe ait filmler listelenecek. ilgili View Movies.cshtml içerisinden çağırılan Genres ViewComponent'tir
            movies içerisine tüm filmleri aldık eğer id değeri null gelmezse bu direkt olarak model içerisinde gönderilecek*/
        {
            //var movies = MovieRepository.Movies; //movies değerine Movies objesindeki tüm filmleri atadık
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
            return View("Movies", model);
        }


        // localhost:44328/movies/details/id
        public IActionResult Details(int id) //int id yazdığımız için model binding olarak _movieListPartial View'inde bulunan butona basıldığında
        {                                     //hreg="movie/details/@Model.MovieId parametresi ile gönderilen route'un son hanesinin id olduğunu bilir ve 
                                              //bu action metodda karşılar. 

            var movie = _context.Movies.AsQueryable();


            movie = movie.Include(x => x.Comments)
                 .Where(p => p.Comments.Any(g => g.MovieId == id));
            var a = movie.Count();
            if (movie.Count() ==0)
            {
                movie = _context.Movies.Where(m=>m.MovieId==id);
            }

            //movie = movie.Include(x => ((x.Comments==null) ? new List<Entity.Comment>() : x.Comments))
            //     .Where(p => p.Comments.Any(g => g.MovieId == id));
            // movie.Include(x => x.Comments).ToList();
            //m.First().Comments.ToList()
            //var  comments = movie.First().Comments.ToList();

            var model = new MovieDetailsViewModel()
            {
                MovieId = movie.First().MovieId,
                Comments = movie.First().Comments,
                Description = movie.First().Description,
                Director = movie.First().Director,
                imageUrl = movie.First().imageUrl,
                Title = movie.First().Title,
                MovieEmbededVideoLink = movie.First().MovieEmbededVideoLink,
                Genres = movie.First().Genres,
                ApplicationUsers = userManager.Users.ToList(),

        };

            return View(model); // buradan da details.cshtml ye bu fonksiyondan dönen modeli yolaldık.
        }



        [HttpPost]
        public IActionResult DeleteComment(string CommentId, string MovieId)
        {

            var comment = _context.Comments.AsQueryable().Where(x => x.CommentId == Convert.ToInt32(CommentId)).FirstOrDefault();

            _context.Remove(comment);
            _context.SaveChanges();


            return RedirectToAction("Details", "Movies", new {id= MovieId });
           
        }


        [HttpPost]
        public IActionResult SendCommentToMovie(MovieDetailsViewModel model)
        {

            var comment = new Entity.Comment
            {
                  Title=model.CommentTittle,
                  commentText=model.CommentText,
                  ApplicationUserId=model.CommentUserId,
                  MovieId = model.MovieId,             
                   commentDate = DateTime.Now.ToString("dd.MM.yyyy")
            };

            var c = _context.Movies.Where(m=>m.MovieId==model.MovieId).Include(x=>x.Comments).First();

            c.Comments.Add(comment);
            _context.SaveChanges();

            return RedirectToAction("Details", "Movies", new { id = model.MovieId });

        }





    }
}
