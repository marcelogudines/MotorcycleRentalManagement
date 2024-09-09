using MotorcycleRentalManagement.API.Models.Request;
using Serilog;

namespace MotorcycleRentalManagement.API.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Ocorreu uma exceção não tratada.");
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status400BadRequest;

            var response = new DefaultResponse
            {
                Mensagem = "Ocorreu um erro ao processar sua solicitação."
            };

            return context.Response.WriteAsJsonAsync(response);
        }
    }
}
