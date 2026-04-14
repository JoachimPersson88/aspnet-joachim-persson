using GymPortal.Domain.Enums;

// Denna klass representerar ett medlemskap i gymmet och innehåller information om användarens medlemskapstyp, status, start- och slutdatum.
// Den har också metoder för att skapa ett nytt medlemskap och avbryta ett befintligt medlemskap.
namespace GymPortal.Domain.Entities;

// Genom att använda privata set-metoder och en privat parameterlös konstruktor kan vi säkerställa att medlemskap
// endast kan skapas och ändras genom de offentliga metoderna, vilket hjälper till att upprätthålla inkapsling och dataintegritet.
public class Membership
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string UserId { get; private set; } = string.Empty;
    public MembershipType Type { get; private set; }
    public MembershipStatus Status { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime? EndDate { get; private set; }

    // Den privata parameterlösa konstruktorn används av Entity Framework för att skapa instanser av Membership när den hämtar data från databasen.
    private Membership() { }

    // Denna konstruktor används för att skapa ett nytt medlemskap med en specifik användar-ID och medlemskapstyp.
    // När ett nytt medlemskap skapas, sätts statusen till Active och startdatumet till det aktuella datumet.
    public Membership(string userId, MembershipType type)
    {
        UserId = userId;
        Type = type;
        Status = MembershipStatus.Active;
        StartDate = DateTime.UtcNow;
    }

    // Denna metod används för att avbryta ett befintligt medlemskap.
    // När ett medlemskap avbryts, sätts statusen till Cancelled och slutdatumet till det aktuella datumet.
    public void Cancel()
    {
        Status = MembershipStatus.Cancelled;
        EndDate = DateTime.UtcNow;
    }
}