using System;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace DeathStarImperator.UI.Hubs
{
    public class ChatHub : Hub
    {
        private static int _viewerCount;

        public void ViewerCountChanged(int viewerCount)
        {
            Clients.All.viewerCountChanged(viewerCount);
        }

        public override Task OnConnected()
        {
            Interlocked.Increment(ref _viewerCount);
            ViewerCountChanged(_viewerCount);

            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            Interlocked.Decrement(ref _viewerCount);
            ViewerCountChanged(_viewerCount);

            return base.OnDisconnected(stopCalled);
        }

        public void Send(string name, string message)
        {
            // Call the addNewMessageToPage method to update clients.
            var centralTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time"));
            Clients.All.addNewMessageToPage(centralTime.ToString("HH:mm:ss.fff"), name, message);
        }
    }
}