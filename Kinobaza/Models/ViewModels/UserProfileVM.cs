using System.ComponentModel.DataAnnotations;

namespace Kinobaza.Models.ViewModels
{
    public class UserProfileVM
    {
        [Required(ErrorMessage = "Введите логин")]
        [StringLength(20, ErrorMessage = "Максимальная длина логина не больше 20 символов")]
        [Display(Name = "Логин")]
        public string? Login { get; set; }

        [Required(ErrorMessage = "Введите пароль")]
        [StringLength(50, ErrorMessage = "Максимальная длина пароля не больше 50 символов")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Введите новый пароль")]
        [StringLength(50, ErrorMessage = "Максимальная длина пароля не больше 50 символов")]
        [DataType(DataType.Password)]
        [Display(Name = "Новый пароль")]
        public string? NewPassword { get; set; }

        [Compare("NewPassword", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        [Display(Name = "Подтвердите новый пароль")]
        public string? NewPasswordConfirm { get; set; }
    }
}
