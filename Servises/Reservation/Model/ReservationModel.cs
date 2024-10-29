namespace Library_Management_System.Servises.Reservation.Model
{
    public class ReservationModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int BookId { get; set; }
        public DateTime ReservationsDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int StatusId { get; set; }
    }
}
