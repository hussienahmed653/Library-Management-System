using System.Text.Json.Serialization;

namespace Library_Management_System.Servises.Borrowings.Model
{
    public class BorrowingsModel
    {
        public int? Id { get; set; }
        public int? UserId { get; set; }
        public int? BookId { get; set; }
        public DateTime? BorrowDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public DateTime? UserReturnDate { get; set; }
        public string? BookStatus { get; set; }
        [JsonIgnore]
        public string Action { get; } = "Update";
    }
}
