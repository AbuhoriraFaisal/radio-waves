namespace radio_waves.Models
{
    public class PartnerServices
    {
        public int Id { get; set; }
        public int PartnerId { get; set; }
        
        public Partner? Partner { get; set; }
        public int ServiceId { get; set; }
        public RadiologyType? Service { get; set; }

        public decimal Amount_Percentage { get; set; }

    }
}
