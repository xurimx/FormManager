using FormManager.Api.Responses;
using FormManager.Application.Common.Exceptions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace FormManager.Api.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception e)
            {
                if (e is IBaseException baseException)
                {
                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = baseException.Status;
                    ErrorResponse errorResponse = new ErrorResponse(e.Message, baseException.Status, baseException.Errors);
                    await context.Response.WriteAsync(JsonConvert.SerializeObject(errorResponse));
                }
                else
                {
                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    ErrorResponse errorResponse = new ErrorResponse("An Error has occurred in server. Please contact the devs.", 500);
                    await context.Response.WriteAsync(JsonConvert.SerializeObject(errorResponse));
                }
            }
        }
    }
}
