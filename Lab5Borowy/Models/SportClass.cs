using System.ComponentModel.DataAnnotations.Schema;

namespace Lab5Borowy.Models
{
    public class SportClass
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan StartTime {  get; set; }
        public TimeSpan Duration { get; set; }
        public int Capacity { get; set; }
        public int Reserved { get; private set; } // number of reserved places

        
        public event EventHandler? ClasstFull;

        public delegate bool ReservationValidationHandler(SportClass sportClass, int userId);
        
        public ReservationValidationHandler ValidateReservation { get; set; }
        
        
        public void AddReservation(int userId)
        {
            if (ValidateReservation != null && !ValidateReservation(this, userId))
            {
                throw new InvalidOperationException("Reservation validation failed.");
            }

            Reserved++;
            if (Reserved == Capacity)
            {
                ClasstFull?.Invoke(this, EventArgs.Empty);
            }

        }
    }
}
