using GoMovies.Data;
using GoMovies.Entity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoMovies
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
             services.AddDbContext<GoMovieContext>(options => 
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, ApplicationRole>()
              .AddEntityFrameworkStores<GoMovieContext>()
              .AddDefaultTokenProviders(); //parola veya email değiştirirken token üretsin diye bunu kullanıyoruz. bu bize yardımcı olur.

            services.AddControllersWithViews() //bu donksiyon ile projemize MCV pattern özelliği kazandırdık. Artık Model View ve Controller
                                               //klasörlerimizi oluşturarak MVC paternine uygun bir proje inşa edebiliriz.
                        .AddViewOptions(options => options.HtmlHelperOptions.ClientValidationEnabled = true); // bu option metod false iken Frontenddeki validasyonlar kapatılır. true iken açılır.

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
               DataSeeding.Seed(app);
            }

            app.UseStaticFiles(); /*wwwroot altındaki resim, js ve class kütüphanelerine ulaşmak için  app.UseStaticFiles(); fonksiyonunu 
                                   * çağırarak bu dizine erişimin statik dosyalara ulaşımı sağlamamız gerekiyor.Aksi taktirde view sayfalarımızda 
                                   * tanımlanan statik dosyalara erişemez ve stiller veya resimler çalışmaz. '*/

            app.UseRouting();


            app.UseAuthentication();// servislerde yazılan Identitiy servislerinin çalışması için bu fonksiyonu yazıyoruz.
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                   name: "dafault",
                   //areaName: "Admin",
                   //pattern: "Admin/{controller=Home}/{action=Index}/{id?}"
                   pattern: "{controller=Home}/{action=Index}/{id?}"

                   );
            });
        }
    }
}
