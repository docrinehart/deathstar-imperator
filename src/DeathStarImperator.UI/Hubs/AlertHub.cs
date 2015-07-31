using System;
using DeathStarImperator.Core;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace DeathStarImperator.UI.Hubs
{
    [HubName("alertHub")]
    public class AlertHub : Hub, IAlertHub
    {
        public void CreateAlert(string message, string alertClass)
        {
            var centralTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time"));
            Clients.All.addNewAlertToPage(centralTime.ToString("HH:mm:ss.fff"), message, alertClass + "-alert");
        }
    }
}