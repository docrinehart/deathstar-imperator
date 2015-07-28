using System;

namespace DeathStarImperator.Core
{
    public class ResourceJob
    {
        public string ResourceName { get; set; }
        public int Progress { get; set; }
        public int TargetValue { get; set; }
        public bool HasFinished { get { return Progress >= TargetValue; } }

        public void Finish()
        {
            // if CanRespawn ? : Respawn/Despawn
            throw new NotImplementedException();
        }
    }
}