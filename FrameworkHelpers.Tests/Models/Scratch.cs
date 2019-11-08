using Shouldly;
using Xunit;

namespace FrameworkHelpers.Tests.Models
{
    public class Scratch
    {

        public Scratch()
        {
            
        }

        [Fact]
        public void Something_to_test()
        {
            int sampleCount = 1;

            sampleCount.ShouldBe(1);

        }

    }
}
