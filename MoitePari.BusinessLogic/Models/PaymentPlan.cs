namespace MoitePari.BusinessLogic.Models
{
    public class RepaymentPlan
    {
        public decimal Principal { get; init; }
        public decimal TotalInterest { get; init; }
        public decimal FeeAmount { get; init; }
        public decimal NetPayout => Principal + TotalInterest - FeeAmount;
    }
}
