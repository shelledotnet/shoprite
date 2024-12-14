using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ShopRite.Infrastructure.Middleware
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggingMiddleware> _logger;

        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Add client IP and machine name to response headers
            AddCustomHeaders(context);

            // Log the incoming request
            await LogRequestAsync(context);

            // Log the outgoing response
            await LogResponseAsync(context);
        }

        private void AddCustomHeaders(HttpContext context)
        {
            // Get client IP
            var clientIp = context.Connection.RemoteIpAddress?.ToString() ?? "Unknown";

            // Get machine name
            var machineName = Environment.MachineName;

            // Add headers to the response
            context.Response.Headers["Client-IP"] = clientIp;
            context.Response.Headers["Machine-Name"] = machineName;

            _logger.LogInformation("Client IP: {ClientIp}, Machine Name: {MachineName}", clientIp, machineName);
        }

        private async Task LogRequestAsync(HttpContext context)
        {
            context.Request.EnableBuffering(); // Allow reading the request body multiple times
            var requestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();

            _logger.LogInformation("Incoming Request: Method:{Method} Path:{Path}, Headers: {Headers}, Body: {Body}",
                context.Request.Method,
                context.Request.Path,
                context.Request.Headers,
                requestBody);

            context.Request.Body.Position = 0; // Reset the request body stream position
        }

        private async Task LogResponseAsync(HttpContext context)
        {
            var originalResponseBodyStream = context.Response.Body;

            await using var memoryStream = new MemoryStream();
            context.Response.Body = memoryStream;

     

            try
            {
                await _next(context); // Continue the middleware pipeline

                memoryStream.Position = 0;
                var responseBody = await new StreamReader(memoryStream).ReadToEndAsync();


                // Deserialize the response body
                var responseJson = JsonSerializer.Deserialize<Dictionary<string, object>>(responseBody);

                // Extract specific properties
                if (responseJson != null)
                {
                    responseJson.TryGetValue("code", out var code);
                    responseJson.TryGetValue("message", out var message);
                    responseJson.TryGetValue("success", out var success);

                    // Log the specific properties

                    _logger.LogInformation("Outgoing Response: StatusCode:{StatusCode}, Headers: {Headers}, Body: {Body},Code: {Code}, Success: {success}, Message: {Message}",
                context.Response.StatusCode,
                context.Response.Headers,
                responseBody, code ?? "N/A", success ?? "N/A", message ?? "N/A");
                }
            }
            catch (JsonException ex)
            {
                // Log if the response body is not a valid JSON
                _logger.LogWarning(ex, "Response body is not a valid JSON:");
            }
            finally
            {
                // Reset the response stream
                memoryStream.Position = 0;
                await memoryStream.CopyToAsync(originalResponseBodyStream);
                context.Response.Body = originalResponseBodyStream; // Restore the original stream
            }





            await memoryStream.CopyToAsync(originalResponseBodyStream); // Write response back to the original stream
            context.Response.Body = originalResponseBodyStream; // Restore the original response body stream
        }
    }


}
