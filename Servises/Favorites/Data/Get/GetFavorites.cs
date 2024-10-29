using Library_Management_System.Operations;
using Library_Management_System.Servises.Favorites.Model;
using Newtonsoft.Json;

namespace Library_Management_System.Servises.Favorites.Data.Get
{
    public class GetFavorites
    {
        public static List<FavoritesModel> GetAllFavorites(FavoritesModel favoritesModel)
        {
            var mydata = Operation.GetData("GetAllUserFavorites", favoritesModel);
            if(mydata is null)
            {
                return new List<FavoritesModel>()
                {
                    new FavoritesModel()
                    {
                        Id = 0
                    }
                };
            }
            return JsonConvert.DeserializeObject<List<FavoritesModel>>(mydata);
        }
    }
}
