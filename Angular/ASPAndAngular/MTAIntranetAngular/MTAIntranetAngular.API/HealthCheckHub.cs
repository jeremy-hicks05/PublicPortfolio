using Microsoft.AspNetCore.SignalR;

namespace MTAIntranetAngular.API
{
    public class HealthCheckHub: Hub
    {
        public async Task ClientUpdate(string message) =>
            await Clients.All.SendAsync("ClientUpdate", message);
    }
}
