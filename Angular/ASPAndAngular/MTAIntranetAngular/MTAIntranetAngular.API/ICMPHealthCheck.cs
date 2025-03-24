using Microsoft.Extensions.Diagnostics.HealthChecks;
using MTAIntranetAngular.Utility;
using System.Net.NetworkInformation;

namespace HealthCheck.API
{
    public class ICMPHealthCheck : IHealthCheck
    {
        private readonly string Host;
        private readonly int HealthyRoundtripTime;

        public ICMPHealthCheck(string host, int healthyRoundtripTime)
        {
            this.Host = host;
            this.HealthyRoundtripTime = healthyRoundtripTime;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
        {
            try
            {
                using var ping = new Ping();
                var reply = await ping.SendPingAsync(Host);
                switch (reply.Status)
                {
                    case IPStatus.Success:
                        var msg =
                            $"PING/ICMP to {Host} took {reply.RoundtripTime} ms.";
                        return (reply.RoundtripTime > HealthyRoundtripTime)
                        ? HealthCheckResult.Degraded(msg)
                        : HealthCheckResult.Healthy(msg);
                    default:
                        var err =
                            $"PING/ICMP to {Host} failed: {reply.Status}";
                        //EmailConfiguration.SendServerFailure(Host);
                        return HealthCheckResult.Unhealthy(err);
                }
            }
            catch (Exception e)
            {
                var err =
                    $"PING/ICMP to {Host} failed {e.Message}";
                //EmailConfiguration.SendServerFailure(Host);
                return HealthCheckResult.Unhealthy(err);
            }
        }
    }
}
