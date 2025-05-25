namespace radio_waves.Models
{
    public class Debt
    {
        public int Id { get; set; }
        public int? PatientId { get; set; }
        public Patient Patient { get; set; }

        public int? ReservationId { get; set; }
        public Reservation Reservation { get; set; }
        public string Comments { get; set; }
        public decimal Amount { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsPaid { get; set; }
    }

}
