using MoitePari.BusinessLogic.Models;

namespace MoitePari.BusinessLogic
{
    public class DepositCalculator
    {
        /// <summary>
        /// Calculates interest and fee for a given deposit product and input amount/term.
        /// </summary>
        public RepaymentPlan Calculate(
            DepositModel product,
            decimal amount,
            int termMonths)
        {
            // 1) Validate input
            if (amount < product.MinAmount || amount > product.MaxAmount)
                throw new ArgumentOutOfRangeException(
                    nameof(amount),
                    $"Amount must be between {product.MinAmount} and {product.MaxAmount}");

            if (termMonths != product.TermMonths)
                throw new ArgumentException(
                    $"Term must be exactly {product.TermMonths} months",
                    nameof(termMonths));

            // 2) Simple interest prorated for term
            var interest = amount
                * (product.InterestRate / 100m)
                * (termMonths / 12m);

            // 3) Fee as a percentage of principal
            var fee = amount * (product.FeePercentage / 100m);

            // 4) Build plan
            return new RepaymentPlan
            {
                Principal = amount,
                TotalInterest = Math.Round(interest, 2),
                FeeAmount = Math.Round(fee, 2),
            };
        }
    }
}
