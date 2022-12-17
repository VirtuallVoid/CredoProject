using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using ProjectMvc.Classes;
using Serilog;
using Serilog.Formatting.Compact;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProjectMVC.Middlewares
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IOptions<Settings> _setting;

        public ErrorHandlerMiddleware(RequestDelegate next, IOptions<Settings> setting)
        {
            _next = next;
            _setting = setting;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                Log.Logger = new LoggerConfiguration()
                 .WriteTo.File(new CompactJsonFormatter(), _setting.Value.WebRoot.Replace(@"\\", @"\"))
                 .CreateLogger();

                Log.Warning(error, error?.Message);
                var response = context.Response;
                response.ContentType = "application/json";
                var responseModel = new Response<decimal>() { Data = 0m, Succeeded = false, Message = error?.Message };
                var result = JsonSerializer.Serialize(responseModel);

                await response.WriteAsync(result);
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}
