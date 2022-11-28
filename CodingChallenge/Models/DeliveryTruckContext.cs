using Microsoft.EntityFrameworkCore;

namespace CodingChallenge.Models
{
    public class DeliveryTruckContext : DbContext
    {
        public DeliveryTruckContext(DbContextOptions<DeliveryTruckContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<DeliveryTruck> DeliveryTrucks { get; set; }
    }
}
