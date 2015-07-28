namespace DeathStarImperator.Core
{
    public class Resource
    {
        public Resource(string name, string tableId, int productionValue)
        {
            Name = name;
            TableId = tableId;
            ProductionValue = productionValue;

            Quantity = 0;
            MaxQuantity = 0;
            PercentAdjustment = 0;            
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
            get { return "(" + ProductionValue.ToString("+0.000;-0.000") + "/min)"; }
        }
        public string Bonus {
            get { return "[" + PercentAdjustment.ToString("F2") + "%]"; }
        }

        
    }
}
