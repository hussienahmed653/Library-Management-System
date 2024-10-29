
using Library_Management_System.Operations;
using Library_Management_System.Servises.Books.Models;
using Newtonsoft.Json;

namespace Library_Management_System.Servises.Books.Data.Get
{
    public class GetData
    {
        public static List<InsertBookModel> getdata()
        {
            var MyJsonString = Operation.GetData<InsertBookModel>("GetAllBooks");
            if(MyJsonString == null)
            {
                return new List<InsertBookModel>()
                {
                    new InsertBookModel()
                    {
                        Id = 0
                    }
                };
            }
            return JsonConvert.DeserializeObject<List<InsertBookModel>>(MyJsonString);
        }
        public static List<InsertBookModel> getdatabyname(InsertBookModel model)
        {
            var deserialization = Operation.GetData("GetAllBooksByParameter", model);
            if (deserialization is null)
                return new List<InsertBookModel>()
                {
                    new InsertBookModel()
                    {
                        Id = 0
                    }
                };
            else
                return JsonConvert.DeserializeObject<List<InsertBookModel>>(deserialization);

        }
    }
}
