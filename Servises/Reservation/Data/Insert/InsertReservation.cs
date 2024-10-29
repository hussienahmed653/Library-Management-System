using Library_Management_System.Operations;

namespace Library_Management_System.Servises.Reservation.Data.Insert
{
    public class InsertReservation
    {
        public static bool InsertIntoReservation(int UserId, int BookId)
        {
            return Operation.InsertData("InsertIntoReservation", UserId, BookId);
        }
    }
}
