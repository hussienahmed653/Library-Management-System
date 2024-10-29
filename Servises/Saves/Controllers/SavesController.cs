

using Library_Management_System.Operations;
using Library_Management_System.Servises.Saves.Data.Delete;
using Library_Management_System.Servises.Saves.Data.Get;
using Library_Management_System.Servises.Saves.Data.Insert;
using Library_Management_System.Servises.Saves.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library_Management_System.Servises.Saves.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SavesController : ControllerBase
    {
        [HttpPost("GetAllSaves")]
        public ActionResult Get()
        {
            try
            {
                SavesModel savesModel = new SavesModel();
                string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                var UserId = Operation.GetIdFromToken(token);
                savesModel.UserId = UserId;
                var MyDataSaves = GetSaves.GetAllSaves(savesModel);
                if(MyDataSaves.Any(x => x.Id == 0))
                {
                    return BadRequest("There are no books saves!");
                }
                return Ok(MyDataSaves);
            }
            catch
            {
                return BadRequest("Can't get data due to unhandled exception");
            }
        }
        [HttpPost("InsertSaves")]
        public IActionResult Save(int BookId)
        {
            try
            {
                string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                var UserId = Operation.GetIdFromToken(token);
                if (InsertSaves.OperationInsertSaves(UserId, BookId))
                {
                    return Ok("Saved successfully");
                }
                return BadRequest("Can't save due to unhandled exception");
            }
            catch
            {
                return BadRequest("Can't save due to unhandled exception");
            }
        }

        [HttpDelete("DeleteFromSaves")]
        public IActionResult Delete(int BookId)
        {
            try
            {
                string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                var UserId = Operation.GetIdFromToken(token);
                if(DeleteSaves.DeleteSavesData(UserId, BookId))
                {
                    return Ok("Unsaves successfully");
                }
                return BadRequest("Can't unsave due to unhandled exception");
            }
            catch
            {
                return BadRequest("Can't delete due to unhandled exception");
            }
        }
    }
}
