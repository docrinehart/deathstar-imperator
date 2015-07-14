namespace DeathStarImperator.Core
{
    public class Resource
    {
        public string Name { get; set; }
        public string TableId { get; set; }
        public double Quantity { get; set; }
        public double MaxQuantity { get; set; }
        public double PercentAdjustment { get; set; }
        public double ProductionRate { get; set; }
        
        public string Progress {
            get { return (MaxQuantity/Quantity) + "%"; }
        }
        public string Rate {
            get { return "(" + ProductionRate.ToString("+0.000;-0.000") + "/min)"; }
        }
        public string Bonus {
            get { return "[" + PercentAdjustment.ToString("F2") + "%]"; }
        }
    }
}
