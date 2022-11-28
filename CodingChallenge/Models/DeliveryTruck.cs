namespace CodingChallenge.Models
{
    public class DeliveryTruck
    {
        public long Id { get; set; }
        public string? DeliveryFor { get; set; }
        public long? Distance { get; set; }
        public DateTime? TimeStamp { get; set; }
    }
}
