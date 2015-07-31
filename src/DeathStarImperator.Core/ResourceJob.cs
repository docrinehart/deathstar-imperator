namespace DeathStarImperator.Core
{
    public class ResourceJob
    {
        public string ResourceName { get; set; }
        public string ResourceId { get; set; }
        public int Progress { get; set; }
        public int TargetValue { get; set; }

        public string ProgressWidth
        {
            get
            {
                return string.Format("{0}%", ((double)Progress / TargetValue) * 100.0);
            }
        }

        public bool HasFinished { get { return Progress >= TargetValue; } }

    }
}