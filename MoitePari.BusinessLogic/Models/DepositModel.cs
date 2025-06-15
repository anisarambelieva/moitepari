namespace MoitePari.BusinessLogic.Models
{
    public class DepositModel
    {
        public int Id { get; init; }
        public required string Name { get; init; }
        public decimal MinAmount { get; init; }
        public decimal MaxAmount { get; init; }
        public decimal InterestRate { get; init; }
        public int TermMonths { get; init; }
        public decimal FeePercentage { get; init; }
    }
}
