using System.Collections.Generic;

namespace DeathStarImperator.Core
{
    public static class FakeDataPump
    {
        public static IList<Resource> InitResourceList()
        {
            var returnList = new List<Resource>
            {
                new Resource("Storm Trooper","stormTrooper", 5)
                {
                    Quantity = 345,
                    MaxQuantity = 7500,
                    PercentAdjustment = 0.00
                }
            };

            return returnList;
        }  
    }
}
