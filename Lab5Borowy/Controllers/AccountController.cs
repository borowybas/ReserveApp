using Lab5Borowy.Data;
using Lab5Borowy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Scripting;

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
            var user = _dbContext.Users.FirstOrDefault(u => u.Id == userId);

            if (user == null)
            {
                return RedirectToAction("Login");
            }

            return View(user);
        }
    }
}
