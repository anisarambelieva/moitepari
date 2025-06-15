using System;
using Xunit;
using MoitePari.BusinessLogic;
using MoitePari.BusinessLogic.Models;

namespace MoitePari.BusinessLogic.Tests
{
    public class ValidCalculationTests
    {
        private readonly DepositCalculator _calc = new();

        [Fact]
        public void Calculate_ValidInput_ComputesCorrectTotals()
        {
            // arrange
            var product = new DepositModel
            {
                Id            = 1,
                Name          = "Test Deposit",
                MinAmount     = 100m,
                MaxAmount     = 10_000m,
                InterestRate  = 5.00m,    // 5% per annum
                TermMonths    = 12,
                FeePercentage = 1.00m     // 1% flat fee
            };
            decimal amount = 2_000m;
            int months    = 12;

            // act
            RepaymentPlan plan = _calc.Calculate(product, amount, months);

            // assert
            Assert.Equal(100.00m, plan.TotalInterest);      // gross interest
            Assert.Equal(20.00m,  plan.FeeAmount);          // fee
            Assert.Equal(2080.00m, plan.NetPayout);         // net payout
        }
    }
}
