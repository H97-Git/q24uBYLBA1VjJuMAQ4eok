using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;
using PatientHistory.Data;
using PatientHistory.Infrastructure.Repositories;
using PatientHistory.Infrastructure.Services;
using PatientHistory.Internal;
using Serilog;
using System.Linq;
using System.Runtime.InteropServices;

namespace PatientHistory
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            Log.Information("Startup : ConfigureServices()");
            services.AddControllers()
                .AddNewtonsoftJson(options => options.UseMemberCasing());

            services.AddHttpClient();
            services.AddTransient<INoteRepository, NoteRepository>();
            services.AddTransient<INoteService, NoteService>();
            services.AddSingleton<IPatientService, PatientService>();

            services.AddSingleton<IMongoClient>(serviceProvider =>
                new MongoClient(Configuration["NoteDbSettings:ConnectionString"]));
            services.AddScoped(serviceProvider =>
                new NoteContext(serviceProvider.GetRequiredService<IMongoClient>(),
                Configuration["NoteDbSettings:DatabaseName"]));

            services.AddCustomSwagger();

            services.AddMediatR(typeof(Startup).Assembly);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            Log.Information("Startup : Configure()");
            app.UseSerilogRequestLogging();

            var urls = app.ServerFeatures.Get<IServerAddressesFeature>().Addresses;
            Log.Information("URl : {0}", urls.FirstOrDefault(x => x.Contains("https")));

            app.UseCustomExceptionHandler();

            if (env.IsDevelopment())
            {
                //app.UseDeveloperExceptionPage();
                app.UseCustomSwagger();
            }

            //isWindows ?
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                app.UseHttpsRedirection();
            }
            else
            {
                app.Use(async (context, next) =>
                {
                    context.Response.Headers.Add("X-XSS-Protection", "1; mode=block ");
                    context.Response.Headers.Add("Content-Security-Policy", "default-src 'self';");
                    context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
                    await next.Invoke();
                });
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
