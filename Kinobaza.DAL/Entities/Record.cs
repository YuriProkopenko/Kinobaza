using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kinobaza.DAL.Entities
{
    public class Record
    {
        public int Id { get; set; }

        public string? Text { get; set; }

        public string? Author { get; set; }

        public DateTime Date { get; set; }

        public string? ContentPaths { get; set; }

        public Topic? Topic { get; set; }

        public int TopicId { get; set; }

    }
}