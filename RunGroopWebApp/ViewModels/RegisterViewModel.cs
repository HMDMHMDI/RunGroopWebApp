using System.ComponentModel.DataAnnotations;

namespace RunGroopWebApp.ViewModels;

public class RegisterViewModel
{
    [Display(Name = "Email Address")]
    [Required(ErrorMessage = "Email Address is Required")]
    public string EmailAddress { get; set; }
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    [Display(Name = "Confirm Password")]
    [Required(ErrorMessage = "Confirm Password is Required")]
    [Compare("Password" , ErrorMessage = "Password do not match.")]
    [DataType(DataType.Password)]
    public string ConfirmPassword { get; set; }

}