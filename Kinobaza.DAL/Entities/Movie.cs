using System.ComponentModel.DataAnnotations;

namespace Kinobaza.DAL.Entities
{
    public class Movie
    {
        public int Id { get; set; }

        public string? Poster {  get; set; }

        public string? TitleRu { get; set; }

        public string? TitleEn { get;set; }

        public DateTime? PremiereDate { get; set;}

        public string? Director { get; set; }

        public string? Cast { get; set; }

        public string? Description { get; set; }

        public IEnumerable<Genre>? Genres { get; set; }
    }
}
