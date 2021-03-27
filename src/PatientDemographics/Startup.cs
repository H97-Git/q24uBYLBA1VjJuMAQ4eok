using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
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
using MediatR;

namespace PatientDemographics
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

            services.AddControllers();

            services.AddTransient<IPatientRepository, PatientRepository>();
            services.AddTransient<IPatientService, PatientService>();

            services.AddDbContext<PatientContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("PatientDB")));

            services.AddSwaggerGen(swaggerGenOptions =>
            {
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
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            Log.Information("Startup : Configure()");
            app.UseSerilogRequestLogging();

            app.UseExceptionHandler(builder =>
            {
                builder.Run(async context =>
                {
                    var ex = context.Features.Get<IExceptionHandlerPathFeature>().Error;
                    string exType = ex.GetType().Name;
                    switch (exType)
                    {
                        case "KeyNotFoundException":
                            await context.Response
                                .WriteAsJsonAsync(new { Error = "Patient not found.", PatientId = ex.Message });
                            Log.Error(ex, "Patient not found.");
                            break;
                        case "ValidationException":
                            var validationException = (ValidationException)ex;
                            Log.Error(ex.Message);
                            await context.Response
                                .WriteAsJsonAsync(new { validationException.Errors });
                            break;
                        default:
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
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
