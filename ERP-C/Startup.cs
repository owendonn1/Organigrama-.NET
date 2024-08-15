using ERP_C.Data;
using ERP_C.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace ERP_C
{
    public static class Startup
    {
        public static WebApplication InicializarApp(string[] args)
        {
            //creo nueva instancia de servidor web
            var builder = WebApplication.CreateBuilder(args);
            ConfigureServices(builder); //lo configuro con sus servicios

            var app = builder.Build(); //sobre esta app insertamos los midddleware
            Configure(app); // configuramos los midleware
            
            
            return app; //retorno la app inicializada
        }

        private static void ConfigureServices(WebApplicationBuilder builder)
        {
            //builder.Services.AddDbContext<BDContext>(options => options.UseInMemoryDatabase("BDContext"));
            builder.Services.AddDbContext<BDContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("BDContext.sql")));
            builder.Services.AddIdentity<Persona, Rol>().AddEntityFrameworkStores<BDContext>();

            builder.Services.Configure<IdentityOptions>(opciones =>
            {
                opciones.Password.RequireNonAlphanumeric = false;
                opciones.Password.RequireLowercase = false;
                opciones.Password.RequiredUniqueChars = 0;
                opciones.Password.RequireUppercase = false;
           }
                );
          

            builder.Services.PostConfigure<CookieAuthenticationOptions>(IdentityConstants.ApplicationScheme, options =>
            {
                options.LoginPath = "/Account/IniciarSesion";
                options.AccessDeniedPath = "/Account/AccesoDenegado";
                options.Cookie.Name = "IdentidadERP-CApp";
            });
                

            builder.Services.AddControllersWithViews();
        }

        private static void Configure(WebApplication app)
        {
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            using(var serviceScop = app.Services.GetService<IServiceScopeFactory>().CreateScope())
            {
                var contexto = serviceScop.ServiceProvider.GetRequiredService<BDContext>(); 
                contexto.Database.Migrate();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
        }
    }
}
