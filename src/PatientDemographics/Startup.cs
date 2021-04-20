using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using PatientDemographics.Data;
using PatientDemographics.Infrastructure.Repositories;
using PatientDemographics.Infrastructure.Services;
using Serilog;
using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace PatientDemographics
{
    public class Startup
    {
        private readonly bool _isWindows;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            _isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            Log.Information("Startup : ConfigureServices()");

            services.AddControllers()
                .AddNewtonsoftJson(options => options.UseMemberCasing());

            services.AddTransient<IPatientRepository, PatientRepository>();
            services.AddTransient<IPatientService, PatientService>();

            string connectionString = _isWindows ? "PatientDB" : "DockerPatientDb";
            services.AddDbContext<PatientContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString(connectionString)));

            services.AddSwaggerGen(swaggerGenOptions =>
            {
                swaggerGenOptions.CustomSchemaIds(type => type.ToString());

                swaggerGenOptions.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Patient Demographics",
                    Version = "v1",
                    Description = "Patient Demographics API for OpenClassrooms",
                    Contact = new OpenApiContact
                    {
                        Name = "Arno",
                        Email = "arno.demarchi.8@gmail.coim",
                        Url = new Uri("https://github.com/H97-Git/"),
                    },
                });
                //swaggerGenOptions.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                //{
                //    Name = "Authorization",
                //    Type = SecuritySchemeType.ApiKey,
                //    Scheme = "Bearer",
                //    BearerFormat = "JWT",
                //    In = ParameterLocation.Header,
                //    Description = "Enter 'Bearer' [space] and then your valid token in the text input below.\r\n\r\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpX....\"",
                //});

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                swaggerGenOptions.IncludeXmlComments(xmlPath);

                //c.AddSecurityRequirement(new OpenApiSecurityRequirement
                //{
                //    {
                //        new OpenApiSecurityScheme
                //        {
                //            Reference = new OpenApiReference
                //            {
                //                Type = ReferenceType.SecurityScheme,
                //                Id = "Bearer"
                //            }
                //        },
                //        Array.Empty<string>()
                //    }
                //});
            });

            services.AddMediatR(typeof(Startup).Assembly);

            //services.AddHttpsRedirection(options =>
            //{
            //    options.RedirectStatusCode = StatusCodes.Status307TemporaryRedirect;
            //    options.HttpsPort = 5001;
            //});
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            Log.Information("Startup : Configure()");
            app.UseSerilogRequestLogging();

            var urls = app.ServerFeatures.Get<IServerAddressesFeature>().Addresses;
            foreach (string item in urls)
            {
                Log.Information("URl : {0}", item);
            }


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
                        case "KeyNotFoundException":
                            await context.Response
                                .WriteAsJsonAsync(new { Error = "Resources not found in the system.", Id = ex.Message });
                            Log.Error(ex, "Resources not found.");
                            return;
                        case "ValidationException":
                            var validationException = (ValidationException)ex;
                            Log.Error(ex.Message, "Validation Exception");
                            await context.Response
                                .WriteAsJsonAsync(new { Errors = validationException.Errors });
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
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Patient Demographics v1"));
            }

            if (_isWindows)
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
