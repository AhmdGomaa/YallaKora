using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Linq;
using YallaKora.Models;

namespace YallaKora.Controllers
{
    public class CourtController : Controller
    {
        MyContext db = new MyContext();

        
        public IActionResult Index()
        {
            var courts = db.Courts.ToList();
            return View(courts);
        }

      
        [HttpGet]
        public IActionResult Add()
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin")
                return RedirectToAction("Index");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(Court newCourt, IFormFile imageFile)
        {
            
            if (imageFile == null)
            {
               
                System.Diagnostics.Debug.WriteLine("Image file is null!");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Image file received: " + imageFile.FileName);

                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Assets");
                if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

                string uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(fileStream);
                }
                newCourt.CourtImage = uniqueFileName;
            }

            db.Courts.Add(newCourt);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

   
        [HttpGet]
        public IActionResult Edit(int id)
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin")
                return RedirectToAction("Index");

            var court = db.Courts.Find(id);
            if (court == null) return NotFound();

            return View(court);
        }

     
        [HttpPost]
        public IActionResult Edit(Court updatedCourt)
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin")
                return RedirectToAction("Index");

            var courtInDb = db.Courts.Find(updatedCourt.CourtId);
            if (courtInDb == null) return NotFound();

            courtInDb.CourtName = updatedCourt.CourtName;
            courtInDb.Description = updatedCourt.Description;
            courtInDb.PricePerHour = updatedCourt.PricePerHour;
            courtInDb.CourtImage = updatedCourt.CourtImage;

            db.SaveChanges();
            return RedirectToAction("Index");
        }

    
        [HttpPost]
        public IActionResult Delete(int id)
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin")
                return RedirectToAction("Index");

            var court = db.Courts.Find(id);
            if (court != null)
            {
                db.Courts.Remove(court);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}