using System.ComponentModel;

namespace radio_waves.Models
{
    public class Expenditure
    {
        public int Id { get; set; }
        public string Category { get; set; }
        public decimal Amount { get; set; }
        public bool IsSealed { get; set; }
        [DisplayName("Service")]
        public int? RadiologyTypeId { get; set; }
        public RadiologyType? RadiologyType { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;

    }

}
