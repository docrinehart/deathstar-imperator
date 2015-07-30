using DeathStarImperator.Core;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace DeathStarImperator.UI.Hubs
{
    [HubName("resourceHub")]
    public class ResourceHub : Hub, IResourceHub
    {
        public void UpdateResources()
        {
            var resourceList = FakeDataPump.InitResourceList();
            Clients.All.updateResourceInfo(resourceList);
        }
    }
}