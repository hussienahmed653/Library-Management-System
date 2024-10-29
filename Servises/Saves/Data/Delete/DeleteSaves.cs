using Library_Management_System.Operations;

namespace Library_Management_System.Servises.Saves.Data.Delete
{
    public class DeleteSaves
    {
        public static bool DeleteSavesData(int UserId, int BookId) 
        {
            return Operation.Delete("DeleteFromTableSaves", UserId, BookId);
        }
    }
}
