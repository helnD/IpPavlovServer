using System.ComponentModel.DataAnnotations;

namespace WebApplication.ViewModels.Login;

/// <summary>
/// Login view model.
/// </summary>
public class LoginViewModel
{
    /// <summary>
    /// Login.
    /// </summary>
    [Required(ErrorMessage = "Логин обязателен к заполнению.")]
    public string Login { get; set; }

    /// <summary>
    /// Password
    /// </summary>
    [Required(ErrorMessage = "Пароль обязателен к заполнению.")]
    public string Password { get; set; }
}