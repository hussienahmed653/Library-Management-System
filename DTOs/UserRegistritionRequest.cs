
namespace Library_Management_System.DTOs
{
    public class UserRegistritionRequest
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int UserType { get; set; }
        public string Action { get; } = "Insert";
    }
}
