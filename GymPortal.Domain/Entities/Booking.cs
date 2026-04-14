namespace GymPortal.Domain.Entities;

// Denna klass representerar en bokning av en gymklass av en användare och innehåller information om användar-ID, gymklass-ID och tidpunkten för bokningen.
public class Booking
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string UserId { get; private set; } = string.Empty;
    public Guid GymClassId { get; private set; }
    public DateTime BookedAt { get; private set; } = DateTime.UtcNow;

    private Booking() { }

    // Denna konstruktor används för att skapa en ny bokning med en specifik användar-ID och gymklass-ID.
    public Booking(string userId, Guid gymClassId)
    {
        UserId = userId;
        GymClassId = gymClassId;
    }
}