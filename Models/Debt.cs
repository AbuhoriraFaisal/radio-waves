namespace radio_waves.Models
{
    public class Debt
    {
        public int Id { get; set; }
        public string Debtor { get; set; }
        public decimal Amount { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsPaid { get; set; }
    }

}
