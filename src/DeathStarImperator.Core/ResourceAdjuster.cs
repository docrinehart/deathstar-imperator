namespace DeathStarImperator.Core
{
    public class ResourceAdjusterResult
    {
        public ResourceAdjusterResult()
        {
            Succeeded = false;
            Message = "";
        }

        public bool Succeeded { get; set; }
        public string Message { get; set; }
        public Resource AdjustedResource { get; set; }
    }

    public class ResourceAdjuster
    {
        public ResourceAdjusterResult IncrementQuantity(Resource resource, int amount)
        {
            var result = new ResourceAdjusterResult();

            if (resource.Quantity < resource.MaxQuantity)
            {
                resource.Quantity += amount;
                result.Succeeded = true;
            }
            else
            {
                result.Message = "Max quantity reached.";
            }

            result.AdjustedResource = resource;
            return result;
        }

        public ResourceAdjusterResult DecrementQuantity(Resource resource, int amount)
        {
            var result = new ResourceAdjusterResult();

            if (resource.Quantity > 0)
            {
                resource.Quantity -= amount;
                result.Succeeded = true;
            }
            else
            {
                result.Message = "Not enough resources.";
            }

            result.AdjustedResource = resource;
            return result;
        }
    }
}