namespace radio_waves.Models
{
    public class Shift
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int StartTime { get; set; }
        public int EndTime { get; set; }
        // Adding Precentage of Shift
        public double TechnicianPercentage { get; set; }
    }

}
