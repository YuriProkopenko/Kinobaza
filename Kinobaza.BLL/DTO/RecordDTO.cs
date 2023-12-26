using Kinobaza.DAL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kinobaza.BLL.DTO
{
    public class RecordDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Запишите что-нибудь!")]
        public string? Text { get; set; }

        public string? Author { get; set; }

        public DateTime Date { get; set; }

        public int TopicId { get; set; }

        public string? ContentPaths { get; set; }
    }
}
