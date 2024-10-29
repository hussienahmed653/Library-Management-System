using Library_Management_System.Operations;
using Library_Management_System.Servises.Reservation.Model;
using Newtonsoft.Json;

namespace Library_Management_System.Servises.Reservation.Data.Get
{
    public class GetReservatoins
    {
        public static List<ReservationModel> GetAllBookReserved()
        {
            var MyData = Operation.GetData<ReservationModel>("GetAllReservation");
            if (MyData is null)
            {
                return new List<ReservationModel>()
                {
                    new ReservationModel()
                    {
                        Id = 0
                    }
                };
            }
            return JsonConvert.DeserializeObject<List<ReservationModel>>(MyData);
        }

        public static List<ReservationModel> GetUserBookReserved(ReservationModel reservationModel)
        {
            var mydata = Operation.GetData("GetUserReservation", reservationModel);
            if(mydata is null)
            {
                return new List<ReservationModel>()
                {
                    new ReservationModel()
                    {
                        Id = 0
                    }
                };
            }
            return JsonConvert.DeserializeObject<List<ReservationModel>>(mydata);
        }
    }
}
