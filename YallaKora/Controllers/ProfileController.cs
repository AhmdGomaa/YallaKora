using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YallaKora.Models;

namespace YallaKora.Controllers
{
    public class ProfileController : Controller
    {
        MyContext db = new MyContext();
        [HttpGet]
        [HttpGet]
        [HttpGet]
        [HttpGet]
        public IActionResult Index(int? id)
        {
        
            int targetId = id ?? HttpContext.Session.GetInt32("UserId") ?? 0;

            if (targetId == 0)
            {
                return RedirectToAction("Login", "Account");
            }

            var user = db.Users.FirstOrDefault(u => u.UserId == targetId);
            if (user == null)
            {
                return NotFound();
            }

            var reviews = db.Reviews.Where(r => r.ToUserId == targetId).ToList();
            var reviewerIds = reviews.Select(r => r.FromUserId).Distinct().ToList();
            var userReviews = db.Users.Where(u => reviewerIds.Contains(u.UserId)).ToList();

            ViewData["Reviews"] = reviews;
            ViewData["UserReviews"] = userReviews;

            return View(user);
        }


        [HttpPost]
        public IActionResult Review(int id, int Rating, string Comment)
        {
            int? currentUserId = HttpContext.Session.GetInt32("UserId");

            if (currentUserId == null)
            {
                return RedirectToAction("Login", "Account");
            }

 
            var newReview = new Review
            {
                FromUserId = currentUserId.Value,
                ToUserId = id,                    
                Rating = Rating,                  
                Comment = Comment,
                Date = DateTime.Now              
            };

            db.Reviews.Add(newReview);
            db.SaveChanges();

         
            return RedirectToAction("Index", new { id = id });
        }
        [HttpGet]
        public async Task<IActionResult> UsersList()
        {
        
            var users = await db.Users.ToListAsync();
            return View(users);
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            int? currentUserId = HttpContext.Session.GetInt32("UserId");
            string currentUserRole = HttpContext.Session.GetString("UserRole");

        
            if (currentUserId == null || (currentUserId != id && currentUserRole != "Admin"))
            {
                return RedirectToAction("Index", "Home");
            }

            var user = db.Users.FirstOrDefault(u => u.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

       
        [HttpPost]
        public IActionResult Edit(User updatedUser)
        {
            int? currentUserId = HttpContext.Session.GetInt32("UserId");
            string currentUserRole = HttpContext.Session.GetString("UserRole");

     
            if (currentUserId == null || (currentUserId != updatedUser.UserId && currentUserRole != "Admin"))
            {
                return RedirectToAction("Index", "Home");
            }

            var userInDb = db.Users.FirstOrDefault(u => u.UserId == updatedUser.UserId);
            if (userInDb == null)
            {
                return NotFound();
            }

      
            userInDb.FirstName = updatedUser.FirstName;
            userInDb.LastName = updatedUser.LastName;
            userInDb.UserName = updatedUser.UserName;
            userInDb.Email = updatedUser.Email;
            userInDb.PhoneNumber = updatedUser.PhoneNumber;
            userInDb.Address = updatedUser.Address;
            userInDb.Age = updatedUser.Age;
            userInDb.UserPosition = updatedUser.UserPosition;
            userInDb.ProfileImage = updatedUser.ProfileImage;

            db.SaveChanges();

            
            return RedirectToAction("Index", new { id = updatedUser.UserId });
        }

       
        [HttpPost]
        public IActionResult Delete(int id)
        {
            int? currentUserId = HttpContext.Session.GetInt32("UserId");
            string currentUserRole = HttpContext.Session.GetString("UserRole");

            if (currentUserId == null || (currentUserId != id && currentUserRole != "Admin"))
            {
                return RedirectToAction("Index", "Home");
            }

            var user = db.Users.FirstOrDefault(u => u.UserId == id);
            if (user != null)
            {
               
                var slotPlayers = db.SlotPlayers.Where(sp => sp.UserId == id).ToList();
                if (slotPlayers.Any())
                {
                    db.SlotPlayers.RemoveRange(slotPlayers);
                }

            
                var userBookings = db.Bookings.Where(b => b.UserId == id).ToList();
                foreach (var booking in userBookings)
                {
                    var slot = db.Slots.FirstOrDefault(s => s.BookingId == booking.BookingId);
                    if (slot != null)
                    {
                      
                        var otherSlotPlayers = db.SlotPlayers.Where(sp => sp.SlotId == slot.SlotId).ToList();
                        if (otherSlotPlayers.Any())
                        {
                            db.SlotPlayers.RemoveRange(otherSlotPlayers);
                        }
                        db.Slots.Remove(slot);
                    }
                    db.Bookings.Remove(booking);
                }

             
                var reviews = db.Reviews.Where(r => r.FromUserId == id || r.ToUserId == id).ToList();
                if (reviews.Any())
                {
                    db.Reviews.RemoveRange(reviews);
                }

                db.Users.Remove(user);
                db.SaveChanges();

                if (currentUserId == id)
                {
                    HttpContext.Session.Clear();
                    return RedirectToAction("Index", "Home");
                }
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
