namespace radio_waves.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public int ReservationId { get; set; }
        public Reservation Reservation { get; set; }

        public DateTime PaymentDate { get; set; }
        public decimal Amount { get; set; }
        public PaymentMethod PaymentMethod { get; set; } // e.g., Cash, Card, Insurance
        public int PaymentMethodId { get; set; } // Foreign key to PaymentMethod
        public bool IsCoveredByInsurance { get; set; }
    }
}
