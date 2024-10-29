using Library_Management_System.Servises.Books.Data.Delete;
using Library_Management_System.Servises.Books.Data.Get;
using Library_Management_System.Servises.Books.Data.Insert;
using Library_Management_System.Servises.Books.Data.Update;
using Library_Management_System.Servises.Books.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library_Management_System.Servises.Books.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BooksController : ControllerBase
    {
        [HttpGet("Get_All")]
        public IActionResult Get()
        {
            try
            {
                var MyDataBook = GetData.getdata();
                if(MyDataBook.Any(x => x.Id == 0))
                    return BadRequest("There are no books to view");
                return Ok(MyDataBook);
            }
            catch
            {
                return BadRequest("There are an error due to unhandled exception");
            }
        }
        [HttpPost("Get_All_By_Parameters")]
        public IActionResult Get(InsertBookModel model)
        {
            try
            {
                var book = GetData.getdatabyname(model);
                if (book.Any(x => x.Id == 0))
                {
                    return BadRequest("Could not find a book match your needs.");
                }
                return Ok(book);
            }
            catch
            {
                return BadRequest("There are an error due to unhandled exception");
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("Insert")]
        public IActionResult Insert(InsertBookModel insertBookModel)
        {
            try
            {
                if (InsertBook.InsertBookData(insertBookModel))
                    return Ok("Inserted successfully.");
                return BadRequest("Error! Couldn't insert.");
            }
            catch
            {
                return BadRequest("Error! Couldn't insert.");
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("Update")]
        public IActionResult Update(UpdateBookModel updateBookModel)
        {
            try
            {
                if (UpdateBook.UpdateBookDate(updateBookModel))
                    return Ok("Updated succefully.");
                return BadRequest("Error! Couldn't update.");
            }
            catch
            {
                return BadRequest("Error! Couldn't update.");
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("Delete")]
        public IActionResult Delete(int id)
        {
            try
            {
                if(DeleteBook.DeleteBookDate(id))
                    return Ok("Deleted successfully");
                return BadRequest("Error, Coudn't delete");
            }
            catch
            {
                return BadRequest("Error, Coudn't delete");
            }
        }

    }
}
