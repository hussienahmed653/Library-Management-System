using Library_Management_System.Operations;
using Library_Management_System.Servises.Reservation.Data.Get;
using Library_Management_System.Servises.Reservation.Data.Insert;
using Library_Management_System.Servises.Reservation.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library_Management_System.Servises.Reservation.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReservationController : ControllerBase
    {
        [HttpGet("GetUserReservation")]
        public IActionResult GetUser()
        {
            try
            {
                ReservationModel reservationModel = new ReservationModel();
                var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                var UserId = Operation.GetIdFromToken(token);
                reservationModel.UserId = UserId;
                var mydata = GetReservatoins.GetUserBookReserved(reservationModel);
                if (mydata.Any(x => x.Id == 0))
                {
                    return BadRequest("No Books Reserved yet");
                }
                return Ok(mydata);
            }
            catch
            {
                return BadRequest("Error!");
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("GetAllReservation")]
        public IActionResult GetAll()
        {
            try
            {
                var mydata = GetReservatoins.GetAllBookReserved();
                if(mydata.Any(x => x.Id == 0))
                {
                    return BadRequest("No Books Reserved yet");
                }
                return Ok(mydata);
            }
            catch
            {
                return BadRequest("Error!");
            }
        }
        [HttpPost("InsertIntoReservation")]
        public IActionResult Insert(int BookId)
        {
            try
            {
                var token = Request.Headers["Authorization"].ToString().Replace("Bearer", "");
                var UserId = Operation.GetIdFromToken(token);
                if(InsertReservation.InsertIntoReservation(UserId, BookId))
                {
                    return Ok("The book has been successfully reserved.");
                }
                return BadRequest("couldn't reserve the book");
            }
            catch
            {
                return BadRequest("couldn't reserve the book");
            }
        }
    }
}
