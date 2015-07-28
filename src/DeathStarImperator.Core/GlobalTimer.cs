using System.Collections.Generic;
using System.Linq;
using System.Timers;

namespace DeathStarImperator.Core
{
    public class GlobalTimer
    {
        private readonly Timer _timer;
        private readonly JobSpawner _jobSpawner;
        private readonly JobProcessor _jobProcessor;
        private readonly List<ResourceJob> _runningJobs;
        private List<Resource> _resourceCache;

        public GlobalTimer(JobSpawner jobSpawner, JobProcessor jobProcessor)
        {
            _jobSpawner = jobSpawner;
            _jobProcessor = jobProcessor;
            _runningJobs = new List<ResourceJob>();

            _timer = new Timer
            {
                Interval = 1000,
                AutoReset = true
            };
            _timer.Elapsed += TickWorldClock;
        }

        private void TickWorldClock(object sender, ElapsedEventArgs e)
        {
            foreach (var resJob in _runningJobs)
            {
                _jobProcessor.UpdateProgress(resJob);
            }
        }

        public void Begin(List<Resource> resources)
        {
            _resourceCache = resources;
            
            // Spawn Base/First Job
            var trooper = _resourceCache.Single(r => r.TableId.Equals("stormTrooper"));
            _runningJobs.Add(_jobSpawner.SpawnJob(trooper));

            _timer.Start();
        }
    }
}