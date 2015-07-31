using System.Collections.Generic;
using DeathStarImperator.Core;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace DeathStarImperator.UI.Hubs
{
    [HubName("resourceHub")]
    public class ResourceHub : Hub, IResourceHub
    {
        public void UpdateResourceInfo(List<Resource> resources)
        {
            Clients.All.updateResourceInfo(resources);
        }

        public void UpdateProgressBars(List<ResourceJob> resourceJobs)
        {
            Clients.All.updateResourceProgress(resourceJobs);
        }
    }
}