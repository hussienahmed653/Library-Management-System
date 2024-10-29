using System.Text.Json.Serialization;
namespace Library_Management_System.DTOs
{
    public class UserLoginRequest
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        [JsonIgnore]
        public string? Type { get; set; }
    }
}
