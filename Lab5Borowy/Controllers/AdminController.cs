using Lab5Borowy.Data;
using Lab5Borowy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Lab5Borowy.Controllers
{
    public class AdminController : Controller
    {
        private readonly Lab5BorowyContext _context;

        public AdminController(Lab5BorowyContext context)
        {
            _context = context;
        }

        // Form of adding new class
        public IActionResult Index()
        {
            return View(_context.SportClasses.ToList());
        }

        // Formularz dodawania nowego zajęcia
        [HttpGet]
        public IActionResult AddSportClass()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //public IActionResult AddSportClass(SportClass sportClass)
        public IActionResult AddSportClass([Bind("Id, Name, Date, StartTime, Duration, Capacity, Reserved")] SportClass sportClass)
        {
            ModelState.ClearValidationState("ValidateReservation");
            ModelState.MarkFieldValid("ValidateReservation");
            if (ModelState.IsValid) 
            {
                
                _context.SportClasses.Add(sportClass);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sportClass);
        }

        // GET: Admin/ReservationList/5
        [HttpGet]
        public IActionResult ReservationList(int id)
        {
            var reservations = _context.Reservations
                .Where(r => r.SportClassId == id)
                .Join(
                    _context.Users, // Druga tabela: Users
                    r => r.UserId, // Klucz z Reservations
                    u => u.Id, // Klucz z Users
                    (r, u) => new // Wynik połączenia
                    {
                        UserName = u.Username,
                        r.ReservationDate,
                        r.UserId,
                        r.Id
                    }

                )
                .ToList();

            return View(reservations);
            
        }
    }

}
