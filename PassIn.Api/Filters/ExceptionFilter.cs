using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PassIn.Communication.Responses;
using PassIn.Exceptions;
using System.Net;

namespace PassIn.Api.Filters
{
    public class ExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var result = context.Exception is PassInException; 

            if (result)
            {
                HandleProjectException();
            }
            else
            {
                ThrowUnknowError(context);
            }
        }

        private void HandleProjectException(ExceptionContext context)
        {
            context.HttpContext.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
            context.Result = new ObjectResult(new ResponseErrorJson(context.Exception.Message));
        }

        private void ThrowUnknowError(ExceptionContext context)
        {
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Result = new ObjectResult(new ResponseErrorJson("Unknown error"));
        }
    }
}
