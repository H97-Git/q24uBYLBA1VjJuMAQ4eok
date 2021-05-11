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
            Log.Debug("Startup : ConfigureServices() ...");

            services.AddCustomBlazorService();

            services.AddHttpClient();
            services.AddScoped<IPatientService, PatientService>();
            services.AddScoped<INoteService, NoteService>();
            services.AddScoped<IAssessmentService, AssessmentService>();
            services.AddBlazoredLocalStorage();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            Log.Debug("Startup : Configure() ...");
            Log.Debug($"EnvironmentName : {env.EnvironmentName}");
            app.UseSerilogRequestLogging();

            var urls = app.ServerFeatures.Get<IServerAddressesFeature>().Addresses;
            foreach (string item in urls)
            {
                Log.Debug("URl : {0}", item);
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseHttpsRedirection();
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
