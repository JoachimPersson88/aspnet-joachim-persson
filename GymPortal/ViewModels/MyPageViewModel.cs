namespace GymPortal.Web.ViewModels;

public class MyPageViewModel
{
    public ProfileViewModel Profile { get; set; } = new();
    public MembershipViewModel? Membership { get; set; }
    public List<BookingViewModel> Bookings { get; set; } = new();
}