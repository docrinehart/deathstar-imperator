using System.Collections.Generic;

namespace DeathStarImperator.Core
{
    public class Imperator
    {
        private List<Resource> _allResources;
        private readonly ResourceCreator _resourceCreator;
        private readonly GlobalTimer _worldTimer;

        public Imperator(ResourceCreator resourceCreator, GlobalTimer worldTimer)
        {
            _resourceCreator = resourceCreator;
            _worldTimer = worldTimer;
        }

        public void InitializeConfig()
        {
            _allResources = _resourceCreator.InitResources();
        }

        public void StartImperator()
        {
            _worldTimer.Begin(_allResources);
        }

    }
}
