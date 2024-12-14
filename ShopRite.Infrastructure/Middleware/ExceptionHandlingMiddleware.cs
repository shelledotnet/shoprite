using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ShopRite.Application.Dto;
using ShopRite.Application.Services.Interfaces.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ShopRite.Infrastructure.Middleware
{
    public class ExceptionHandlingMiddleware(RequestDelegate _next)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            var date = DateTime.Now.ToString("yyyyMMddHHmmssfffffff");
            context.Response.Headers["correlationId"] = date;

            try
            {
                await _next(context);
            }
            catch (DbUpdateException ex)
            {
                var logger = context.RequestServices.GetRequiredService<IAppLogger<ExceptionHandlingMiddleware>>();
                if (ex.InnerException is SqlException innerException)
                {
                    logger.LogError(innerException, $"Sql Exception correlationId:::{date}");


                    switch (innerException.Number)
                    {
                        case 2627: //unique constraint violation
                            ServiceFailedResponse serviceFail = new ServiceFailedResponse(
                               System.Net.HttpStatusCode.Conflict, false, "Unique constraint violation");
                            context.Response.StatusCode = StatusCodes.Status409Conflict;
                            await context.Response.WriteAsync(JsonSerializer.Serialize(serviceFail));
                            break;
                        case 515: //cannot insert null
                            ServiceFailedResponse serviceFaile = new ServiceFailedResponse(
                               System.Net.HttpStatusCode.Conflict, false, "cannot insert null");
                            context.Response.StatusCode = StatusCodes.Status400BadRequest;
                            await context.Response.WriteAsync(JsonSerializer.Serialize(serviceFaile));
                            break;
                        case 547: //foreign key constraint
                            ServiceFailedResponse serviceFailedResponse = new ServiceFailedResponse(
                                System.Net.HttpStatusCode.Conflict, false, "foreign key constraint violation");
                            context.Response.StatusCode = StatusCodes.Status409Conflict;
                            await context.Response.WriteAsync(JsonSerializer.Serialize(serviceFailedResponse));
                            break;
                        default:
                            ServiceFailedResponse serviceFailed = new ServiceFailedResponse(
                               System.Net.HttpStatusCode.Conflict, false, "an error occurred while processing your request");
                            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                            await context.Response.WriteAsync(JsonSerializer.Serialize(serviceFailed));
                            break;
                    }
                }
                else
                {
                    logger.LogError(ex, "Related EFCore Exception");

                    ServiceFailedResponse serviceFailed = new ServiceFailedResponse(
                  System.Net.HttpStatusCode.Conflict, false, "An error occurred while saving the entity changes. ");
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    await context.Response.WriteAsync(JsonSerializer.Serialize(serviceFailed));
                }
            }
            catch (Exception ex)
            {
                {
                    context.RequestServices.GetRequiredService<IAppLogger<ExceptionHandlingMiddleware>>()
                                           .LogError(ex, "Unknown Exception");

                    ServiceFailedResponse serviceFailed = new ServiceFailedResponse(
   System.Net.HttpStatusCode.Conflict, false, "An error occurred . Please contact support . ");
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    await context.Response.WriteAsync(JsonSerializer.Serialize(serviceFailed));
                }
            }
        }
    }
}