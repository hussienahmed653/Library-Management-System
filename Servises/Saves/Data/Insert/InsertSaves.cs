using Library_Management_System.Operations;

namespace Library_Management_System.Servises.Saves.Data.Insert
{
    public class InsertSaves
    {
        public static bool OperationInsertSaves(int UserId, int BookId)
        {
            return Operation.InsertData("InsertIntoTableSaves", UserId, BookId);
        }
    }
}
