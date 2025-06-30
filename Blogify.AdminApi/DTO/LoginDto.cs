using System.ComponentModel.DataAnnotations;

namespace Blogify.AdminApi.DTO;

public class LoginDto
{
    [Required(ErrorMessage = "請輸入帳號")]
    [Display(Name = "帳號")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "請輸入密碼")]
    [Display(Name = "密碼")]
    public string Password { get; set; } = string.Empty;
}
