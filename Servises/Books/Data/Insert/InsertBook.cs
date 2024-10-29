using Library_Management_System.Operations;
using Library_Management_System.Servises.Books.Models;

namespace Library_Management_System.Servises.Books.Data.Insert
{
    public class InsertBook
    {
        public static bool InsertBookData(InsertBookModel insertBookModel)
        {
            return Operation.OpertionDataBook("OperationBooks", insertBookModel);
        }
    }
}
