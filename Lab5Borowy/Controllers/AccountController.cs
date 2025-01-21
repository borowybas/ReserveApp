using Lab5Borowy.Data;
using Lab5Borowy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.EntityFrameworkCore;

namespace Lab5Borowy.Controllers
{
    public class AccountController : Controller
    {
       

        // Temporary store users in memory
        private static List<User> _users = new List<User>();
        //private readonly Lab5BorowyContext _dbContext = new Lab5BorowyContext();
        private readonly Lab5BorowyContext _dbContext;

        public AccountController(Lab5BorowyContext dbContext)
        {
            _dbContext = dbContext;
        }



        // GET: Account/Register
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        // POST: Account/Register
        [HttpPost]
        public IActionResult Register(string username, string password)
        {
            // Check if there anr any users of given username
            if (_dbContext.Users.Any(u => u.Username == username))
            {
                ViewBag.Error = "Username is already taken";
                return View();
            }

            var newUser = new User
            {
                //Id = _users.Count() + 1,
                Username = username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password)
            };

            _dbContext.Users.Add(newUser);
            _dbContext.SaveChanges();

            ViewBag.Success = "Registration successful. Please log in.";
            return RedirectToAction("Login");
        }

        // GET: Account/Login
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Username == username);

            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                ViewBag.Error = "Invalid username or password.";
                return View();
            }

            //Session["UserId"] = user.Id; // Przechowujemy identyfikator użytkownika w sesji
            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("UserRole", user.Role);
            return RedirectToAction("Welcome"); // Przekierowanie na stronę główną
        }

        public IActionResult Logout()
        {
            //Session["UserId"] = null; // Wylogowanie
            HttpContext.Session.Remove("UserId"); // Usuń UserId z sesji
            HttpContext.Session.Clear(); // Opcjonalnie: wyczyść całą sesję
            return RedirectToAction("Login");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _dbContext.Dispose();
            }
            base.Dispose(disposing);
        }

        [HttpGet]
        public IActionResult Welcome()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login");
            }

            // Pobranie danych użytkownika z bazy danych
            //var user = _dbContext.Users.FirstOrDefault(u => u.Id == userId);

            //if (user == null)
            //{
            //    return RedirectToAction("Login");
            //}

            //var reservations = _dbContext.Reservations
            //    .Where(r => r.UserId == userId)
            //    .Include(r => r.SportClassId)
            //    .ToList();

            var reservations = _dbContext.Reservations
                .Where(r => r.UserId == userId)
                .Select(r => new
                {
                    r.Id,
                    r.SportClassId,
                    SportClassName = _dbContext.SportClasses
                        .Where(sc => sc.Id == r.SportClassId)
                        .Select(sc => sc.Name)
                        .FirstOrDefault(),
                    SportClassDate = _dbContext.SportClasses
                        .Where(sc => sc.Id == r.SportClassId)
                        .Select(sc => sc.Date)
                        .FirstOrDefault()
                })
                .ToList();

            var model = new UserReservationsViewModel
            {
                Reservations = reservations.Select(r => new UserReservationViewModel
                {
                    Id = r.Id,
                    SportClassId = r.SportClassId,
                    SportClassName = r.SportClassName,
                    SportClassDate = r.SportClassDate
                }).ToList()
            };

            //var model = new WelcomeViewModel
            //{
            //    User = user,
            //    Reservations = reservations
            //};

            return View(model);
        }
    }
}
