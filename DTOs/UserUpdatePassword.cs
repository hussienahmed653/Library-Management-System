namespace Library_Management_System.DTOs
{
    public class UserUpdatePassword
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmNewPassword { get; set; }
        public string Action { get; } = "Update";

    }
}
