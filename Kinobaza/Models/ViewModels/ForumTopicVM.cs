using System.ComponentModel.DataAnnotations;

namespace Kinobaza.Models.ViewModels
{
    public class ForumTopicVM
    {
        public int Id { get; set; }

        public string? Login { get; set; }

        [Required(ErrorMessage = "Название темы должно быть указано!")]
        [StringLength(50, ErrorMessage = "Длина названия должна быть не более 50 символов.")]
        public string? Title { get; set; }

        [StringLength(250, ErrorMessage = "Длина описания должна быть не более 250 символов.")]
        public string? Description { get; set; }

        public string? Author { get; set; }

        public DateTime Date { get; set; }

        public int? RecordsCount { get; set; }

        public int? RecordToDelId { get; set; }

        public IEnumerable<ForumRecordVM>? RecordsVM { get; set; }
    }
}
