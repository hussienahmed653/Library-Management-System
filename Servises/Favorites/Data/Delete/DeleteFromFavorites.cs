using Library_Management_System.Operations;

namespace Library_Management_System.Servises.Favorites.Data.Delete
{
    public class DeleteFromFavorites
    {
        public static bool DeleteFavorites(int UserId, int BookId)
        {
            return Operation.Delete("DeleteFromFavorites", UserId, BookId);
        }
    }
}
