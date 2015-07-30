using Microsoft.AspNet.SignalR.Hubs;

namespace DeathStarImperator.Core
{
    public interface IResourceHub : IHub
    {
        void UpdateResources();
    }

    public interface IAlertHub : IHub
    {
        void CreateAlert(string message);
    }

}