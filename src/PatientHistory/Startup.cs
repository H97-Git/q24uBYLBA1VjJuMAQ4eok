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

namespace PatientHistory
{
    public class Startup
    {
        private readonly IWebHostEnvironment _env;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            _env = env;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            Log.Debug("Startup : ConfigureServices()");
            services.AddControllers()
                .AddNewtonsoftJson(options => options.UseMemberCasing()); ;

            services.AddHttpClient();
            services.AddTransient<INoteRepository, NoteRepository>();
            services.AddTransient<INoteService, NoteService>();
            services.AddSingleton<IPatientService, PatientService>();

            string connectionString = _env.IsDevelopment()
                ? Configuration["NoteDbSettings:ConnectionStringW"]
                : Configuration["NoteDbSettings:ConnectionStringL"];

            services.AddSingleton<IMongoClient>(serviceProvider =>
                new MongoClient(connectionString));

            services.AddScoped(serviceProvider =>
                new NoteContext(serviceProvider.GetRequiredService<IMongoClient>(),
                Configuration["NoteDbSettings:DatabaseName"]));

            services.AddCors();
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
                app.UseHttpsRedirection();
                app.UseDeveloperExceptionPage();
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
