namespace Lab5Borowy.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int SportClassId { get; set; }
        public DateTime ReservationDate { get; set; }
    }
}
