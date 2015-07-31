using System;
using System.Configuration;
using System.Threading;
using DeathStarImperator.Core;
using Microsoft.AspNet.SignalR.Client;

namespace DeathStarImperator.WebJob
{
    class Program
    {
        static void Main(string[] args)
        {
            
                Console.WriteLine("Initializing Container...");
                var container = new ContainerInitializer().Initialize();

                Console.WriteLine("Connecting to SignalR Hub");
                var hubClient = container.GetInstance<IHubClient>();
                hubClient.OpenConnection();

                var i = container.GetInstance<Imperator>();
                i.InitializeConfig();
                i.StartImperator();

            while (true)
            {
                // Keeps Imperator from ending
            }

        }
    }

    public class HubClient : IHubClient
    {
        private readonly string _hubUrl;
        private IHubProxy _hubProxy;
        private HubConnection _connection;

        public HubClient()
        {
            _hubUrl = ConfigurationManager.AppSettings["HubUrlProd"];
        }

        public void OpenConnection()
        {
            _connection = new HubConnection(_hubUrl);
            _hubProxy = _connection.CreateHubProxy("alertHub");
            _connection.Start().Wait();
        }

        public void CreateAlert(string msg)
        {
            _hubProxy.Invoke("CreateAlert", msg);
        }
    }
}
