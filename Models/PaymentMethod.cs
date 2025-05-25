namespace radio_waves.Models
{
    public class PaymentMethod
    {
        public int Id { get; set; }
        public string Name { get; set; } // e.g., Cash, Card, Insurance
        public string Description { get; set; } // Optional description of the payment method
        // Navigation property to link to payments
        public ICollection<Payment> Payments { get; set; }
        public PaymentMethod()
        {
            Payments = new HashSet<Payment>();
        }
    }
}
