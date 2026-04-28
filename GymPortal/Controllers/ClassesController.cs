using GymPortal.Infrastructure.Data;
using GymPortal.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GymPortal.Web.Controllers;

public class ClassesController : Controller
{
    private readonly ApplicationDbContext _context; // Denna fält används för att interagera med databasen och hämta information om gymklasser.

    // Konstruktor som tar emot en instans av ApplicationDbContext och tilldelar den till det privata fältet _context.
    // Detta möjliggör för klassen att använda databaskontexten för att hämta och manipulera data relaterad till gymklasser.
    public ClassesController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var classes = await _context.GymClasses // Hämtar alla gymklasser från databasen.
            .OrderBy(x => x.StartTime) // Sorterar klasserna i stigande ordning baserat på deras starttid, så att de som börjar tidigare visas först.
            .Select(x => new GymClassViewModel // Skapar en ny instans av GymClassViewModel för varje gymklass som hämtas från databasen.
            {
                Id = x.Id,
                Name = x.Name,
                Category = x.Category,
                Instructor = x.Instructor,
                StartTime = x.StartTime,
                Capacity = x.Capacity,
                BookedCount = _context.Bookings.Count(b => b.GymClassId == x.Id)
            })
            .ToListAsync(); // Konverterar resultatet till en lista asynkront, vilket gör att metoden inte blockerar tråden medan den väntar på att databasoperationen ska slutföras.

        return View(classes);
    }
}