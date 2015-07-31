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
        private readonly IHubClient _alertHubProxy;

        private List<Resource> _tempJobList;

        public JobProcessor(JobSpawner jobSpawner, IHubClient hubProxyClient)
        {
            _jobSpawner = jobSpawner;
            _runningJobs = _runningJobs = new List<ResourceJob>();
            _alertHubProxy = hubProxyClient;
        }

        public void UpdateProgress()
        {
            var endingJobs = new List<ResourceJob>();
            foreach (var resJob in _runningJobs)
            {
                // Adjust Progress
                resJob.Progress += 1;

                if (resJob.HasFinished)
                {
                    // AdjustResources

                    // AlertComplete
                    _alertHubProxy.CreateAlert("Completed Resource: " + resJob.ResourceName);
                    //_alertHubProxy.Clients.All.CreateAlert("Completed Resource: " + resJob.ResourceName);

                    //resJob.Finish();
                    endingJobs.Add(resJob);
                }
                    
                // ReportProgress
                var msg = resJob.Progress + "/" + resJob.TargetValue + " Progress from Resource: " + resJob.ResourceName;
                _alertHubProxy.CreateAlert(msg);
                //_alertHubProxy.Clients.All.CreateAlert(msg);
            }

            foreach (var completedJob in endingJobs)
            {
                _runningJobs.Remove(completedJob);
                AddJob(_tempJobList.Single(j => j.TableId == "stormTrooper"));
            }
            
        }

        public void AddJob(Resource jobType)
        {
            _alertHubProxy.CreateAlert(jobType.TableId + " Job Added");
            _runningJobs.Add(_jobSpawner.SpawnJob(jobType));
        }

        public void SetJobTypes(List<Resource> resources)
        {
            _tempJobList = resources;
        }
    }

    public interface IHubClient
    {
        void OpenConnection();
        void CreateAlert(string msg);
    }

    
}