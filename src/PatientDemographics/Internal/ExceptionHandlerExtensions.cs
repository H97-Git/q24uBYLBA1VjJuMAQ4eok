using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace PatientDemographics.Internal
{
    public static class ExceptionHandlerExtensions
    {
        public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder app)
        {
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
                        case "ArgumentNullException":
                            await context.Response
                                .WriteAsJsonAsync(new { Error = "Argument was null.", Arg = ex.Message });
                            Log.Error(ex, "Resources not found.");
                            return;
                        case "KeyNotFoundException":
                            await context.Response
                                .WriteAsJsonAsync(new { Error = "Resources not found in the system.", Id = ex.Message });
                            Log.Error(ex, "Resources not found.");
                            return;
                        case "ValidationException":
                            var validationException = (ValidationException)ex;
                            Log.Error(ex.Message, "Validation Exception");
                            await context.Response
                                .WriteAsJsonAsync(new { Error = "Validation Exception", validationException.Errors });
                            return;
                        default:
                            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                            await context.Response
                                .WriteAsJsonAsync(new { Error = "An unknown error has occurred." });
                            return;
                    }
                });
            });
            return app;
        }
    }
}
