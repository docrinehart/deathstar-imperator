using System;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace DeathStarImperator.UI.Hubs
{
    public class ChatHub : Hub
    {
        public void Send(string name, string message)
        {
            // Call the addNewMessageToPage method to update clients.
            var centralTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time"));
            Clients.All.addNewMessageToPage(centralTime.ToString("HH:mm:ss.fff"), name, message);
        }
    }
}