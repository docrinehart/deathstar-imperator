using System.Collections.Generic;

namespace DeathStarImperator.Core
{
    public interface IHubClient : IResourceHub, IAlertHub
    {
        void OpenConnection();        
    }

    public interface IResourceHub
    {
        void UpdateResourceInfo(List<Resource> resources);
        void UpdateProgressBars(List<ResourceJob> resourceJobs);
    }

    public interface IAlertHub
    {
        void CreateAlert(string message, string messageClass);
    }
}