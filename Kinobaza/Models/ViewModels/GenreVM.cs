using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Kinobaza.Models.ViewModels
{
    public class GenreVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Название должно быть установлено.")]
        public string? Name { get; set; }

        public string? TitleName { get; set; }

        public IEnumerable<string?>? MoviesNames { get; set; }
    }
}
