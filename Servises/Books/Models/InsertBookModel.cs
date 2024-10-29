using System.Text.Json.Serialization;

namespace Library_Management_System.Servises.Books.Models
{
    public class InsertBookModel
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string? Title { get; set; }
        public int? ClassificationId { get; set; }
        public DateTime? YearOfPublication { get; set; }
        public string? Author { get; set; }
        public bool? IsValid { get; set; }
        public int ValidIn { get; set; }
        [JsonIgnore]
        public string Action { get; } = "Insert";
    }
}
