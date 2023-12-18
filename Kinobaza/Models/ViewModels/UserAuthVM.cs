using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Kinobaza.Models.ViewModels
{
    public class UserAuthVM
    {
        [Required(ErrorMessage = "Введите логин")]
        [StringLength(20, ErrorMessage = "Максимальная длина логина не больше 20 символов")]
        [Display(Name = "Логин")]
        public string? Login { get; set; }

        [Required(ErrorMessage = "Введите пароль")]
        [DataType(DataType.Password)]
        [StringLength(50, ErrorMessage = "Максимальная длина пароля не больше 50 символов")]
        [Display(Name = "Пароль")]
        public string? Password { get; set; }
    }
}
