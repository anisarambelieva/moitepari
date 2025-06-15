using System;
using Xunit;
using MoitePari.BusinessLogic;
using MoitePari.BusinessLogic.Models;

namespace MoitePari.BusinessLogic.Tests
{
    public class TermValidationTests
    {
        private readonly DepositCalculator _calc = new();

        [Fact]
        public void Calculate_WrongTerm_ThrowsArgumentException()
        {
            var product = new DepositModel
            {
                Id            = 4,
                Name          = "Fixed Term 6m",
                MinAmount     = 0m,
                MaxAmount     = 10_000m,
                InterestRate  = 2.50m,
                TermMonths    = 6,
                FeePercentage = 0m
            };

            Assert.Throws<ArgumentException>(() =>
                _calc.Calculate(product, 1_000m, termMonths: 12));
        }
    }
}
