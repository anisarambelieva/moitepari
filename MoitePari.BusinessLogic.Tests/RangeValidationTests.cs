using System;
using Xunit;
using MoitePari.BusinessLogic;
using MoitePari.BusinessLogic.Models;

namespace MoitePari.BusinessLogic.Tests
{
    public class RangeValidationTests
    {
        private readonly DepositCalculator _calc = new();

        [Fact]
        public void Calculate_AmountBelowMin_ThrowsArgumentOutOfRange()
        {
            var product = new DepositModel
            {
                Id            = 2,
                Name          = "Small Deposit",
                MinAmount     = 500m,
                MaxAmount     = 5_000m,
                InterestRate  = 3.00m,
                TermMonths    = 6,
                FeePercentage = 0m
            };

            Assert.Throws<ArgumentOutOfRangeException>(() =>
                _calc.Calculate(product, 499.99m, 6));
        }

        [Fact]
        public void Calculate_AmountAboveMax_ThrowsArgumentOutOfRange()
        {
            var product = new DepositModel
            {
                Id            = 3,
                Name          = "Large Deposit",
                MinAmount     = 100m,
                MaxAmount     = 1_000m,
                InterestRate  = 4.00m,
                TermMonths    = 6,
                FeePercentage = 0m
            };

            Assert.Throws<ArgumentOutOfRangeException>(() =>
                _calc.Calculate(product, 1_000.01m, 6));
        }
    }
}
