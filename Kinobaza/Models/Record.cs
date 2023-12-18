using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Kinobaza.Models
{
    public class Record
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? Text { get; set; }

        [Required]
        public string? Author { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public List<ContentFile>? Files { get; set; }

        [Required]
        public Topic? Topic { get; set; }

        [Required]
        public int TopicId { get; set; }
    }
}