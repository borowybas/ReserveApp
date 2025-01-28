using Lab5Borowy.Data;
using Lab5Borowy.Models;
using Microsoft.AspNetCore.Mvc;

namespace ReserveApp.Controllers
{
    public class SportClassController : Controller
    {
        private readonly Lab5BorowyContext _context;
        
        public SportClassController (Lab5BorowyContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var isAdmin = HttpContext.Session.GetString("UserRole") == "Admin";
            var userId = HttpContext.Session.GetInt32("UserId");
            // Przekazanie tej informacji do widoku
            ViewData["IsAdmin"] = isAdmin;

            var classes = _context.SportClasses
                .Where(c => c.Date >= DateTime.Today)
                .OrderBy(c => c.Date)
                .ToList();

            var userReservations = userId != null
                ? _context.Reservations
                    .Where(r => r.UserId == userId)
                    .Select(r => r.SportClassId)
                    .ToList()
                : new List<int>();

            ViewData["UserReservations"] = userReservations;
            ViewBag.UserReservations = userReservations;

            return View(classes);
        }

        [HttpPost]
        public IActionResult Reserve(int id)
        {
            var sportClass = _context.SportClasses.FirstOrDefault(c => c.Id == id);

            if (sportClass == null)
            {
                return NotFound();
            }

            // Using Delegate for reservation
            sportClass.ValidateReservation = (cls, userId) =>
            {
                return _context.Reservations
                    .Count(r => r.SportClassId == cls.Id && r.UserId == userId) == 0;
            };

            // Using Event
            sportClass.ClasstFull += (sender, args) =>
            {
                Console.WriteLine($"Class {sportClass.Name} is now full");
            };

            try
            {
                sportClass.AddReservation(GetCurrentUserId());
                _context.Reservations.Add(new Reservation
                {
                    UserId = GetCurrentUserId(),
                    SportClassId = sportClass.Id,
                    ReservationDate = DateTime.Now
                });
                _context.SaveChanges();
            }
            catch (Exception ex) 
            { 
                ModelState.AddModelError("", ex.Message);
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");

        }

        public int GetCurrentUserId()
        {
            return HttpContext.Session.GetInt32("UserId") ?? 0;
        }
    }
}
