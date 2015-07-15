using Shouldly;

namespace DeathStarImperator.Tests
{
    public class CoreTests
    {
        public void ShouldPass()
        {
            bool thisIsTrue = true;
            thisIsTrue.ShouldBe(true);
        }
    }
}
