using Kinobaza.DAL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kinobaza.BLL.DTO
{
    public class TopicDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Название темы должно быть указано!")]
        [StringLength(50, ErrorMessage = "Длина названия должна быть не более 50 символов.")]

        public string? Title { get; set; }

        [StringLength(250, ErrorMessage = "Длина описания должна быть не более 250 символов.")]
        public string? Description { get; set; }

        public DateTime Date { get; set; }

        public string? Author { get; set; }

        public List<RecordDTO>? RecordDTOs { get; set; }
    }
}
