using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PatientDemographics.Data;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using System;
using System.Runtime.InteropServices;

namespace PatientDemographics
{
    public class Program
    {
        public static int Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}", theme: AnsiConsoleTheme.Literate)
                .CreateLogger();
            try
            {
                Log.Information("Main : Building web host...");

                var host = CreateHostBuilder(args).Build();

                using var scope = host.Services.CreateScope();
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<PatientContext>();
                    DbInitializer.Initialize(context);
                }
                catch (Exception ex)
                {
                    Log.Fatal(ex, "Main : An error occurred while seeding the database.");
                }

                host.Run();
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Main : Host terminated unexpectedly.");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    bool isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
                    webBuilder.UseStartup<Startup>();
                    if (isWindows)
                    {
                        webBuilder.UseUrls("http://localhost:5000", "https://localhost:5001");
                    }
                    else
                    {
                        webBuilder.UseUrls("http://0.0.0.0:5000", "https://0.0.0.0:5001");
                    }
                });
        }
    }
}
