using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace DeathStarImperator.UI.Hubs
{
    public class ProgressHub : Hub
    {
        public void TrackJob(string jobId)
        {
            Groups.Add(Context.ConnectionId, jobId);
        }

        public void ProgressChanged(string jobId, int progress)
        {
            Clients.Group(jobId).progressChanged(jobId, progress);
        }

        public void JobCompleted(string jobId)
        {
            Clients.Group(jobId).jobCompleted(jobId);
        }
    }
}