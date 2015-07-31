using System.Collections.Generic;
using System.Configuration;
using System.Reflection;
using DeathStarImperator.Core;
using Microsoft.AspNet.SignalR.Client;

namespace DeathStarImperator.WebJob
{
    public class HubClient : IHubClient
    {
        private readonly string _hubUrl;
        private IHubProxy _alertHubProxy;
        private IHubProxy _resourceHubProxy;
        private HubConnection _connection;

        public HubClient()
        {
            _hubUrl = ConfigurationManager.AppSettings["HubUrl"];
        }

        public void OpenConnection()
        {
            _connection = new HubConnection(_hubUrl);
            _alertHubProxy = _connection.CreateHubProxy("alertHub");
            _resourceHubProxy = _connection.CreateHubProxy("resourceHub");
            _connection.Start().Wait();
        }


        // === Alert Hub Methods === //
        public void CreateAlert(string message, string messageClass)
        {
            _alertHubProxy.Invoke(MethodBase.GetCurrentMethod().Name, message, messageClass);
        }


        // === Resource Hub Methods === //
        public void UpdateResourceInfo(List<Resource> resources)
        {
            var tName = MethodBase.GetCurrentMethod().Name;
            _resourceHubProxy.Invoke(tName, resources);
        }

        public void UpdateProgressBars(List<ResourceJob> resourceJobs)
        {
            _resourceHubProxy.Invoke(MethodBase.GetCurrentMethod().Name, resourceJobs);
        }
    }

}