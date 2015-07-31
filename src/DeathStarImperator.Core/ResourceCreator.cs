using System.Collections.Generic;

namespace DeathStarImperator.Core
{
    public class ResourceCreator
    {
        public List<Resource> InitResources()
        {
            return new List<Resource>
            {
                new Resource("Storm Trooper","stormTrooper", 100, 1000),
                new Resource("Tie Fighter","tieFighter", 1000, 1000),
                new Resource("Tie Fighter Adv.","tieFighterAdv", 5000, 10),
                new Resource("Star Destroyer","starDestroyer", 100000, 1)
            };
        }
    }
}