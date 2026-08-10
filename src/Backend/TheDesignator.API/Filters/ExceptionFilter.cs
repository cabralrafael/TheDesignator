using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TheDesignator.Communication.Responses;
using TheDesignator.Exception.ExceptionsBase;
using TheDesignator.Exception;

namespace TheDesignator.API.Filters;

public class ExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if(context.Exception is TheDesignatorException theDesignatorException)
        {
            context.HttpContext.Response.StatusCode = (int)theDesignatorException.GetStatusCode();
            context.Result = new ObjectResult(new ResponseErrorJson(theDesignatorException.GetErrorMessages()));
        }
        else
        {
            context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Result = new ObjectResult(new ResponseErrorJson(ResourceMessagesException.UNKNOW_ERROR));
        }
    }
}
