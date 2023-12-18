using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Kinobaza.Models.ViewModels
{
    public class MovieVM
    {
        public int MovieId { get; set; }

        public string? Image { get; set; }

        public string? TempTitle { get; set; }

        [Required(ErrorMessage = "Укажите название фильма на русском")]
        public string? TitleRu { get; set; }

        public string? TitleEn { get; set; }

        [Required(ErrorMessage = "Укажите дату премьеры")]
        public DateTime? PremiereDate { get; set; }

        [Required(ErrorMessage = "Выберите жанры")]
        public IEnumerable<string>? Genres { get; set; }

        [Required(ErrorMessage = "Укажите режиссера фильма")]
        public string? Director { get; set; }

        [Required(ErrorMessage = "Укажите, кто снимался в фильме")]
        [StringLength(500, ErrorMessage = "Длина строки должна быть не больше 500 символов")]
        public string? Cast { get; set; }

        [StringLength(3000, ErrorMessage = "Длина строки должна быть не больше 3000 символов")]
        public string? Description { get; set; }

        public bool IsFavorite { get; set; }

        public IEnumerable<SelectListItem>? Items { get; set; }
    }
}
