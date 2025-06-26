namespace radio_waves.Models
{
    public class InsuranceCompanySettlement
    {
        public int Id { get; set; }
        public string InsuranceCompanyName { get; set; }
        public int ProviderId { get; set; }
        public decimal TotalInsuranceShare { get; set; }
        public decimal NetPayable { get; set; }
        public DateTime SettlementDate { get; set; }
    }

}
