
using Library_Management_System.Operations;

namespace Library_Management_System.Servises.Books.Data.Delete
{
    public class DeleteBook
    {
        public static bool DeleteBookDate(int id)
        {
            return Operation.Delete("DeleteBook", id);
        }
    }
}
