using MoitePari.BusinessLogic.Models;

namespace MoitePari.BusinessLogic
{
    /// <summary>
    /// Provides logic for calculating interest, fees, and net payout
    /// for a given deposit product based on input parameters.
    /// </summary>
    public class DepositCalculator
    {
        /// <summary>
        /// Calculates the repayment plan for a deposit, including
        /// interest earned and applicable fee.
        /// </summary>
        /// <param name="product">The deposit product definition with interest rate, fee, and constraints.</param>
        /// <param name="amount">The principal amount the user wants to deposit.</param>
        /// <param name="termMonths">The deposit term in months. Must match the term defined by the product.</param>
        /// <returns>A <see cref="RepaymentPlan"/> object containing the principal, total interest, and fee.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the amount is outside the allowed range.</exception>
        /// <exception cref="ArgumentException">Thrown if the term does not match the product's allowed term.</exception>
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
