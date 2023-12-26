using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kinobaza.BLL.DTO
{
    public class GenreDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Заполните поле с названием жанра.")]
        public string? Name { get; set; }

        public IEnumerable<string?>? MoviesNames { get; set; }
    }
}
