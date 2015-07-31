using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using DeathStarImperator.UI.Hubs;
using Microsoft.AspNet.SignalR;

namespace DeathStarImperator.UI.Models
{
    public interface IProgressMonitor
    {
        void BroadcastJobStatus(ProgressJob progressJob);
    }

    public class ProgressMonitor : IProgressMonitor
    {
        private readonly IHubContext _hubContext;

        public ProgressMonitor()
        {
            _hubContext = GlobalHost.ConnectionManager.GetHubContext<ProgressHub>();
        }

        public void BroadcastJobStatus(ProgressJob progressJob)
        {
            progressJob.ProgressChanged += HandleJobProgressChanged;
            progressJob.Completed += HandleJobCompleted;
        }

        private void HandleJobCompleted(object sender, EventArgs e)
        {
            var job = (ProgressJob)sender;

            _hubContext.Clients.Group(job.Id).jobCompleted(job.Id);

            job.ProgressChanged -= HandleJobProgressChanged;
            job.Completed -= HandleJobCompleted;
        }

        private void HandleJobProgressChanged(object sender, EventArgs e)
        {
            var job = (ProgressJob)sender;
            _hubContext.Clients.Group(job.Id).progressChanged(job.Id, job.Progress);
        }

    }
}