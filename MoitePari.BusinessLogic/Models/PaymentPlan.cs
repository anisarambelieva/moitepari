namespace MoitePari.BusinessLogic.Models
{
    /// <summary>
    /// Represents the calculated result of a deposit,
    /// including interest, fees, and final payout.
    /// </summary>
    public class RepaymentPlan
    {
        /// <summary>
        /// Gets the original amount deposited (principal).
        /// </summary>
        public decimal Principal { get; init; }

        /// <summary>
        /// Gets the total interest earned for the given term.
        /// </summary>
        public decimal TotalInterest { get; init; }

        /// <summary>
        /// Gets the total fee applied to the deposit.
        /// </summary>
        public decimal FeeAmount { get; init; }

        /// <summary>
        /// Gets the final net payout after deducting the fee from the total.
        /// This is calculated as Principal + Interest - Fee.
        /// </summary>
        public decimal NetPayout => Principal + TotalInterest - FeeAmount;
    }
}
