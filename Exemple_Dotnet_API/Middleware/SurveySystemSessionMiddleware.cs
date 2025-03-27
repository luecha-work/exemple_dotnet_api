using IService;
using Microsoft.Extensions.Options;
using Shared.ConfigurationModels;
using Shared.Exceptions;
using Shared.Utils;

namespace Exemple_Dotnet_API.Middleware
{
    public class SurveySystemSessionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly JwtConfiguration _jwtConfiguration;

        public SurveySystemSessionMiddleware(
            RequestDelegate next,
            IOptions<JwtConfiguration> configurationJWT
        )
        {
            _next = next;
            _jwtConfiguration = configurationJWT.Value;
        }

        public async Task InvokeAsync(HttpContext context, IUserProvider userProvider)
        {
            try
            {
                if (IsHealthCheckRequest(context))
                {
                    await _next(context);
                    return;
                }

                IServiceManager _service =
                   context.RequestServices.GetRequiredService<IServiceManager>();

                string? ipAddress = context.Connection.RemoteIpAddress?.ToString();
                string? controller = context.GetRouteValue("controller")?.ToString()?.ToUpper();
                string? methodName = context.GetRouteValue("action")?.ToString()?.ToUpper() ?? string.Empty;

                if (IsAuthenticationRequired(controller, methodName))
                {
                    string? authorizationHeader = context.Request.Headers.Authorization;

                    if (
                        !string.IsNullOrEmpty(authorizationHeader)
                        && authorizationHeader.StartsWith("Bearer ")
                    )
                    {
                        string token = context
                            .Request.Headers.Authorization.ToString()
                            .Replace("Bearer ", "");

                        if (string.IsNullOrEmpty(token))
                        {
                            throw new VerificationTokenNotFoundForCheckSessionException();
                        }

                        bool isVerifyJwtToken = JWTHelper.VerifyToken(token, _jwtConfiguration);

                        if (!isVerifyJwtToken)
                        {
                            throw new FailedAuthenticateSessionException();
                        }

                        string sessionId = JWTHelper.GetSessionIdFromToken(token);
                        string accountId = JWTHelper.GetAccoutIdFromToken(token);
                        Guid guidSessionId = Guid.Parse(sessionId);
                        int guidAccountId = int.Parse(accountId);

                        bool checkedSession = await _service.SystemSessionService.CheckSystemSessionStatusAsync(
                            guidSessionId,
                            guidAccountId,
                            ipAddress
                        );

                        if (!checkedSession)
                        {
                            throw new PlanWiseSessionFailUnauthorizedException();
                        }

                        userProvider = await _service.AuthenticationService.GetUserProviderAsync(
                               accountId
                           );

                        // Console.WriteLine("UserProvider: " + userProvider);

                        context.Items["userProvider"] = userProvider;
                    }
                    else
                    {
                        throw new VerificationTokenNotFoundForCheckSessionException();
                    }
                }

                await _next(context);
            }
            catch (Exception ex)
            {
                //_logger.LogError(
                //    $"Something Went wrong while processing {context.Request.Path}, Details: {ex}"
                //);
                Console.WriteLine($"Something Went wrong while processing {context.Request.Path}, Details: {ex}");
                throw;
            }
        }

        private bool IsHealthCheckRequest(HttpContext context)
        {
            return context.Request.Path.Value?.ToUpper() == "/HEALTHCHECK";
        }

        private bool IsAuthenticationRequired(string? controller, string? methodName)
        {
            return controller != "AUTHENTICATION"
                && methodName != "LOGINLOCAL"
                && methodName != "REFRESHTOKEN"
                && methodName != "VERIFYACCESSTOKEN";
        }
    }
}


