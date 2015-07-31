using System.Collections.Generic;
using System.Linq;
using System.Timers;

namespace DeathStarImperator.Core
{
    public class GlobalTimer
    {
        private readonly Timer _timer;
        
        private readonly JobProcessor _jobProcessor;
        
        private List<Resource> _resourceCache;

        public GlobalTimer(JobProcessor jobProcessor)
        {
            _jobProcessor = jobProcessor;

            _timer = new Timer
            {
                Interval = 1000,
                AutoReset = false,
                Enabled = true
            };
            _timer.Elapsed += TickWorldClock;
        }

        private void TickWorldClock(object sender, ElapsedEventArgs e)
        {
            _jobProcessor.UpdateProgress();
            _timer.Start();
        }

        public void Begin(List<Resource> resources)
        {
            _resourceCache = resources;
            _jobProcessor.SetJobTypes(resources);

            // Spawn Base/First Job
            var trooper = _resourceCache.Single(r => r.TableId.Equals("stormTrooper"));
            _jobProcessor.AddJob(trooper);

            _timer.Start();
        }
    }
}