namespace GymPortal.Domain.Entities;

// Denna klass representerar en gymklass som erbjuds av gymmet och innehåller information om klassens namn, kategori, instruktör, starttid och kapacitet.
public class GymClass
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Name { get; private set; } = string.Empty;
    public string Category { get; private set; } = string.Empty;
    public string? Instructor { get; private set; }
    public DateTime StartTime { get; private set; }
    public int Capacity { get; private set; }

    // Den privata parameterlösa konstruktorn används av Entity Framework för att skapa instanser av GymClass när den hämtar data från databasen.
    private GymClass() { }

    // Denna konstruktor används för att skapa en ny gymklass med specifika egenskaper som namn, kategori, starttid, kapacitet och valfri instruktör.
    public GymClass(string name, string category, DateTime startTime, int capacity, string? instructor = null)
    {
        Name = name;
        Category = category;
        StartTime = startTime;
        Capacity = capacity;
        Instructor = instructor;
    }
}