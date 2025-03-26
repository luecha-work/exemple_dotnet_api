using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Exemple_Dotnet_API.HealthCheck
{
    public class ServiceHealthCheck : IHealthCheck
    {
        public Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = default
        )
        {
            try
            {
                return Task.FromResult(HealthCheckResult.Healthy("The service is up and running."));
            }
            catch (Exception)
            {
                return Task.FromResult(
                    new HealthCheckResult(
                        context.Registration.FailureStatus,
                        "The service is down."
                    )
                );
            }
        }
    }
}
