using BeFaster.App.Solutions.CHK;
using NUnit.Framework;


namespace BeFaster.App.Tests.Solutions.CHK
{
    [TestFixture]
    public class ChkSolutionTest
    {
        [TestCase("AA", ExpectedResult = 100)]
        public int ComputePrice(string skus)
        {
            return CheckoutSolution.ComputePrice(skus);
        }

        [TestCase("1", ExpectedResult = -1)]
        public int ComputePriceWithInvalidInput(string skus)
        {
            return CheckoutSolution.ComputePrice(skus);
        }

        [TestCase("AAA", ExpectedResult = 130)]
        public int ComputePriceWithMultiple(string skus)
        {
            return CheckoutSolution.ComputePrice(skus);
        }
    }
}
