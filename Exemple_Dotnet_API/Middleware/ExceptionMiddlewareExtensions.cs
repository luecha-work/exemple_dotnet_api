using Microsoft.AspNetCore.Diagnostics;
using Shared.ConfigurationModels;
using Shared.Exceptions;

namespace Exemple_Dotnet_API.Middleware
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureExceptionHandler(this WebApplication app)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.ContentType = "application/json";

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        context.Response.StatusCode = GetStatusCode(contextFeature.Error);
                        var errorDetails = GetErrorDetails(
                            contextFeature.Error,
                            context.Response.StatusCode
                        );

                        LogError(context.Request.Path, errorDetails.Message);

                        await context.Response.WriteAsync(errorDetails.ToString());
                    }
                });
            });
        }

        private static int GetStatusCode(Exception exception)
        {
            return exception switch
            {
                NotFoundException => StatusCodes.Status404NotFound,
                BadRequestException => StatusCodes.Status400BadRequest,
                UnauthorizedException => StatusCodes.Status401Unauthorized,
                _ => StatusCodes.Status500InternalServerError
            };
        }

        private static ErrorDetails GetErrorDetails(Exception exception, int statusCode)
        {
            var message = exception.Message;
            // string errorType = GetErrorType(statusCode);

            if (statusCode == StatusCodes.Status500InternalServerError)
            {
                var innerMessage = GetInnerExceptionMessage(exception.InnerException);
                if (!string.IsNullOrEmpty(innerMessage))
                {
                    message = $" {innerMessage}";
                }
            }

            return new ErrorDetails
            {
                Message = message,
                // ErrorType = errorType
            };
        }

        // private static string GetErrorType(int statusCode)
        // {
        //     return statusCode switch
        //     {
        //         StatusCodes.Status404NotFound => "Not Found",
        //         StatusCodes.Status400BadRequest => "Bad Request",
        //         StatusCodes.Status401Unauthorized => "Unauthorized",
        //         StatusCodes.Status500InternalServerError => "Failure",
        //         _ => string.Empty
        //     };
        // }

        private static string GetInnerExceptionMessage(Exception innerException)
        {
            if (innerException == null)
            {
                return string.Empty;
            }

            if (innerException.Message.Contains("23503"))
            {
                return "because it's currently associated with other data. "
                    + "Please make sure to remove or update any related entries before trying again.";
            }

            return innerException.Message;
        }

        private static void LogError(string path, string message)
        {
            Console.WriteLine(
                "Something Went wrong while processing message: {0}, path: {1}",
                message,
                path
            );
            // _logger.LogError($"Something went wrong: {message}");
        }
    }
}
