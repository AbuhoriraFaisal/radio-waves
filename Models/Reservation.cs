namespace radio_waves.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public string PatientName { get; set; }
        public DateTime AppointmentDate { get; set; }

        public int RadiologyTypeId { get; set; }
        public RadiologyType RadiologyType { get; set; }
    }

}
