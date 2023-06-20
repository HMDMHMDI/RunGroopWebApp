using System.ComponentModel.DataAnnotations;

namespace RunGroopWebApp.ViewModels;

public class LoginViewModel
{
    [Display (Name = "Email Address")]
    [Required(ErrorMessage = "Email Address is Needed.")]
    public string EmailAddress { get; set; }
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}