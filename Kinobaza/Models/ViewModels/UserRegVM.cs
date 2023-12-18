using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Kinobaza.Models.ViewModels
{
    public class UserRegVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Введите логин")]
        [StringLength(20, ErrorMessage = "Максимальная длина логина не больше 20 символов")]
        [Display(Name = "Логин")]
        public string? Login { get; set; }

        [Required(ErrorMessage = "Введите адрес электронной почты")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Некорректный адрес")]
        [StringLength(50, ErrorMessage = "Максимальная длина логина не больше 50 символов")]
        [Display(Name = "Адрес электронной почты")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Введите пароль")]
        [StringLength(50, ErrorMessage = "Максимальная длина пароля не больше 50 символов")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string? Password { get; set; }

        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        [Display(Name = "Подтвердите пароль")]
        public string? PasswordConfirm { get; set; }
    }
}
