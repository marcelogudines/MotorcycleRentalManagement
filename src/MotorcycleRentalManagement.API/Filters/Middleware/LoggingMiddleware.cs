using Serilog;
using System.Diagnostics;
using System.Text;

namespace MotorcycleRentalManagement.API.Filters.Middleware
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();

            // Captura o corpo da requisição
            var requestBodyContent = await ReadRequestBody(context.Request);

            // Captura a resposta e reescreve o corpo
            var originalBodyStream = context.Response.Body;
            using (var responseBody = new MemoryStream())
            {
                context.Response.Body = responseBody;

                // Executa a próxima etapa do middleware
                await _next(context);

                stopwatch.Stop();

                var statusCode = context.Response.StatusCode;
                var responseBodyContent = await ReadResponseBody(context.Response);

                LogRequestResponse(context, requestBodyContent, responseBodyContent, statusCode, stopwatch.ElapsedMilliseconds);

                // Reescreve o corpo da resposta
                await responseBody.CopyToAsync(originalBodyStream);
            }
        }

        private async Task<string> ReadRequestBody(HttpRequest request)
        {
            request.EnableBuffering();
            var buffer = new byte[Convert.ToInt32(request.ContentLength)];
            await request.Body.ReadAsync(buffer, 0, buffer.Length);
            var bodyAsText = Encoding.UTF8.GetString(buffer);
            request.Body.Position = 0;
            return bodyAsText;
        }

        private async Task<string> ReadResponseBody(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            var text = await new StreamReader(response.Body).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);
            return text;
        }

        private void LogRequestResponse(HttpContext context, string requestBody, string responseBody, int statusCode, long elapsedMilliseconds)
        {
            var logTemplate = @"
            Request Information:
            Scheme: {Scheme}
            Host: {Host}
            Path: {Path}
            QueryString: {QueryString}
            Request Body: {RequestBody}
            Response Status Code: {StatusCode}
            Response Body: {ResponseBody}
            Time Taken: {ElapsedMilliseconds} ms";

            Log.Information(logTemplate,
                context.Request.Scheme,
                context.Request.Host,
                context.Request.Path,
                context.Request.QueryString,
                requestBody,
                statusCode,
                responseBody,
                elapsedMilliseconds);
        }
    }
}
