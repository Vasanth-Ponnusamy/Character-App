using CharacterApp.Data.Context;
using CharacterApp.Data.Repository;
using CharacterApp.Services;
using CharacterApp.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace CharacterApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddScoped<ICharacterService, CharacterService>();
            builder.Services.AddScoped<ICharacterRepository, CharacterRepository>();

            builder.Services.AddControllersWithViews();

            builder.Services.AddMemoryCache(); 

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Characters}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
