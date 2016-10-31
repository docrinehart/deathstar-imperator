using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;

namespace DeathStarImperator.Core
{
    public class GlobalTimer
    {
        public const int PRODUCTION_VALUE_PER_SECOND = 10;

        private readonly Timer _timer;
        private readonly JobProcessor _jobProcessor;
        private List<Resource> _resourceCache;

        public GlobalTimer(JobProcessor jobProcessor)
        {
            _jobProcessor = jobProcessor;

            _timer = new Timer
            {
                Interval = 100,
                AutoReset = false,
                Enabled = true
            };
            _timer.Elapsed += TickWorldClock;
        }

        private void TickWorldClock(object sender, ElapsedEventArgs e)
        {
            _resourceCache = _jobProcessor.UpdateProgress(_resourceCache);
            _timer.Start();
        }

        public void Begin(List<Resource> resources)
        {
            _resourceCache = resources;
            _jobProcessor.SetJobTypes(resources);

            // Spawn Base/First Job
            var trooper = _resourceCache.Single(r => r.TableId.Equals("stormTrooper"));
            _jobProcessor.AddJob(trooper);

            var fighter = _resourceCache.Single(r => r.TableId.Equals("tieFighter"));
            _jobProcessor.AddJob(fighter);

            _timer.Start();
        }

        public void Pause()
        {
            _timer.Enabled = false;
        }

        public void Resume()
        {
            _timer.Enabled = true;
        }
    }
}