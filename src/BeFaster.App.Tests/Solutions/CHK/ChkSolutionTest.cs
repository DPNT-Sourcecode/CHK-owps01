﻿using BeFaster.App.Solutions.CHK;
using NUnit.Framework;


namespace BeFaster.App.Tests.Solutions.CHK
{
    [TestFixture]
    public class ChkSolutionTest
    {
        [TestCase("AA", ExpectedResult = 100)]
        public int ComputePrice(string skus)
        {
            CheckoutSolution CheckoutSolution = new CheckoutSolution();
            return CheckoutSolution.ComputePrice(skus);
        }

        [TestCase("1", ExpectedResult = -1)]
        public int ComputePriceWithInvalidInput(string skus)
        {
            CheckoutSolution CheckoutSolution = new CheckoutSolution();
            return CheckoutSolution.ComputePrice(skus);
        }

        [TestCase("AAA", ExpectedResult = 130)]
        public int ComputePriceWithMultiple(string skus)
        {
            CheckoutSolution CheckoutSolution = new CheckoutSolution();
            return CheckoutSolution.ComputePrice(skus);
        }

        [TestCase("AAAAAAAAA", ExpectedResult = 380)]
        public int ComputePriceWith2Multiples(string skus)
        {
            CheckoutSolution CheckoutSolution = new CheckoutSolution();
            return CheckoutSolution.ComputePrice(skus);
        }

        [TestCase("EEB", ExpectedResult = 80)]
        public int ComputePriceWithMultipleAndFreebieOfDifferentItem(string skus)
        {
            CheckoutSolution CheckoutSolution = new CheckoutSolution();
            return CheckoutSolution.ComputePrice(skus);
        }

        [TestCase("FFFFF", ExpectedResult = 40)]
        public int ComputePriceWithMultipleAndFreebieOfTheSameItem(string skus)
        {
            CheckoutSolution CheckoutSolution = new CheckoutSolution();
            return CheckoutSolution.ComputePrice(skus);
        }

        [TestCase("AHGTYAS", ExpectedResult = 190)]
        public int ComputePriceWithMultipleAndFreebieOfTheSameItem2(string skus)
        {
            CheckoutSolution CheckoutSolution = new CheckoutSolution();
            return CheckoutSolution.ComputePrice(skus);
        }
    }
}

