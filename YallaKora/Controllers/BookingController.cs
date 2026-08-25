using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using YallaKora.Models;

namespace YallaKora.Controllers
{
    public class BookingController : Controller
    {
        MyContext db = new MyContext();
        public IActionResult BookingType()
        {
            return View();
        }


        #region Individual IActionResults

        public IActionResult Individual()
        {
            var Bookings = db.Bookings.Where(b => b.Type == "Individual").ToList();
            var Slots = db.Slots.ToList();
            var SlotPlayers = db.SlotPlayers.ToList();
            ViewData["Slots"] = Slots;
            ViewData["SlotPlayers"] = SlotPlayers;
            return View(Bookings);


        }
        public IActionResult IndividualView(int id) 
        {
           
            var slot = db.Slots.FirstOrDefault(s => s.SlotId == id);
            if (slot == null) return NotFound();

            
            var slotPlayers = db.SlotPlayers
                                .Where(sp => sp.SlotId == id)
                                .Include(sp => sp.User)
                                .ToList();

            ViewData["CurrentSlot"] = slot;
            return View(slotPlayers);
        }

        [HttpPost]
        public IActionResult JoinSlot(int slotId, string position)
        {
            
            int? currentUserId = HttpContext.Session.GetInt32("UserId");
            if (currentUserId == null)
            {
                return RedirectToAction("Login", "Account");
            }

          
            bool isPositionTaken = db.SlotPlayers.Any(sp => sp.SlotId == slotId && sp.Position == position);
            if (isPositionTaken)
            {
                TempData["Error"] = "This position is already taken!";
                return RedirectToAction("IndividualView", new { id = slotId });
            }

            bool hasAlreadyJoined = db.SlotPlayers.Any(sp => sp.SlotId == slotId && sp.UserId == currentUserId.Value);
            if (hasAlreadyJoined)
            {
                TempData["Error"] = "You have already joined this match!";
                return RedirectToAction("IndividualView", new { id = slotId });
            }

            
            var newSlotPlayer = new SlotPlayer
            {
                SlotId = slotId,
                UserId = currentUserId.Value,
                Position = position,
                JoinDate = DateTime.Now
            };

            db.SlotPlayers.Add(newSlotPlayer);

          
            var slot = db.Slots.FirstOrDefault(s => s.SlotId == slotId);
            if (slot != null)
            {
                slot.CurrentPlayers = db.SlotPlayers.Count(sp => sp.SlotId == slotId) + 1;

               
                if (slot.CurrentPlayers >= 10)
                {
                    slot.Status = "Full";
                }
            }
            Console.WriteLine($"SlotId received: {slotId}");

            db.SaveChanges();
            return RedirectToAction("IndividualView", new { id = slotId });
        }

        [HttpPost]
        public IActionResult LeaveSlot(int slotPlayerId, int slotId)
        {
           
            int? currentUserId = HttpContext.Session.GetInt32("UserId");
            if (currentUserId == null)
                return RedirectToAction("Login", "Account");

           
            var slotPlayer = db.SlotPlayers.FirstOrDefault(sp =>
                sp.SlotPlayerId == slotPlayerId &&
                sp.UserId == currentUserId.Value);

            if (slotPlayer != null)
            {
               
                db.SlotPlayers.Remove(slotPlayer);

                
                var slot = db.Slots.FirstOrDefault(s => s.SlotId == slotId);

                if (slot != null)
                {
                  
                    int remainingPlayersCount = db.SlotPlayers.Count(sp => sp.SlotId == slotId && sp.SlotPlayerId != slotPlayerId);

                    slot.CurrentPlayers = Math.Max(0, remainingPlayersCount);
                    slot.Status = "Open";

                    if (slot.CurrentPlayers == 0)
                    {
                   
                        var thisBooking = db.Bookings.FirstOrDefault(b => b.BookingId == slot.BookingId);

                        if (thisBooking != null)
                        {
                            db.Bookings.Remove(thisBooking);
                        }

                        db.Slots.Remove(slot);
                    }
                }

               
                db.SaveChanges();
            }

        
            return RedirectToAction("Individual", "Booking"); 
        }



        [HttpGet]
        public IActionResult AvailableSlots()
        {
            var today = DateTime.Now.Date;
            var threeDaysLater = today.AddDays(3);

           
            var slots = db.Slots
                          .Include(s => s.Booking) 
                          .Where(s => s.Booking != null && s.Booking.Date >= today && s.Booking.Date < threeDaysLater)
                          .ToList();

            
            var slotPlayers = db.SlotPlayers.ToList();

            ViewBag.SlotPlayers = slotPlayers;

          
            return View(slots);
        }

        [HttpPost]
        public IActionResult BookSlot(int slotId)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

           
            var existingBooking = db.SlotPlayers.FirstOrDefault(sp => sp.SlotId == slotId && sp.UserId == userId.Value);
            if (existingBooking == null)
            {
                var slotPlayer = new SlotPlayer
                {
                    SlotId = slotId,
                    UserId = userId.Value
                   
                };

                db.SlotPlayers.Add(slotPlayer);
                db.SaveChanges();
            }

            return RedirectToAction(nameof(AvailableSlots));
        }

        public IActionResult CreateIndividual()
        {
            var courts = db.Courts.ToList();
            var bookings = db.Bookings.ToList();
            ViewData["Bookings"] = bookings;
            return View(courts);
        }

        [HttpPost]
        public IActionResult CreateIndividual(int courtId, string day, int hour)
        {
            int? currentUserId = HttpContext.Session.GetInt32("UserId");
            if (currentUserId == null)
                return RedirectToAction("Login", "Account");

            var today = DateTime.Now;
            int daysUntil = ((int)Enum.Parse<DayOfWeek>(day) - (int)today.DayOfWeek + 7) % 7;
            DateTime bookingDate = today.AddDays(daysUntil);

       
            bool alreadyBooked = db.Bookings
                .Where(b => b.CourtId == courtId &&
                            b.StartDate.Hour == hour )
                .AsEnumerable()
                .Any(b => b.Date.DayOfWeek.ToString() == day);

            if (alreadyBooked)
                return RedirectToAction("CreateIndividual");

           
            var newBooking = new Booking
            {
                Type = "Individual",
                CourtId = courtId,
                UserId = currentUserId.Value,
                Date = bookingDate,
                StartDate = bookingDate.Date.AddHours(hour),
                EndDate = bookingDate.Date.AddHours(hour + 1),
                Status = "Open",
                TotalPrice = db.Courts.FirstOrDefault(c => c.CourtId == courtId)?.PricePerHour ?? 0
            };

            db.Bookings.Add(newBooking);
            db.SaveChanges();

      
            var slot = new Slot
            {
                BookingId = newBooking.BookingId,
                CurrentPlayers = 0,
                Status = "Open"
            };

            db.Slots.Add(slot);
            db.SaveChanges();

            return RedirectToAction("IndividualView", new { id = slot.SlotId });
        }
        public IActionResult JoinIndividual(int courtId, string day, int hour)
        {
            int? currentUserId = HttpContext.Session.GetInt32("UserId");
            if (currentUserId == null)
                return RedirectToAction("Login", "Account");

            var booking = db.Bookings
                .Where(b => b.CourtId == courtId &&
                            b.Type == "Individual" &&
                            b.StartDate.Hour == hour)
                .AsEnumerable()
                .FirstOrDefault(b => b.Date.DayOfWeek.ToString() == day);

            if (booking == null)
                return RedirectToAction("CreateIndividual");


            var slot = db.Slots.FirstOrDefault(s => s.BookingId == booking.BookingId);

            if (slot == null)
                return RedirectToAction("CreateIndividual");

     
            return RedirectToAction("IndividualView", new { id = slot.SlotId });
        }
        [HttpPost]
        [HttpPost]
        public IActionResult CancelIndividual(int bookingId)
        {
            int? currentUserId = HttpContext.Session.GetInt32("UserId");
            if (currentUserId == null)
                return RedirectToAction("Login", "Account");

            var booking = db.Bookings.FirstOrDefault(b =>
                b.BookingId == bookingId &&
                b.UserId == currentUserId.Value);

            if (booking != null)
            {
                var slot = db.Slots.FirstOrDefault(s => s.BookingId == bookingId);
                if (slot != null)
                {
             
                    var slotPlayers = db.SlotPlayers.Where(sp => sp.SlotId == slot.SlotId).ToList();
                    if (slotPlayers.Count == 0)
                    {
                        db.Slots.Remove(slot);
                        db.Bookings.Remove(booking);
                    }
                    else
                    {
               
                        TempData["Error"] = "Cannot cancel, players have already joined!";
                        return RedirectToAction("CreateIndividual");
                    }
                }
                db.SaveChanges();
            }

            return RedirectToAction("CreateIndividual");
        }


        #endregion



        #region WithFriends IActionResults


        [HttpGet]
        public IActionResult WithFriends()
        {
            var courts = db.Courts.ToList();
            var bookings = db.Bookings.ToList();
            ViewData["Bookings"] = bookings;
            return View(courts);
        }
        [HttpPost]
        public IActionResult WithFriends(int courtId, string day, int hour)
        {
            int? currentUserId = HttpContext.Session.GetInt32("UserId");
            if (currentUserId == null)
                return RedirectToAction("Login", "Account");

            var today = DateTime.Now;
            int daysUntil = ((int)Enum.Parse<DayOfWeek>(day) - (int)today.DayOfWeek + 7) % 7;
            DateTime bookingDate = today.AddDays(daysUntil);

            // ← تحقق إن مفيش Booking موجود
            bool alreadyBooked = db.Bookings
            .Where(b => b.CourtId == courtId && b.StartDate.Hour == hour)
            .AsEnumerable()
            .Any(b => b.Date.DayOfWeek.ToString() == day);

            if (alreadyBooked)
                return RedirectToAction("WithFriends");

            var newBooking = new Booking
            {
                Type = "WithFriends", // ← غيرت هنا
                CourtId = courtId,
                UserId = currentUserId.Value,
                Date = bookingDate,
                StartDate = bookingDate.Date.AddHours(hour),
                EndDate = bookingDate.Date.AddHours(hour + 1),
                Status = "Confirmed",
                TotalPrice = db.Courts.FirstOrDefault(c => c.CourtId == courtId)?.PricePerHour ?? 0
            };

            db.Bookings.Add(newBooking);
            db.SaveChanges();

            return RedirectToAction("WithFriends");
        }

        [HttpPost]
        [HttpPost]
        public IActionResult CancelFriendsBooking(int bookingId)
        {
            int? currentUserId = HttpContext.Session.GetInt32("UserId");
            if (currentUserId == null)
                return RedirectToAction("Login", "Account");

            var booking = db.Bookings.FirstOrDefault(b =>
                b.BookingId == bookingId &&
                b.UserId == currentUserId.Value);

            if (booking != null)
            {
                // امسح الـ SlotPlayers الأول
                var slot = db.Slots.FirstOrDefault(s => s.BookingId == bookingId);
                if (slot != null)
                {
                    var slotPlayers = db.SlotPlayers.Where(sp => sp.SlotId == slot.SlotId);
                    db.SlotPlayers.RemoveRange(slotPlayers);
                    db.Slots.Remove(slot);
                }

                db.Bookings.Remove(booking);
                db.SaveChanges();
            }

            return RedirectToAction("WithFriends");
        }
        #endregion



        #region TeamVsTeam IActionResults

        public IActionResult TeamVsTeam()
        {
            var courts = db.Courts.ToList();
            var bookings = db.Bookings.ToList();
            var users = db.Users.ToList();
            ViewData["Bookings"] = bookings;
            ViewData["Users"] = users;
            return View(courts);
        }

        [HttpPost]
        public IActionResult CreateTeamVsTeam(int courtId, string day, int hour)
        {
            int? currentUserId = HttpContext.Session.GetInt32("UserId");
            if (currentUserId == null)
                return RedirectToAction("Login", "Account");

            var today = DateTime.Now;
            int daysUntil = ((int)Enum.Parse<DayOfWeek>(day) - (int)today.DayOfWeek + 7) % 7;
            DateTime bookingDate = today.AddDays(daysUntil);

            bool alreadyBooked = db.Bookings
                .Any(b => b.CourtId == courtId &&
                          b.StartDate.Hour == hour &&
                          b.Date.Date == bookingDate.Date);

            if (alreadyBooked)
                return RedirectToAction("TeamVsTeam");

            var newBooking = new Booking
            {
                Type = "TeamVsTeam",
                CourtId = courtId,
                UserId = currentUserId.Value,
                Date = bookingDate,
                StartDate = bookingDate.Date.AddHours(hour),
                EndDate = bookingDate.Date.AddHours(hour + 1),
                Status = "Waiting",
                TotalPrice = db.Courts.FirstOrDefault(c => c.CourtId == courtId)?.PricePerHour ?? 0
            };

            db.Bookings.Add(newBooking);
            db.SaveChanges();
            return RedirectToAction("TeamVsTeam");
        }

        [HttpPost]
        public IActionResult JoinTeamVsTeam(int bookingId)
        {
            int? currentUserId = HttpContext.Session.GetInt32("UserId");
            if (currentUserId == null)
                return RedirectToAction("Login", "Account");

            var booking = db.Bookings.FirstOrDefault(b => b.BookingId == bookingId);
            if (booking != null && booking.OpponentUserId == null)
            {
                booking.OpponentUserId = currentUserId.Value;
                booking.Status = "Confirmed";
                db.SaveChanges();
            }

            return RedirectToAction("TeamVsTeam");
        }

        [HttpPost]
        public IActionResult LeaveTeamVsTeam(int bookingId)
        {
            int? currentUserId = HttpContext.Session.GetInt32("UserId");
            if (currentUserId == null)
                return RedirectToAction("Login", "Account");

            var booking = db.Bookings.FirstOrDefault(b => b.BookingId == bookingId);
            if (booking != null)
            {
                if (booking.UserId == currentUserId.Value)
                {
                    db.Bookings.Remove(booking);
                }
                else if (booking.OpponentUserId == currentUserId.Value)
                {
                    booking.OpponentUserId = null;
                    booking.Status = "Waiting";
                }
                db.SaveChanges();
            }

            return RedirectToAction("TeamVsTeam");
        }




    } 
        #endregion
}
