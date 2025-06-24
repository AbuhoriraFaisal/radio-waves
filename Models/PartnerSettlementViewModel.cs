namespace radio_waves.Models
{
    public class PartnerSettlementViewModel
    {
        public int Id { get; set; }
        public string PartnerName { get; set; }
        public decimal Amount { get; set; }
        public DateTime SettlementDate { get; set; }
    }
}
