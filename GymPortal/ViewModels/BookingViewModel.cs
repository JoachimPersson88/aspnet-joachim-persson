namespace GymPortal.Web.ViewModels;

public class BookingViewModel
{
    public Guid BookingId { get; set; }
    public Guid GymClassId { get; set; }
    public string ClassName { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string? Instructor { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime BookedAt { get; set; }
}