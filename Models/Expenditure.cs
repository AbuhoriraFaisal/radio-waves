namespace radio_waves.Models
{
    public class Expenditure
    {
        public int Id { get; set; }
        public string Category { get; set; }
        public decimal Amount { get; set; }
        public bool IsSealed { get; set; }
        public DateTime Date = DateTime.Now;
    }

}
