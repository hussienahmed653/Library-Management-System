using Library_Management_System.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library_Management_System.AuthunticationController
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        UserRepo userRepo;
        IConfiguration configuration;

        public AuthController(UserRepo userRepo, IConfiguration configuration)
        {
            this.userRepo = userRepo;
            this.configuration = configuration;
        }

        [HttpPost("Register")]
        public IActionResult Register(UserRegistritionRequest userRegistrition)
        {
            try
            {
                var user = UserRepo.HasUser(userRegistrition.Username);
                if (user.Username == "Not found" && !(UserRepo.GetByEmail(userRegistrition.Email)))
                {
                    UserRepo.UsersOperation(userRegistrition);
                    return Ok();
                }
                return BadRequest(new AuthResult()
                {
                    Errors = new List<string>()
                    {
                        "User or Email is already exists."
                    }
                });
            }
            catch
            {
                return BadRequest(new AuthResult()
                {
                    Errors = new List<string>()
                    {
                        "User or Email is already exists."
                    }
                });
            }
        }

        [HttpPost("Login")]
        public IActionResult Login(UserLoginRequest userLoginRequest)
        {
            try
            {
                var user = UserRepo.HasUser(userLoginRequest.Username);
                if(user.Username != userLoginRequest.Username ||
                    user.Password != userLoginRequest.Password)
                {
                    return BadRequest(
                        new AuthResult()
                        {
                            Errors = new List<string>
                            {
                                "Your Login credentials don't match an account in our system."
                            }
                        });
                }
                var token = JwtTokenHealper.GenerateToken(user.Type, user.Id, configuration["Jwt:Key"]);
                return Ok(new AuthResult()
                {
                    Token = token,
                    Errors = new List<string>()
                    { "No errors found" }
                });
            }
            catch
            {
                return BadRequest(
                        new AuthResult()
                        {
                            Errors = new List<string>
                            {
                                "Your Login credentials don't match an account in our system."
                            }
                        });
            }
        }

        [Authorize(Roles = "User")]
        [HttpPost("Change user password")]
        public IActionResult ChangePassword(UserUpdatePassword updatePassword)
        {
            try
            {
                var user = UserRepo.HasUser(updatePassword.Username);
                if (user.Password != updatePassword.Password &&
                    user.Username != updatePassword.Username)
                {
                    return BadRequest(new AuthResult()
                    {
                        Errors = new List<string>()
                        {
                            "Invalid username or current password."
                        }
                    });                
                }
                else if(updatePassword.NewPassword != updatePassword.ConfirmNewPassword)
                {
                    return BadRequest(new AuthResult()
                    {
                        Errors = new List<string>()
                        {
                            "The new password does not match. Enter the new password here again."
                        }
                    });
                }
                UserRepo.UsersOperation(updatePassword);
                return Ok();
            }
            catch
            {
                return BadRequest("Error!");
            }
        }
    }
}
