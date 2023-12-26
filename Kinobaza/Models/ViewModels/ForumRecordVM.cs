using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kinobaza.Models.ViewModels
{
    public class ForumRecordVM
    {
        public int Id { get; set; }
        public int TopicId { get; set; }

        [Required(ErrorMessage = "Запишите что-нибудь!")]
        public string? Text { get; set; }

        public string? Author { get; set; }

        public DateTime Date { get; set; }

        public string? ContentPaths { get; set; }

        public IEnumerable<string>? ContentPathsList { get; set; }
    }
}
