namespace Library_Management_System.DTOs
{
    public class AuthResult
    {
        public string Token { get; set; }
        public List<string> Errors { get; set; }
    }
}
