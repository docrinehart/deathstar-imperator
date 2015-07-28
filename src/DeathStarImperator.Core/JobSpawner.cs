namespace DeathStarImperator.Core
{
    public class JobSpawner
    {
        public ResourceJob SpawnJob(Resource jobType)
        {
            return new ResourceJob
            {
                ResourceName = jobType.Name,
                TargetValue = jobType.ProductionValue,
                Progress = 0
            };
        }
    }
}