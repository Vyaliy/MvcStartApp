using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MvcStartApp.Models.AppContext;
using MvcStartApp.Models.Repositories;
using WebApplication2.Middleware;

namespace MvcStartApp
{
    public class Program
    {
        static IWebHostEnvironment _env;
        static IConfiguration _configuration;

        public static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            _configuration = builder.Configuration;
            ConfigureServices(builder.Services);

            var app = builder.Build();

            _env = app.Environment;
            

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // Подключаем логирвоание с использованием ПО промежуточного слоя
            app.UseMiddleware<LoggingMiddleware>();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                    name: "RequestHistory",
                    pattern: "RequestHistory",
                    defaults: new { controller = "RequestHistory", action = "Index" });
            app.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            app.
            app.Run();
        }
        public static void ConfigureServices(IServiceCollection services)
        {
            string connection = _configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<BlogContext>(options => options.UseSqlServer(connection), ServiceLifetime.Singleton);
            // регистрация сервиса репозитория для взаимодействия с базой данных
            services.AddTransient<IRequestRepository, RequestRepository>();
            services.AddTransient<IBlogRepository, BlogRepository>();
            services.AddControllersWithViews();
            
        }
    }
}