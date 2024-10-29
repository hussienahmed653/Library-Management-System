using Library_Management_System.Operations;

namespace Library_Management_System.Servises.Borrowings.Data.Insert
{
    public class BorrowBook
    {
        public static bool RecordNewBookBorrowin(int UserId, int BookId)
        {
            return Operation.InsertData("AddBorrowing", UserId, BookId);
        }
    }
}
