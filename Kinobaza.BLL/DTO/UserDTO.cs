using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kinobaza.BLL.DTO
{
    public class UserDTO
    {
        public int Id { get; set; }

        [Required]
        public string? Login { get; set; }

        public string? Email { get; set; }

        public string? Password { get; set; }

        public string? Salt { get; set; }

        //waiting, banned, ok
        public string? Status { get; set; } = "waiting";
    }
}
