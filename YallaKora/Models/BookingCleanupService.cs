namespace YallaKora.Models
{
    public class BookingCleanupService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public BookingCleanupService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _serviceProvider.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<MyContext>();

                var expiredBookings = db.Bookings
                    .Where(b => b.EndDate < DateTime.Now)
                    .ToList();

                foreach (var booking in expiredBookings)
                {
                    var slot = db.Slots.FirstOrDefault(s => s.BookingId == booking.BookingId);
                    if (slot != null)
                    {
                        db.SlotPlayers.RemoveRange(db.SlotPlayers.Where(sp => sp.SlotId == slot.SlotId));
                        db.Slots.Remove(slot);
                    }
                    db.Bookings.Remove(booking);
                }

                db.SaveChanges();
                await Task.Delay(TimeSpan.FromMinutes(30), stoppingToken);
            }
        }
    }
}
