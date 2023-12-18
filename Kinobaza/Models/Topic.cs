using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Kinobaza.Models
{
    public class Topic
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? Title { get; set; }

        public string? Description { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public string? Author { get; set; }

        [Required]
        public List<Record>? Records { get; set; } = new List<Record>();


    }
}
