using FluentValidation;
using Microsoft.Data.SqlClient;
using SoccerPro.Application.Common.ResultPattern;
using System.Net;
using System.Text.Json;

public class GlobalExeptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExeptionHandlingMiddleware> _logger;

    public GlobalExeptionHandlingMiddleware(RequestDelegate next, ILogger<GlobalExeptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception error)
        {
            await HandleExceptionAsync(context, error);
            _logger.LogError(error, error.Message);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception error)
    {
        var response = context.Response;
        response.ContentType = "application/json";

        // default values
        var statusCode = HttpStatusCode.InternalServerError;
        var apiError = new ApiResponse<string>
        {
            Succeeded = false,
            Message = "An unexpected error occurred. Please try again later.",
            Errors = [error.Message]
        };

        switch (error)
        {
            // ----- FluentValidation errors -----
            case ValidationException ve:
                statusCode = HttpStatusCode.UnprocessableEntity;
                apiError.Message = "One or more validation errors occurred.";
                apiError.Errors = [error.Message];
                break;

            // ----- Unauthorized -----
            case UnauthorizedAccessException _:
                statusCode = HttpStatusCode.Unauthorized;
                apiError.Message = "Unauthorized access.";
                break;

            // ----- Not found -----
            case KeyNotFoundException _:
                statusCode = HttpStatusCode.NotFound;
                apiError.Message = error.Message;
                break;


            // ----- SQL Server errors -----
            case SqlException sqlEx:
                switch (sqlEx.Number)
                {
                    case 2627: // unique constraint
                    case 2601: // unique index
                        statusCode = HttpStatusCode.Conflict;
                        apiError.Message = "Resource already exists.";
                        break;

                    case 547:  // foreign key violation
                        statusCode = HttpStatusCode.BadRequest;
                        apiError.Message = "Cannot delete resource because it’s referenced elsewhere.";
                        break;

                    case 1205: // deadlock
                        statusCode = HttpStatusCode.Conflict;
                        apiError.Message = "Request failed due to a database conflict. Please retry.";
                        break;

                    case 515: // Cannot insert the value NULL into column; column does not allow nulls
                        statusCode = HttpStatusCode.BadRequest;
                        apiError.Message = "Missing required data: a field was left empty.";
                        break;

                    default:
                        statusCode = HttpStatusCode.InternalServerError;
                        apiError.Message = "A database error occurred. Please contact support.";
                        break;
                }
                break;

            // ----- Fallback for all other exceptions -----
            default:
                // keep defaults (500 + generic message)
                break;
        }

        response.StatusCode = (int)statusCode;
        var payload = JsonSerializer.Serialize(apiError);
        return response.WriteAsync(payload);
    }





    //private async static Task HandleExceptionAsync(HttpContext context, Exception error)
    //{
    //    var response = context.Response;
    //    response.ContentType = "application/json";
    //    var responseModel = new ApiResponse<string>() { Succeeded = false, Errors = [] };

    //    //TODO:: cover all validation errors
    //    switch (error)
    //    {
    //        case UnauthorizedAccessException e:
    //            // custom application error
    //            responseModel.Message = error.Message;
    //            responseModel.StatusCode = HttpStatusCode.Unauthorized;
    //            response.StatusCode = (int)HttpStatusCode.Unauthorized;
    //            break;

    //        case ValidationException e:
    //            // custom validation error
    //            responseModel.Errors.Add(error.Message);
    //            responseModel.StatusCode = HttpStatusCode.UnprocessableEntity;
    //            response.StatusCode = (int)HttpStatusCode.UnprocessableEntity;
    //            break;
    //        case KeyNotFoundException e:
    //            // not found error
    //            responseModel.Message = error.Message; ;
    //            responseModel.StatusCode = HttpStatusCode.NotFound;
    //            response.StatusCode = (int)HttpStatusCode.NotFound;
    //            break;

    //        case Exception e:
    //            if (e.GetType().ToString() == "ApiException")
    //            {
    //                responseModel.Message += e.Message;
    //                responseModel.Message += e.InnerException == null ? "" : "\n" + e.InnerException.Message;
    //                responseModel.StatusCode = HttpStatusCode.BadRequest;
    //                response.StatusCode = (int)HttpStatusCode.BadRequest;
    //            }
    //            responseModel.Message = e.Message;
    //            responseModel.Message += e.InnerException == null ? "" : "\n" + e.InnerException.Message;

    //            responseModel.StatusCode = HttpStatusCode.InternalServerError;
    //            response.StatusCode = (int)HttpStatusCode.InternalServerError;
    //            break;

    //        default:
    //            // unhandled error
    //            responseModel.Message = error.Message;
    //            responseModel.StatusCode = HttpStatusCode.InternalServerError;
    //            response.StatusCode = (int)HttpStatusCode.InternalServerError;
    //            break;
    //    }

    //    var result = JsonSerializer.Serialize(responseModel);

    //    await response.WriteAsync(result);
    //}
}














//using FluentValidation;
//using Microsoft.Data.SqlClient;
//using SoccerPro.Application.Common.ResultPattern;
//using System.Net;
//using System.Text.Json;

//namespace SoccerPro.API.Controllers.Middleware;

//public class GlobalExeptionHandlingMiddleware
//{
//    private readonly RequestDelegate _next;
//    private readonly ILogger<GlobalExeptionHandlingMiddleware> _logger;

//    public GlobalExeptionHandlingMiddleware(RequestDelegate next, ILogger<GlobalExeptionHandlingMiddleware> logger)
//    {
//        _next = next;
//        _logger = logger;
//    }

//    public async Task InvokeAsync(HttpContext context)
//    {
//        try
//        {
//            await _next(context);
//        }
//        catch (Exception error)
//        {
//            _logger.LogError(error, error.Message);
//            await HandleExceptionAsync(context, error);
//        }
//    }



//}