using System.Collections.Generic;

namespace DeathStarImperator.Core
{
    public class ResourceCreator
    {
        public List<Resource> InitResources()
        {
            return new List<Resource>
            {
                new Resource("Storm Trooper","stormTrooper", 10),
                new Resource("Tie Fighter","tieFighter", 100),
                new Resource("Tie Fighter Adv.","tieFighterAdv", 500),
                new Resource("Star Destroyer","starDestroyer", 10000)
            };
        }
    }
}