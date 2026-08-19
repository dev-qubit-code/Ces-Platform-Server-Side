using System.Data.Common;
using System.Text.RegularExpressions;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SPMS_PROJECT.Exceptions;

namespace SPMS_PROJECT.Exceptions;

public class GlobalExceptionHandler(IProblemDetailsService problemDetailsService)
        : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var defaultMessage = "Internal Server Error.";
        
        httpContext.Response.StatusCode = exception switch
        {
            ValidationException  => StatusCodes.Status400BadRequest,
            BusinessRuleException br => br.StatusCode,
            DbUpdateException => CheckForUnique(exception)? StatusCodes.Status409Conflict:StatusCodes.Status500InternalServerError,
            _ => StatusCodes.Status500InternalServerError
        };

        return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails = new ProblemDetails 
            {
                Type = exception.GetType().Name,
                Title = $"Error has occured In: {httpContext.Request.Method} {httpContext.Request.Path}",
                Detail = exception switch
                {
                    ValidationException v => v.Message,
                    DbUpdateException => CheckForUnique(exception)? GetUniquePropertyMessage(exception.InnerException!.Message) : defaultMessage,
                    BusinessRuleException br => br.Message,
                    _ => defaultMessage
                },
            },
        });
    }

    private string GetUniquePropertyMessage(string message)
    {
        var constraint =  Regex.Match(message, @"with unique index '([^']+)'").Groups[1].ToString();

        var property = constraint.Split('_')[2];
        return $"The {property} already exist";
    }

    private bool CheckForUnique(Exception ex) => ex.InnerException is SqlException sqlEx && (sqlEx.Number == 2627 || sqlEx.Number == 2601);
}