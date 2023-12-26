using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Kinobaza.DAL.Entities
{
    public class Topic
    {
        public int Id { get; set; }

        public string? Title { get; set; }

        public string? Description { get; set; }

        public DateTime Date { get; set; }

        public string? Author { get; set; }

        public IEnumerable<Record>? Records { get; set; }


    }
}
