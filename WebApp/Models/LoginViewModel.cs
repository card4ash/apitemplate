using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace WebApp.Models;

public class LoginViewModel
{
    [Required(ErrorMessage = "User name is required")]
    [Display(Name = "User name")]
    public string UserName { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string Password { get; set; }
}
