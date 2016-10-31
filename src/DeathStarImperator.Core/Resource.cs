namespace DeathStarImperator.Core
{
    public class Resource
    {
        public Resource(string name, string tableId, int productionValue, int maxQuantity)
        {
            Name = name;
            TableId = tableId;
            ProductionValue = productionValue;

            Quantity = 0;
            MaxQuantity = maxQuantity;
            PercentAdjustment = 0;            
        }

        public Resource()
        {
        }

        public string Name { get; set; }
        public string TableId { get; set; }
        public double Quantity { get; set; }
        public double MaxQuantity { get; set; }
        public double PercentAdjustment { get; set; }
        public int ProductionValue { get; set; }
        
        public string Progress {
            get { return (MaxQuantity/Quantity) + "%"; }
        }
        public string Rate {
            get { return "(" + (ProductionValue / (GlobalTimer.PRODUCTION_VALUE_PER_SECOND * 60)).ToString("+0.000;-0.000") + "/min)"; }
        }
        public string Bonus {
            get { return "[" + PercentAdjustment.ToString("F2") + "%]"; }
        }

        
    }
}
