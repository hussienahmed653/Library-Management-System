using Library_Management_System.Operations;
using Library_Management_System.Servises.Saves.Models;
using Newtonsoft.Json;

namespace Library_Management_System.Servises.Saves.Data.Get
{
    public class GetSaves
    {
        public static List<SavesModel> GetAllSaves(SavesModel savesModel)
        {
            var MyJsonString = Operation.GetData("GetDataFromSaves", savesModel);
            if (MyJsonString == null)
            {
                return new List<SavesModel>()
                {
                    new SavesModel()
                    {
                        Id = 0
                    }
                };
            }
            return JsonConvert.DeserializeObject<List<SavesModel>>(MyJsonString);
        }
    }
}
