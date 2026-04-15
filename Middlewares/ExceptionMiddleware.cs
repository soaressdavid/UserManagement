using System.Net;
using System.Text.Json;
using UserManagement.API.Exceptions;
using UserManagement.API.Utils;

namespace UserManagement.API.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (NotFoundException ex)
            {
                await Handle(context, ex.Message, HttpStatusCode.NotFound);
            }
            catch (BadRequestException ex)
            {
                await Handle(context, ex.Message, HttpStatusCode.BadRequest);
            }
            catch (Exception)
            {
                await Handle(context, "Erro interno", HttpStatusCode.InternalServerError);
            }
        }

        private static async Task Handle(HttpContext context, string message, HttpStatusCode status)
        {
            context.Response.StatusCode = (int)status;
            context.Response.ContentType = "application/json";

            var response = ApiResponse<string>.Failure(message);
            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
