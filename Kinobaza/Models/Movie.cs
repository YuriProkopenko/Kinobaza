using System.ComponentModel.DataAnnotations;

namespace Kinobaza.Models
{
    public class Movie
    {
        [Key] 
        public int Id { get; set; }

        public string? Image {  get; set; }

        public string? TitleRU { get; set; }

        public string? TitleEN { get;set; }

        public DateTime? PremiereDate { get; set;}

        public string? Director { get; set; }

        public string? Cast { get; set; }

        public string? Description { get; set; }

        public List<Genre>? Genres { get; set; }
    }
}
