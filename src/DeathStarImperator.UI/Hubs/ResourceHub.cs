﻿using DeathStarImperator.Core;
using Microsoft.AspNet.SignalR;

namespace DeathStarImperator.UI.Hubs
{
    public class ResourceHub : Hub
    {
        public void UpdateResources()
        {
            var resourceList = FakeDataPump.InitResourceList();
            Clients.All.updateResourceInfo(resourceList);
        }
    }
}