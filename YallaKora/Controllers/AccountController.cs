using Microsoft.AspNetCore.Mvc;
using YallaKora.Models;

namespace YallaKora.Controllers
{
    public class AccountController : Controller
    {
        MyContext db = new MyContext();


        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ViewBag.Error = "Please fill all fields";
                return View();
            }

            var user = db.Users.FirstOrDefault(u => u.Email == email && u.Password == password);

            if (user != null)
            {
               
                HttpContext.Session.SetString("UserName", user.FirstName);
                HttpContext.Session.SetInt32("UserId", user.UserId);
                HttpContext.Session.SetString("UserRole", user.Role);

                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Error = "Invalid email or password";
                return View();
            }
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(User user, IFormFile ProfileImage)
        {
            
            user.Role = "Player";

            
            ModelState.Remove("ProfileImage");
            ModelState.Remove("Role");

            if (ModelState.IsValid)
            {
                if (ProfileImage != null && ProfileImage.Length > 0)
                {
                    string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Assets");

                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(ProfileImage.FileName);
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await ProfileImage.CopyToAsync(fileStream);
                    }

                    user.ProfileImage = "/Assets/" + uniqueFileName;
                }
                else
                {
                    user.ProfileImage = "/Assets/default.png"; 
                }

                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction(nameof(Login));
            }

     
            return View(user);
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Account");
        }
    }
}
