using Library_Management_System.Operations;
using Library_Management_System.Servises.Borrowings.Model;
using Newtonsoft.Json;

namespace Library_Management_System.Servises.Borrowings.Data.Get
{
    public class GetAllDataOfBorrowingsBook
    {
        public static List<BorrowingsModel> GetDataOfBorrowingsBook()
        {
            var MyData = Operation.GetData<BorrowingsModel>("GetAllDataOfBorrowingsBook");
            if(MyData is null)
            {
                return new List<BorrowingsModel>()
                {
                    new BorrowingsModel()
                    {
                        Id = 0
                    }
                };
            }
            return JsonConvert.DeserializeObject<List<BorrowingsModel>>(MyData);
        }
    }
}
