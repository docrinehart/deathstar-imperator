using System;

namespace DeathStarImperator.Core
{
    public class JobProcessor
    {
        public void UpdateProgress(ResourceJob job)
        {
            // ReportProgress

            if (job.HasFinished)
            {
                // AdjustResources
                // AlertComplete
                job.Finish();
            }
            else
            {
                // ReportProgress
            }

            throw new NotImplementedException();
        }
    }
}