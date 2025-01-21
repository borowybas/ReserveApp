using Lab5Borowy.Data;
using Lab5Borowy.Models;
using Microsoft.AspNetCore.Mvc;
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

    }
}
