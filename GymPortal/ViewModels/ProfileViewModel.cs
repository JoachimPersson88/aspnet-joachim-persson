using System.ComponentModel.DataAnnotations;

namespace GymPortal.Web.ViewModels;

public class ProfileViewModel
{
    [Required]
    [Display(Name = "Förnamn")]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Efternamn")]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [Display(Name = "E-post")]
    public string Email { get; set; } = string.Empty;

    [Phone]
    [Display(Name = "Telefonnummer")]
    public string? PhoneNumber { get; set; }
}