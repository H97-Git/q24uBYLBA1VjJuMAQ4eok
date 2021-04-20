using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Serilog;

namespace PatientHistory.Internal
{
    public class NoteExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _request;

        public NoteExceptionHandlerMiddleware(RequestDelegate request)
        {
            _request = request;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _request(context);
            }
            catch (Exception ex)
            {
                Log.Error("An error has occurred.",ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var code = HttpStatusCode.InternalServerError;
            var result = string.Empty;

            switch (ex)
            {
                case KeyNotFoundException:
                    code = HttpStatusCode.NotFound;
                    break;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int) code;
            if (string.IsNullOrEmpty(result))
            {
                result = JsonConvert.SerializeObject(new {error = ex.Message});

            }

            return context.Response.WriteAsync(result);
        }
    }
}
