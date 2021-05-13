using System;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PatientDemographics.Data;
using PatientDemographics.Infrastructure.Repositories;
using PatientDemographics.Infrastructure.Services;
using PatientDemographics.Internal;
using Serilog;

namespace PatientDemographics
{
    public class Startup
    {
        private readonly IWebHostEnvironment _env;
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            _env = env;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            Log.Debug("Startup : ConfigureServices()");

            services.AddControllers()
                .AddNewtonsoftJson(options => options.UseMemberCasing());;

            services.AddTransient<IPatientRepository, PatientRepository>();
            services.AddTransient<IPatientService, PatientService>();

            string connectionString = Configuration.GetConnectionString(_env.IsDevelopment() ? "PatientDB" : "DockerPatientDb");
            services.AddDbContext<PatientContext>(options => options.UseSqlServer(connectionString));

            services.AddCustomSwagger();
            services.AddMediatR(typeof(Startup).Assembly);
            services.AddCors();
        }

        public void Configure(IApplicationBuilder app)
        {
            Log.Debug("Startup : Configure()");
            Log.Debug($"EnvironmentName : {_env.EnvironmentName}");
            app.UseSerilogRequestLogging();

            var urls = app.ServerFeatures.Get<IServerAddressesFeature>().Addresses;
            foreach (string item in urls)
            {
                Log.Debug("URl : {0}", item);
            }

            app.UseCustomExceptionHandler();
            app.UseCustomSwagger();

            if (_env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseHttpsRedirection();
            }

            app.UseCors();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
