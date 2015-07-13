namespace DeathStarImperator.Core
{
    public class Resource
    {
        public string Name { get; set; }
        public double Quantity { get; set; }
        public double MaxQuantity { get; set; }
        public double ProductionRate { get; set; }
        public string ProductionRateString {
            get { return ProductionRate.ToString("+F3;-F3") + "/min"; }
        }
        public double PercentAdjustment { get; set; }
        public string PercentAdjustmentString {
            get { return PercentAdjustment.ToString("F2") + "%"; }
        }

    }
}
