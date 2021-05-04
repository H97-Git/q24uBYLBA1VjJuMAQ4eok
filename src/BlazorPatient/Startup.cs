using Blazored.LocalStorage;
using BlazorPatient.Infrastructure.Services;
using BlazorPatient.Internal;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace BlazorPatient
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
            Log.Information("Startup : ConfigureServices() ...");

            services.AddCustomBlazorService();

            services.AddHttpClient();
            services.AddScoped<IPatientService, PatientService>();
            services.AddScoped<INoteService, NoteService>();
            services.AddScoped<IAssessmentService, AssessmentService>();
            services.AddBlazoredLocalStorage();

            //services.AddHttpsRedirection(options =>
            //{
            //    options.RedirectStatusCode = StatusCodes.Status307TemporaryRedirect;
            //    options.HttpsPort = 5005;
            //});
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            Log.Information("Startup : ConfigureServices() ...");

            var urls = app.ServerFeatures.Get<IServerAddressesFeature>().Addresses;
            foreach (string item in urls)
            {
                Log.Information("URl : {0}", item);
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseHttpsRedirection();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
                app.Use(async (context, next) =>
                {
                    context.Response.Headers.Add("X-XSS-Protection", "1; mode=block ");
                    context.Response.Headers.Add("Content-Security-Policy", "default-src 'self';");
                    context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
                    await next.Invoke();
                });
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
