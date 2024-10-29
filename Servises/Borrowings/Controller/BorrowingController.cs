using Library_Management_System.Operations;
using Library_Management_System.Servises.Borrowings.Data.Get;
using Library_Management_System.Servises.Borrowings.Data.Insert;
using Library_Management_System.Servises.Borrowings.Data.Update;
using Library_Management_System.Servises.Borrowings.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library_Management_System.Servises.Borrowings.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BorrowingController : ControllerBase
    {
        [Authorize(Roles = "Admin")]
        [HttpGet("GetDataOfBorrowingsBook")]
        public IActionResult Get()
        {
            try
            {
                var MyData = GetAllDataOfBorrowingsBook.GetDataOfBorrowingsBook();
                if (MyData.Any(x => x.Id == 0))
                {
                    return BadRequest("No data here");
                }
                return Ok(MyData);
            }
            catch
            {
                return BadRequest("Error due to unhandled exception");
            }
        }
        [HttpPost("AddBookToBorrowings/{BookId}")]
        public IActionResult Add(int BookId)
        {
            try
            { 
                //Here this will be open for everyone and i will just take the bookid.
                string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                var UserId = Operation.GetIdFromToken(token);
                if(BorrowBook.RecordNewBookBorrowin(UserId, BookId))
                    return Ok("Borrowing recorded successfully.");
                return BadRequest("Unsuccessfully borrowing book");
            }
            catch
            {
                return BadRequest("Unsuccessfully borrowing book");
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("ReturnedUserDate")]
        public IActionResult Update(int UserId, int BookId)
        {
            try
            {
                BorrowingsModel borrowingsModel = new BorrowingsModel();
                borrowingsModel.UserId = UserId;
                borrowingsModel.BookId = BookId;
                if (ReturnBorrowinBook.ReturnBook(borrowingsModel))
                    return Ok("Book returned successfully.");
                return BadRequest("Unsuccessfully to return book");
            }
            catch
            {
                return BadRequest("Unsuccessfully to return book");
            }
        }
    }
}
