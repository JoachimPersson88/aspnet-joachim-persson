using GymPortal.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace GymPortal.Web.ViewModels;

public class MembershipViewModel
{
    [Required]
    [Display(Name = "Medlemskapstyp")]
    public MembershipType Type { get; set; }

    public MembershipStatus? Status { get; set; }
    public DateTime? StartDate { get; set; }
}