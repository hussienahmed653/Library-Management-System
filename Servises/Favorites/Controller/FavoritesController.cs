

using Library_Management_System.Operations;
using Library_Management_System.Servises.Favorites.Data.Delete;
using Library_Management_System.Servises.Favorites.Data.Get;
using Library_Management_System.Servises.Favorites.Data.Insert;
using Library_Management_System.Servises.Favorites.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library_Management_System.Servises.Favorites.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FavoritesController : ControllerBase
    {
        
        [HttpGet("GetYourFavorites")]
        public IActionResult Get()
        {
            try
            {
                FavoritesModel model = new FavoritesModel();
                var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                var UserId = Operation.GetIdFromToken(token);
                model.UserId = UserId;
                var mydata = GetFavorites.GetAllFavorites(model);
                if(mydata.Any(x => x.Id == 0))
                {
                    return BadRequest("No Books in favorites");
                }
                return Ok(mydata);
            }
            catch
            {
                return BadRequest("Error!");
            }
        }
        [HttpPost("InsertIntoFavorites")]
        public IActionResult insert(int BookId)
        {
            try
            {
                FavoritesModel model = new FavoritesModel();
                var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                var UserId = Operation.GetIdFromToken(token);
                model.UserId = UserId;
                model.BookId = BookId;
                if(InsertIntoFavorites.InsertIntoFavoritesTable(UserId, BookId))
                {
                    return Ok("Added to favorites");
                }
                return BadRequest("Couldn't add into favorites");
            }
            catch
            {
                return BadRequest("Couldn't add into favorites");
            }
        }
        [HttpDelete("DeleteFromFavorites")]
        public IActionResult delete(int BookId)
        {
            try
            {
                var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                var UserId = Operation.GetIdFromToken(token);
                if(DeleteFromFavorites.DeleteFavorites(UserId, BookId))
                {
                    return Ok("Removed from favorite successfully.");
                }
                return BadRequest("Coudln't remove from favorites");
            }
            catch
            {
                return BadRequest("Coudln't remove from favorites");
            }

        }
    }
}
