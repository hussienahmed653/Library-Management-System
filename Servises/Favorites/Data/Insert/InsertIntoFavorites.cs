using Library_Management_System.Operations;

namespace Library_Management_System.Servises.Favorites.Data.Insert
{
    public class InsertIntoFavorites
    {
        public static bool InsertIntoFavoritesTable(int UserId, int BookId)
        {
            return Operation.InsertData("InsertIntoFavorites", UserId, BookId);
        }
    }
}
