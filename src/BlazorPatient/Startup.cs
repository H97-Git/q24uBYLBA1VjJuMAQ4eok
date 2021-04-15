using System.Linq;
using Blazored.LocalStorage;
using BlazorPatient.Infrastructure.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MudBlazor.Services;
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

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            Log.Information("Startup : ConfigureServices() ...");
            services.AddRazorPages();
            services.AddControllersWithViews();
            services.AddServerSideBlazor();
            services.AddMudServices();
            services.AddHttpClient();
            services.AddScoped<IPatientService, PatientService>();
            services.AddScoped<INoteService, NoteService>();
            services.AddBlazoredLocalStorage();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            Log.Information("Startup : ConfigureServices() ...");

            var urls = app.ServerFeatures.Get<IServerAddressesFeature>().Addresses;
            Log.Information("URl : {0}", urls.FirstOrDefault(x => x.Contains("https")));

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
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
