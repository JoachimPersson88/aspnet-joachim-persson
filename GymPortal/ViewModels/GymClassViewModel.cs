namespace GymPortal.Web.ViewModels;

public class GymClassViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string? Instructor { get; set; }
    public DateTime StartTime { get; set; }
    public int Capacity { get; set; }
}