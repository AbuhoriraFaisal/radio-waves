namespace radio_waves.Models
{
    public class Partner
    {
        public int Id { get; set; }
        public string PartnerName { get; set; }
        public string Comment { get; set; }
        public bool Active { get; set; } = true;

    }
}
