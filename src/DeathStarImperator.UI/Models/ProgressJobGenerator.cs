using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace DeathStarImperator.UI.Models
{
    public interface IProgressJobGenerator
    {
        ProgressJob Execute(Action<ProgressJob> action);
    }

    public class ProgressJobGenerator : IProgressJobGenerator
    {
        private readonly ConcurrentDictionary<string, ProgressJob> _runningJobs = new ConcurrentDictionary<string, ProgressJob>();
        private readonly IProgressMonitor _monitor;

        public ProgressJobGenerator(IProgressMonitor monitor)
        {
            _monitor = monitor;
        }

        public ProgressJob Execute(Action<ProgressJob> action)
        {
            var job = new ProgressJob(Guid.NewGuid().ToString());

            // this will (should!) never fail, because job.Id is globally unique
            _runningJobs.TryAdd(job.Id, job);

            Task.Factory.StartNew(() =>
            {
                action(job);
                job.ReportComplete();
                _runningJobs.TryRemove(job.Id, out job);
            },
            TaskCreationOptions.LongRunning);

            _monitor.BroadcastJobStatus(job);

            return job;
        }
    }
}