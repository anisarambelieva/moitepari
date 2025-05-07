namespace MoitePari.Models
{
    public class Deposit
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public decimal MinAmount { get; set; }
        public decimal MaxAmount { get; set; }
        public decimal InterestRate { get; set; }
        public int TermMonths { get; set; }
        public string? Currency { get; set; }
        public string? FeeType { get; set; }
        public string? PaymentFrequency { get; set; }
        public string? Notes { get; set; }
    }
}
