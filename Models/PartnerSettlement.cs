namespace radio_waves.Models
{
    public class PartnerSettlement
    {
        public int Id { get; set; }
        public string PartnerName { get; set; }
        public decimal Amount_Percentage { get; set; }
        public DateTime SettlementDate { get; set; }
    }

}
