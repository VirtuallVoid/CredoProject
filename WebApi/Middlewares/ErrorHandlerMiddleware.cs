using Application.Exceptions;
using Application.Wrappers;
using Microsoft.AspNetCore.Http;
using Serilog;
using Serilog.Formatting.Compact;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace WebApi.Middlewares
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        public ErrorHandlerMiddleware(RequestDelegate next, ILogger logger)
        {
            _next = next;
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
                  .WriteTo.File(new CompactJsonFormatter(), "log.txt")
                  .CreateLogger();

                Log.Warning(error, error?.Message);
                var response = context.Response;
                response.ContentType = "application/json";
                var responseModel = new Response<int?>() { Data = null, Succeeded = false, Message = error?.Message };

                switch (error)
                {
                    case ApiException e:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        responseModel.Code = e.Code;
                        break;
                    case ValidationException e:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        responseModel.Error = e.Error;
                        responseModel.Code = Convert.ToInt32(e.Code);
                        break;
                    case KeyNotFoundException e:
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;
                    default:
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }
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
