using Laminal.Server.Models;
using Laminal.Server.Services;
using Laminal.Shared.Services;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Stl.Fusion;
using Stl.Fusion.EntityFramework;
using Stl.Fusion.Server;
using Stl.Time;

namespace Laminal
{
    public class Program
    {

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllersWithViews();
                //.AddJsonOptions(options => 
                //{
                //    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
                //});
            builder.Services.AddRazorPages();

            builder.Services.AddDbContextFactory<AppDbContext>(db => {
                //db.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
                db.UseSqlite("Filename=LocalDatabase.db");
            });
            // AddDbContextServices is just a convenience builder allowing
            // to omit DbContext type in misc. normal and extension methods 
            // it has
            builder.Services.AddDbContextServices<AppDbContext>(db => {
                db.AddEntityResolver<int, Shared.Models.Task>(_ => new()
                {
                    QueryTransformer = task => task.Include(c => c.Properties)
                });
                db.AddEntityResolver<int, Shared.Models.TaskProperty>();
                db.AddEntityResolver<int, Shared.Models.Project>(_ => new()
                {
                    QueryTransformer = project => project.Include(c => c.Tasks)
                });
                db.AddEntityResolver<int, Shared.Models.Resource>();

                // Uncomment if you'll be using AddRedisOperationLogChangeTracking 
                // db.AddRedisDb("localhost", "Fusion.Tutorial.Part10");

                // This call enabled Operations Framework (OF) for AppDbContext. 
                db.AddOperations(operations => {
                    operations.ConfigureOperationLogReader(_ => new()
                    {
                        // We use FileBasedDbOperationLogChangeTracking, so unconditional wake up period
                        // can be arbitrary long - all depends on the reliability of Notifier-Monitor chain.
                        // See what .ToRandom does - most of timeouts in Fusion settings are RandomTimeSpan-s,
                        // but you can provide a normal one too - there is an implicit conversion from it.
                        UnconditionalCheckPeriod = TimeSpan.FromSeconds(60/*Env.IsDevelopment() ? 60 : 5*/).ToRandom(0.05),
                    });
                    // And this call tells that hosts will use a shared file
                    // to "message" each other that operation log was updated.
                    // In fact, they'll just be "touching" this file once
                    // this happens and watch for change of its modify date.
                    // You shouldn't use this mechanism in real multi-host
                    // scenario, but it works well if you just want to test
                    // multi-host invalidation on a single host by running
                    // multiple processes there.
                    operations.AddFileBasedOperationLogChangeTracking();

                    // Or, if you use PostgreSQL, use this instead of above line
                    // operations.AddNpgsqlOperationLogChangeTracking();

                    // Or, if you use Redis, use this instead of above line
                    // operations.AddRedisOperationLogChangeTracking();
                });
            });

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

            app.UseWebSockets(new WebSocketOptions()
            {
                KeepAliveInterval = TimeSpan.FromSeconds(30), // Just in case
            });
            app.UseRouting();
            app.MapFusionWebSocketServer();
            app.MapRazorPages();
            app.MapControllers();
            app.MapFallbackToFile("index.html");

            app.Run();
        }
    }
}