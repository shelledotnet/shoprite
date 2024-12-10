using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ShopRite.Application.Dto;
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
            try
			{
				await _next(context);
			}
			catch (DbUpdateException ex)
			{
				if(ex.InnerException is SqlException innerException)
				{
					switch (innerException.Number)
					{
						case 2627: //unique constraint violation
                            ServiceFailedResponse serviceFail = new ServiceFailedResponse(
                               System.Net.HttpStatusCode.Conflict, false, "Unique constraint violation");
                            context.Response.StatusCode=StatusCodes.Status409Conflict;
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
                    ServiceFailedResponse serviceFailed = new ServiceFailedResponse(
                  System.Net.HttpStatusCode.Conflict, false, "An error occurred while saving the entity changes. ");
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    await context.Response.WriteAsync(JsonSerializer.Serialize(serviceFailed));
                }
			}
			catch(Exception ex)
			{
                ServiceFailedResponse serviceFailed = new ServiceFailedResponse(
   System.Net.HttpStatusCode.Conflict, false, "An error occurred . Please contact support . ");
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsync(JsonSerializer.Serialize(serviceFailed));
            }
        }
    }
}
