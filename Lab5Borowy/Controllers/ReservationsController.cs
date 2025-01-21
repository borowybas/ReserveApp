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
    }
}
