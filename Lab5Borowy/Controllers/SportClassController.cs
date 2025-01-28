using Lab5Borowy.Data;
using Lab5Borowy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            var userIdx = HttpContext.Session.GetInt32("UserId");

            if (userIdx == null)
            {
                return RedirectToAction("Login", "Account");
            }

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

        // GET: SportClass/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sportClass = await _context.SportClasses.FindAsync(id);
            if (sportClass == null)
            {
                return NotFound();
            }
            return View(sportClass);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Date,StartTime,Duration,Capacity,Reserved")] SportClass sportClass)
        {
            if (id != sportClass.Id)
            {
                return NotFound();
            }

            ModelState.ClearValidationState("ValidateReservation");
            ModelState.MarkFieldValid("ValidateReservation");
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sportClass);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SportClassExists(sportClass.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(sportClass);
        }

        private bool SportClassExists(int id)
        {
            return _context.SportClasses.Any(e => e.Id == id);
        }

        // GET: SportClass/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sportClass = await _context.SportClasses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sportClass == null)
            {
                return NotFound();
            }

            return View(sportClass);
        }

        // POST: SportClass/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sportClass = await _context.SportClasses.FindAsync(id);
            if (sportClass != null)
            {
                _context.SportClasses.Remove(sportClass);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
