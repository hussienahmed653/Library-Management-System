using Library_Management_System.Operations;
using Library_Management_System.Servises.Books.Models;

namespace Library_Management_System.Servises.Books.Data.Update
{
    public class UpdateBook
    {
        public static bool UpdateBookDate(UpdateBookModel updateBookModel)
        {
            return Operation.OpertionDataBook("OperationBooks", updateBookModel);
        }
    }
}
