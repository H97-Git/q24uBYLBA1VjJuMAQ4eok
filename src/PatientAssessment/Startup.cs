using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PatientAssessment.Internal;
using Serilog;
using System.Linq;
using System.Runtime.InteropServices;
using PatientAssessment.Infrastructure.Services;

namespace PatientAssessment
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            Log.Information("Startup : ConfigureServices()");
            services.AddControllers()
                .AddNewtonsoftJson(options => options.UseMemberCasing());
            services.AddHttpClient();
            services.AddSingleton<IAssessmentService, AssessmentService>();
            services.AddCustomSwagger();
            services.AddMediatR(typeof(Startup).Assembly);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            Log.Information("Startup : Configure()");
            app.UseSerilogRequestLogging();

            var urls = app.ServerFeatures.Get<IServerAddressesFeature>().Addresses;
            Log.Information("URl : {0}", urls.FirstOrDefault(x => x.Contains("https")));

            app.UseCustomExceptionHandler();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
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
