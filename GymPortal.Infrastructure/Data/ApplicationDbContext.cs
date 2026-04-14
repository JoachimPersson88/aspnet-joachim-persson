using GymPortal.Domain.Entities;
using GymPortal.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GymPortal.Infrastructure.Data;
// Denna klass representerar databasens kontext och är ansvarig för att hantera anslutningen till databasen och definiera DbSet-egenskaper för varje entitet i applikationen.
public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    // Genom att definiera DbSet-egenskaper för varje entitet kan vi enkelt utföra CRUD-operationer på dessa entiteter genom Entity Framework Core.
    public DbSet<Membership> Memberships => Set<Membership>();
    public DbSet<GymClass> GymClasses => Set<GymClass>();
    public DbSet<Booking> Bookings => Set<Booking>();

    // Konstruktor som tar emot DbContextOptions och skickar dem vidare till bas-klassen IdentityDbContext.
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // Denna metod används för att konfigurera modellens schema och relationer när databasen skapas eller uppdateras.
    protected override void OnModelCreating(ModelBuilder builder)
    {
        // Genom att anropa base.OnModelCreating(builder) säkerställer vi att IdentityDbContext's konfiguration också körs, vilket är viktigt för att korrekt konfigurera Identity-relaterade tabeller och relationer.
        base.OnModelCreating(builder);

        // Här konfigurerar vi varje entitet i databasen, inklusive primära nycklar, egenskaper och index.
        builder.Entity<Membership>(entity =>
        {
            // Genom att använda HasKey-metoden definierar vi Id-egenskapen som den primära nyckeln för Membership-entiteten.
            entity.HasKey(x => x.Id);

            // Genom att använda Property-metoden kan vi konfigurera egenskaperna för varje kolumn i databasen, såsom att göra UserId obligatoriskt och konvertera enum-typer till int.
            entity.Property(x => x.UserId)
                .IsRequired();

            // Genom att använda HasConversion<int>() konverterar vi enum-typerna MembershipType och MembershipStatus till int i databasen,
            entity.Property(x => x.Type)
                .HasConversion<int>();

            // Genom att använda HasConversion<int>() konverterar vi enum-typerna MembershipType och MembershipStatus till int i databasen,
            entity.Property(x => x.Status)
                .HasConversion<int>();

            // Genom att använda HasIndex-metoden kan vi skapa ett unikt index på UserId-kolumnen, vilket säkerställer att varje användare endast kan ha ett medlemskap i databasen.
            entity.HasIndex(x => x.UserId)
                .IsUnique();
        });

        // Här konfigurerar vi GymClass-entiteten, inklusive primära nycklar, egenskaper och index.
        builder.Entity<GymClass>(entity =>
        {
            // Genom att använda HasKey-metoden definierar vi Id-egenskapen som den primära nyckeln för GymClass-entiteten.
            entity.HasKey(x => x.Id);

            // Genom att använda Property-metoden kan vi konfigurera egenskaperna för varje kolumn i databasen, såsom att göra Name obligatoriskt och sätta en maximal längd.
            entity.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100);

            // Genom att använda Property-metoden kan vi konfigurera egenskaperna för varje kolumn i databasen, såsom att göra Category obligatoriskt och sätta en maximal längd.
            entity.Property(x => x.Category)
                .IsRequired()
                .HasMaxLength(100);

            // Genom att använda Property-metoden kan vi konfigurera egenskaperna för varje kolumn i databasen, såsom att sätta en maximal längd för Instructor-kolumnen, även om den är valfri.
            entity.Property(x => x.Instructor)
                .HasMaxLength(100);

            // Genom att använda Property-metoden kan vi konfigurera egenskaperna för varje kolumn i databasen, såsom att göra StartTime obligatoriskt.
            entity.Property(x => x.Capacity)
                .IsRequired();
        });

        // Här konfigurerar vi Booking-entiteten, inklusive primära nycklar, egenskaper och index.
        builder.Entity<Booking>(entity =>
        {
            // Genom att använda HasKey-metoden definierar vi Id-egenskapen som den primära nyckeln för Booking-entiteten.
            entity.HasKey(x => x.Id);

            // Genom att använda Property-metoden kan vi konfigurera egenskaperna för varje kolumn i databasen, såsom att göra UserId och GymClassId obligatoriska.
            entity.Property(x => x.UserId)
                .IsRequired();

            // Genom att använda Property-metoden kan vi konfigurera egenskaperna för varje kolumn i databasen, såsom att göra UserId och GymClassId obligatoriska.
            entity.Property(x => x.GymClassId)
                .IsRequired();

            // Genom att använda HasIndex-metoden kan vi skapa ett unikt index på kombinationen av UserId och GymClassId, vilket säkerställer att en användare endast kan boka en specifik gymklass en gång.
            entity.HasIndex(x => new { x.UserId, x.GymClassId })
                .IsUnique();
        });
    }
}