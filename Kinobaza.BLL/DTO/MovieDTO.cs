using Kinobaza.DAL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kinobaza.BLL.DTO
{
    public class MovieDTO
    {
        public int Id { get; set; }

        public string? Poster { get; set; }


        [Required(ErrorMessage = "Укажите название фильма на русском")]
        public string? TitleRu { get; set; }

        public string? TitleEn { get; set; }

        [Required(ErrorMessage = "Укажите дату премьеры")]
        public DateTime? PremiereDate { get; set; }

        [Required(ErrorMessage = "Выберите жанры")]
        public IEnumerable<string>? GenresIds { get; set; }

        public IEnumerable<string?>? GenresNames { get; set; }

        [Required(ErrorMessage = "Укажите режиссера фильма")]
        public string? Director { get; set; }

        [Required(ErrorMessage = "Укажите, кто снимался в фильме")]
        [StringLength(500, ErrorMessage = "Длина строки должна быть не больше 500 символов")]
        public string? Cast { get; set; }

        [StringLength(3000, ErrorMessage = "Длина строки должна быть не больше 3000 символов")]
        public string? Description { get; set; }
    }
}
