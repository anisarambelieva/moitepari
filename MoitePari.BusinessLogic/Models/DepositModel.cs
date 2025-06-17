namespace MoitePari.BusinessLogic.Models
{
    /// <summary>
    /// Represents a deposit product with defined financial parameters,
    /// such as interest rate, term, and fee structure.
    /// </summary>
    public class DepositModel
    {
        /// <summary>
        /// Gets the unique identifier of the deposit product.
        /// </summary>
        public int Id { get; init; }

        /// <summary>
        /// Gets the name of the deposit product.
        /// </summary>
        public required string Name { get; init; }

        /// <summary>
        /// Gets the minimum deposit amount allowed for the product.
        /// </summary>
        public decimal MinAmount { get; init; }

        /// <summary>
        /// Gets the maximum deposit amount allowed for the product.
        /// </summary>
        public decimal MaxAmount { get; init; }

        /// <summary>
        /// Gets the annual interest rate (as a percentage) offered by the product.
        /// </summary>
        public decimal InterestRate { get; init; }

        /// <summary>
        /// Gets the term of the deposit in months.
        /// </summary>
        public int TermMonths { get; init; }

        /// <summary>
        /// Gets the fee percentage applied to the principal amount.
        /// </summary>
        public decimal FeePercentage { get; init; }
    }
}
