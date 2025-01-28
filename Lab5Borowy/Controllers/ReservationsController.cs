using Lab5Borowy.Data;
using Lab5Borowy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lab5Borowy.Controllers
{
    public class ReservationsController : Controller
    {
        private readonly Lab5BorowyContext _context;

        public ReservationsController(Lab5BorowyContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }


        // GET: Reservations/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var reservation = await _context.Reservations
                .Where(r => r.Id == id)
                .Select(r => new UserReservationViewModel
                {
                    Id = r.Id,
                    ReservationDate = r.ReservationDate,
                    SportClassId = r.SportClassId,
                    SportClassName = _context.SportClasses
                                        .Where(sc => sc.Id == r.SportClassId)
                                        .Select(sc => sc.Name)
                                        .FirstOrDefault(),
                    SportClassDate = _context.SportClasses
                                        .Where(sc => sc.Id == r.SportClassId)
                                        .Select(sc => sc.Date)
                                        .FirstOrDefault(),
                    SportClassStartTime = _context.SportClasses
                                        .Where(sc => sc.Id == r.SportClassId)
                                        .Select(sc => sc.StartTime)
                                        .FirstOrDefault(),
                    SportClassDuration = _context.SportClasses
                                        .Where(sc => sc.Id == r.SportClassId)
                                        .Select(sc => sc.Duration)
                                        .FirstOrDefault(),
                    SportClassCapacity = _context.SportClasses
                                        .Where(sc => sc.Id == r.SportClassId)
                                        .Select(sc => sc.Capacity)
                                        .FirstOrDefault(),
                    SportClassReserved = _context.SportClasses
                                        .Where(sc => sc.Id == r.SportClassId)
                                        .Select(sc => sc.Reserved)
                                        .FirstOrDefault(),
                })
                .FirstOrDefaultAsync();

            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        [HttpPost]
        public IActionResult Cancel(int id, int? userId = null)
        {
            var reservation = _context.Reservations.FirstOrDefault(r => r.Id == id);

            if (reservation == null)
            {
                return NotFound(); // Rezerwacja nie istnieje
            }

            // Jeśli admin (userId != null), można anulować dowolną rezerwację
            if (userId == null)
            {
                var currentUserId = HttpContext.Session.GetInt32("UserId");

                // Użytkownik może anulować tylko własne rezerwacje
                if (reservation.UserId != currentUserId)
                {
                    return Forbid(); // Brak uprawnień
                }
            }

            // Pobierz klasę sportową powiązaną z rezerwacją
            var sportClass = _context.SportClasses.FirstOrDefault(sc => sc.Id == reservation.SportClassId);
            if (sportClass != null)
            {
                // Zmniejsz liczbę zarezerwowanych miejsc
                sportClass.Reserved = Math.Max(sportClass.Reserved - 1, 0); // Upewnij się, że Reserved nie będzie ujemne
            }

            // Usuń rezerwację
            _context.Reservations.Remove(reservation);
            _context.SaveChanges();


            TempData["Message"] = "Reservation cancelled successfully.";

            // Jeśli admin anulował, wróć do listy rezerwacji; inaczej wróć do rezerwacji użytkownika
            return userId != null
                ? RedirectToAction("ReservationList", "Admin", new { id = reservation.SportClassId })
                : RedirectToAction("Welcome", "Account");
        }

    }
}
