using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PatientAssessment.Infrastructure.Services;
using PatientAssessment.Internal;
using Serilog;

namespace PatientAssessment
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
            Log.Debug("Startup : ConfigureServices()");
            services.AddControllers()
                .AddNewtonsoftJson(options => options.UseMemberCasing());;
            services.AddHttpClient();
            services.AddSingleton<IAssessmentService, AssessmentService>();
            services.AddCustomSwagger();
            services.AddMediatR(typeof(Startup).Assembly);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            Log.Debug("Startup : Configure()");
            Log.Debug($"EnvironmentName : {env.EnvironmentName}");
            app.UseSerilogRequestLogging();

            var urls = app.ServerFeatures.Get<IServerAddressesFeature>().Addresses;
            foreach (string item in urls)
            {
                Log.Debug("URl : {0}", item);
            }

            app.UseCustomExceptionHandler();
            app.UseCustomSwagger();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseHttpsRedirection();
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
