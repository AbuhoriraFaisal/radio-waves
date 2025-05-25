namespace radio_waves.Models
{
    public class Shift
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        // Adding Precentage of Shift
        public double TechnicianPercentage { get; set; }
        public bool IsSpecial { get; set; }
    }

}
