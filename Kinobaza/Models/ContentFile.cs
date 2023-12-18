using System.ComponentModel.DataAnnotations;

namespace Kinobaza.Models
{
    public class ContentFile
    {
        [Key]
        public int Id { get; set; }
        public string? FilePath { get; set; }
        public int RecordId { get; set; }
    }
}
