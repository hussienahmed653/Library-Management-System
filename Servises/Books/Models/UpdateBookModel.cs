using System.Text.Json.Serialization;

namespace Library_Management_System.Servises.Books.Models
{
    public class UpdateBookModel 
    {
        public int? Id { get; set; }
        public string? Title { get; set; }
        public string? Classification { get; set; }
        public DateTime? YearOfPublication { get; set; }
        public string? Author { get; set; }
        [JsonIgnore]
        public string Action { get; } = "Update";
    }
}
