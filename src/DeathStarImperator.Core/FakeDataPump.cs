using System.Collections.Generic;

namespace DeathStarImperator.Core
{
    public static class FakeDataPump
    {
        public static IList<Resource> InitResourceList()
        {
            var returnList = new List<Resource>
            {
                new Resource
                {
                    Name = "Storm Trooper",
                    TableId = "stormTrooper",
                    Quantity = 345,
                    MaxQuantity = 7500,
                    PercentAdjustment = 0.00,
                    ProductionRate = 5.00
                },
                new Resource
                {
                    Name = "Tie Fighter",
                    TableId = "tieFighter",
                    Quantity = 27,
                    MaxQuantity = 1000,
                    PercentAdjustment = 5.00,
                    ProductionRate = 2.50
                },
                new Resource
                {
                    Name = "Tie Fighter Adv.",
                    TableId = "tieFighterAdv",
                    Quantity = 14,
                    MaxQuantity = 250,
                    PercentAdjustment = -25.0,
                    ProductionRate = 1.00
                },
                new Resource
                {
                    Name = "Star Destroyer",
                    TableId = "starDestroyer",
                    Quantity = 2,
                    MaxQuantity = 50,
                    PercentAdjustment = 0.00,
                    ProductionRate = 0.05
                }
            };

            return returnList;
        }  
    }
}
