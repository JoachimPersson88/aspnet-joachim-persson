using Microsoft.AspNetCore.Identity;

// Den här klassen representerar applikationsanvändaren och utökar IdentityUser-klassen som tillhandahålls av ASP.NET Core Identity.
namespace GymPortal.Infrastructure.Identity;

// Genom att utöka IdentityUser kan vi lägga till ytterligare egenskaper som är specifika för vår applikation, såsom FirstName, LastName och ProfileImageUrl.
public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? ProfileImageUrl { get; set; }
}