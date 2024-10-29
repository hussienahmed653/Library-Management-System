using System.Text.Json.Serialization;

namespace Library_Management_System.Servises.Saves.Models
{
    public class SavesModel
    {
        [JsonIgnore]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int BookId { get; set; }
        public DateTime SaveDate { get; set; }
    }
}
