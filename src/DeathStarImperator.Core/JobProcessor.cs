using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Infrastructure;

namespace DeathStarImperator.Core
{
    public class JobProcessor
    {
        private readonly List<ResourceJob> _runningJobs;
        private readonly JobSpawner _jobSpawner;
        private readonly IHubContext _alertHub;

        private List<Resource> _tempJobList;

        public JobProcessor(JobSpawner jobSpawner, IConnectionManager connectionManager)
        {
            _jobSpawner = jobSpawner;
            _runningJobs = _runningJobs = new List<ResourceJob>();
            _alertHub = connectionManager.GetHubContext<IAlertHub>();
        }

        public void UpdateProgress()
        {
            foreach (var resJob in _runningJobs)
            {
                // Adjust Progress
                resJob.Progress += 1;

                if (resJob.HasFinished)
                {
                    // AdjustResources

                    // AlertComplete
                    _alertHub.Clients.All.CreateAlert("Completed Resource: " + resJob.ResourceName);

                    //resJob.Finish();
                    _runningJobs.Remove(resJob);
                    AddJob(_tempJobList.Single(j => j.TableId == "stormTrooper"));
                }
                    
                // ReportProgress
                _alertHub.Clients.All.CreateAlert(resJob.Progress + "/" + resJob.TargetValue + " Progress from Resource: " + resJob.ResourceName);
            }
            
        }

        public void AddJob(Resource jobType)
        {
            _runningJobs.Add(_jobSpawner.SpawnJob(jobType));
        }

        public void SetJobTypes(List<Resource> resources)
        {
            _tempJobList = resources;
        }
    }
}