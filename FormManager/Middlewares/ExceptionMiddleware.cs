using FluentValidation;
using FormManager.Api.Responses;
using FormManager.Application.Common.Exceptions;
using FormManager.Application.Common.Models;
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
            catch (ValidationException e)
            {
                List<Error> erros = e.Errors.Select(x => new Error { Title = x.PropertyName, Description = x.ErrorMessage }).ToList();
                ErrorResponse errorResponse = new ErrorResponse("Validation Failed", 400, erros);
                await HandleResponse(context, errorResponse);
            }
            catch (FormMgrException e)
            {               
                ErrorResponse errorResponse = new ErrorResponse(e.Message, e.Status, e.Errors);
                await HandleResponse(context, errorResponse);
            }
            catch (AuthenticateException e)
            {               
                ErrorResponse errorResponse = new ErrorResponse(e.Message, e.Status, e.Errors);
                await HandleResponse(context, errorResponse);
            }
            catch (Exception e)
            {
                ErrorResponse errorResponse = new ErrorResponse("An Error has occurred in server. Please contact the devs.", 500);
                await HandleResponse(context, errorResponse);
            }
        }

        private async Task HandleResponse(HttpContext context, ErrorResponse error)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = error.Status;
            await context.Response.WriteAsync(JsonConvert.SerializeObject(error));
        }
    }
}
