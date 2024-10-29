using Library_Management_System.Operations;
using Library_Management_System.Servises.Borrowings.Model;

namespace Library_Management_System.Servises.Borrowings.Data.Update
{
    public class ReturnBorrowinBook
    {
        public static bool ReturnBook(BorrowingsModel borrowingsModel)
        {
            return Operation.OpertionDataBook("ReturnBook", borrowingsModel);
        }
    }
}
