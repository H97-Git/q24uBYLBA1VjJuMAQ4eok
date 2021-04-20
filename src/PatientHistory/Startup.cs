using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using PatientHistory.Data;
using PatientHistory.Infrastructure.Repositories;
using PatientHistory.Infrastructure.Services;
using Serilog;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Hosting.Server.Features;

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

            services.AddSwaggerGen(swaggerGenOptions =>
            {
                swaggerGenOptions.CustomSchemaIds(type => type.ToString());

                swaggerGenOptions.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Patient History",
                    Version = "v1",
                    Description = "Patient History API for OpenClassrooms",
                    Contact = new OpenApiContact
                    {
                        Name = "Arno",
                        Email = "arno.demarchi.8@gmail.coim",
                        Url = new Uri("https://github.com/H97-Git/"),
                    },
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                swaggerGenOptions.IncludeXmlComments(xmlPath);
            });
            services.AddMediatR(typeof(Startup).Assembly);
            //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(NoteValidatorBehaviour<,>));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            Log.Information("Startup : Configure()");
            app.UseSerilogRequestLogging();

            var urls = app.ServerFeatures.Get<IServerAddressesFeature>().Addresses;
            Log.Information("URl : {0}", urls.FirstOrDefault(x => x.Contains("https")));


            app.UseExceptionHandler(builder =>
            {
                builder.Run(async context =>
                {
                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    var ex = context.Features.Get<IExceptionHandlerPathFeature>().Error;
                    string exType = ex.GetType().Name;
                    switch (exType)
                    {
                        case "MongoWriteException":
                            await context.Response
                                .WriteAsJsonAsync(new { Error = "Mongo Exception", ex.Message });
                            Log.Error(ex, "MongoWriteException");
                            return;
                        case "KeyNotFoundException":
                            await context.Response
                                .WriteAsJsonAsync(new { Error = "Resources not found in the system.", Id = ex.Message });
                            Log.Error(ex, "Resources not found in the system.");
                            return;
                        case "ValidationException":
                            var validationException = (ValidationException)ex;
                            await context.Response
                                .WriteAsJsonAsync(new { validationException.Errors });
                            Log.Error(ex.Message, "Validation Exception");
                            return;
                        default:
                            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                            await context.Response
                                .WriteAsJsonAsync(new { Error = "An unknown error has occurred." });
                            return;
                    }
                });
            });

            if (env.IsDevelopment())
            {
                //app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PatientHistory v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            //app.UseMiddleware<NoteExceptionHandlerMiddleware>();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
