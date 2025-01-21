namespace Lab5Borowy.Models
{
    public class UserReservationsViewModel
    {
        public List<UserReservationViewModel> Reservations { get; set; }
    }

    public class UserReservationViewModel
    {
        public int Id { get; set; }
        public int SportClassId { get; set; }
        public DateTime ReservationDate { get; set; }
        public string SportClassName { get; set; }
        public DateTime SportClassDate { get; set; }

        public TimeSpan SportClassStartTime { get; set; }
        public TimeSpan SportClassDuration { get; set; }
        public int SportClassCapacity { get; set; }
        public int SportClassReserved { get; set; }

    }
}
