using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeathStarImperator.Core
{
    public static class FakeDataPump
    {
        public static IList<Resource> InitResourceList()
        {
            var returnList = new List<Resource>();

            returnList.Add(new Resource()
            {
                Name = "Storm Trooper",
                Quantity = 345,
                MaxQuantity = 7500,
                PercentAdjustment = 0.00,
                ProductionRate = 5.00                
            });
            returnList.Add(new Resource()
            {
                Name = "Tie Fighter",
                Quantity = 27,
                MaxQuantity = 1000,
                PercentAdjustment = 5.00,
                ProductionRate = 2.50
            });
            returnList.Add(new Resource()
            {
                Name = "Tie Fighter Adv.",
                Quantity = 14,
                MaxQuantity = 250,
                PercentAdjustment = -25.0,
                ProductionRate = 1.00
            });
            returnList.Add(new Resource()
            {
                Name = "Star Destroyer",
                Quantity = 2,
                MaxQuantity = 50,
                PercentAdjustment = 0.00,
                ProductionRate = 0.05
            });

            return returnList;
        }  
    }
}
