using Laminal.Server.Models;
using Laminal.Server.Services;
using Laminal.Shared.Services;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Stl.Fusion;
using Stl.Fusion.Server;

namespace Laminal
{
    public class Program
    {

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();

            var cnn = new SqliteConnection("Filename=:memory:");
            cnn.Open();
            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                //options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));                
                options.UseSqlite(cnn);
            });

            builder.Services.AddScoped<ITaskService, TaskService>();

            // Fusion:
            var fusion = builder.Services.AddFusion();
            fusion.AddWebServer();
            // Registering Compute Service
            fusion.AddComputeService<ITaskService, TaskService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if(app.Environment.IsDevelopment())
            {
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.MapRazorPages();
            app.MapControllers();
            app.MapFallbackToFile("index.html");

            app.Run();
        }
    }
}