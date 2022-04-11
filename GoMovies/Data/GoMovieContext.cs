using GoMovies.Entity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoMovies.Data
{
    public class GoMovieContext: IdentityDbContext<ApplicationUser, ApplicationRole, int>
    {
        public GoMovieContext(DbContextOptions<GoMovieContext> options):base(options)
        {
            this.Database.SetCommandTimeout(999999);
        }

        public DbSet<Movie> Movies { get; set; }    
        public DbSet<Genre> Genres { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<AppUserRole> AppUserRoles { get; set; }
        public DbSet<Person> People { get; set; }
        public DbSet<Crew> Crews { get; set; }
        public DbSet<Cast> Casts { get; set; }
        public DbSet<Comment> Comments { get; set; }


        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer(
        //        //@"Server=(localdb)\ProjectsV13;Database=ProgrammersBlog;Trusted_Connection=True;Connect Timeout=30;MultipleActiveResultSets=True;");
        //        @"Server=tcp:gosunucu.database.windows.net,1433;Initial Catalog=GoMovieDB;Persist Security Info=False;User ID=ozelgoktug;Password=Hangman@33.;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
        //}

    }
}
