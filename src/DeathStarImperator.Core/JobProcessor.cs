using System.Collections.Generic;
using System.Linq;

namespace DeathStarImperator.Core
{
    public class JobProcessor
    {
        private readonly List<ResourceJob> _runningJobs;
        private readonly JobSpawner _jobSpawner;
        private readonly ResourceAdjuster _resourceAdjuster;
        private readonly IHubClient _hubClient;

        private List<Resource> _tempJobList;

        public JobProcessor(JobSpawner jobSpawner, ResourceAdjuster resourceAdjuster, IHubClient hubClient)
        {
            _jobSpawner = jobSpawner;
            _resourceAdjuster = resourceAdjuster;
            _hubClient = hubClient;            

            _runningJobs = _runningJobs = new List<ResourceJob>();
        }

        public List<Resource> UpdateProgress(List<Resource> resourceCache)
        {
            var endingJobs = new List<ResourceJob>();
            foreach (var resJob in _runningJobs)
            {
                // Adjust Progress
                resJob.Progress += 1;

                if (!resJob.HasFinished) { continue; }

                // AdjustResources
                var completedResource = resourceCache.Single(r => r.TableId == resJob.ResourceId);
                var result = _resourceAdjuster.IncrementQuantity(completedResource, 1);
                // TODO: Handle result.Succeeded == false
                var cacheIndex = resourceCache.IndexOf(completedResource);
                resourceCache[cacheIndex] = result.AdjustedResource;

                // AlertComplete
                _hubClient.CreateAlert("Completed Resource: " + resJob.ResourceName, "success");

                // Add to list of ending jobs
                endingJobs.Add(resJob);
            }

            foreach (var completedJob in endingJobs)
            {
                _runningJobs.Remove(completedJob);

                // TODO: Check if Job can respawn; For now, always respawn
                AddJob(_tempJobList.Single(j => j.TableId == completedJob.ResourceId));                
            }

            // ReportProgress
            _hubClient.UpdateProgressBars(_runningJobs);
            _hubClient.UpdateResourceInfo(resourceCache);
            return resourceCache;
        }

        public void AddJob(Resource jobType)
        {
            _hubClient.CreateAlert(jobType.TableId + " Job Added", "default");
            _runningJobs.Add(_jobSpawner.SpawnJob(jobType));
        }

        public void SetJobTypes(List<Resource> resources)
        {
            _tempJobList = resources;
        }
    }
}