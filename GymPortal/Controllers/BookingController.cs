using GymPortal.Domain.Entities;
using GymPortal.Infrastructure.Data;
using GymPortal.Infrastructure.Identity;
using GymPortal.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GymPortal.Web.Controllers;

[Authorize]
public class BookingController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    // Konstruktor som tar emot ApplicationDbContext och UserManager<ApplicationUser> som beroenden och tilldelar dem till privata fält för att användas i controller-metoderna.
    public BookingController(
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }
    
    // Skapar en ny bokning för det angivna gympasset.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Guid gymClassId)
    {
        var user = await _userManager.GetUserAsync(User); // Hämta den inloggade användaren. Om användaren inte är inloggad, returnera en Challenge-resultat som kommer att omdirigera användaren till inloggningssidan.

        if (user == null)
            return Challenge();

        // Kontrollera om det angivna gympasset finns i databasen.
        var gymClassExists = await _context.GymClasses
            .AnyAsync(x => x.Id == gymClassId);

        // Kontrollera om det angivna gympasset finns i databasen.
        // Om det inte finns, sätt ett felmeddelande i TempData och omdirigera användaren tillbaka till klasser-sidan.
        var gymClass = await _context.GymClasses
            .FirstOrDefaultAsync(x => x.Id == gymClassId);

        // Kontrollera om det angivna gympasset finns i databasen.
        // Om det inte finns, sätt ett felmeddelande i TempData och omdirigera användaren tillbaka till klasser-sidan.
        if (gymClass == null)
        {
            TempData["ErrorMessage"] = "Passet kunde inte hittas.";
            return RedirectToAction("Index", "Classes");
        }

        // Kontrollera om det angivna gympasset har nått sin kapacitet genom att räkna antalet befintliga bokningar för det passet.
        // Om kapaciteten är uppnådd, sätt ett felmeddelande i TempData och omdirigera användaren tillbaka till klasser-sidan.
        var currentBookings = await _context.Bookings
            .CountAsync(x => x.GymClassId == gymClassId);

        // Kontrollera om det angivna gympasset har nått sin kapacitet genom att räkna antalet befintliga bokningar för det passet.
        // Om kapaciteten är uppnådd, sätt ett felmeddelande i TempData och omdirigera användaren tillbaka till klasser-sidan.
        if (currentBookings >= gymClass.Capacity)
        {
            TempData["ErrorMessage"] = "Passet är fullt.";
            return RedirectToAction("Index", "Classes");
        }

        // Kontrollera om användaren redan har bokat det angivna gympasset.
        // Om så är fallet, sätt ett felmeddelande i TempData och omdirigera användaren tillbaka till klasser-sidan.
        var alreadyBooked = await _context.Bookings
            .AnyAsync(x => x.UserId == user.Id && x.GymClassId == gymClassId);

        // Kontrollera om användaren redan har bokat det angivna gympasset.
        if (alreadyBooked)
        {
            TempData["ErrorMessage"] = "Du har redan bokat detta pass.";
            return RedirectToAction("Index", "Classes");
        }

        var booking = new Booking(user.Id, gymClassId);

        // Skapa en ny bokning genom att instansiera en Booking-objekt med användarens ID och det angivna gympassets ID. Lägg sedan till bokningen i databasen och spara ändringarna.
        _context.Bookings.Add(booking);
        await _context.SaveChangesAsync();

        // Sätt ett framgångsmeddelande i TempData och omdirigera användaren till "MyBookings
        TempData["SuccessMessage"] = "Passet har bokats.";
        return RedirectToAction("MyBookings");
    }

    // Hämtar och visar en lista över användarens bokningar.
    [HttpGet]
    public async Task<IActionResult> MyBookings()
    {
        var user = await _userManager.GetUserAsync(User);

        if (user == null)
            return Challenge();

        // Hämta alla bokningar för den inloggade användaren och inkludera information om det bokade gympasset genom att använda en Join mellan Bookings och GymClasses.
        // Skapa en lista av BookingViewModel-objekt som innehåller relevant information om varje bokning och sortera dem efter starttid.
        var bookings = await _context.Bookings
            .Where(x => x.UserId == user.Id)
            .Join(
                _context.GymClasses,
                booking => booking.GymClassId,
                gymClass => gymClass.Id,
                (booking, gymClass) => new BookingViewModel
                {
                    BookingId = booking.Id,
                    GymClassId = gymClass.Id,
                    ClassName = gymClass.Name,
                    Category = gymClass.Category,
                    Instructor = gymClass.Instructor,
                    StartTime = gymClass.StartTime,
                    BookedAt = booking.BookedAt
                })
            // Sortera bokningarna efter starttid och konvertera resultatet till en lista asynkront.
            .OrderBy(x => x.StartTime)
            .ToListAsync();

        return View(bookings);
    }

    // Avbokar en befintlig bokning baserat på det angivna boknings-ID:t.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Cancel(Guid bookingId)
    {
        var user = await _userManager.GetUserAsync(User); // Hämta den inloggade användaren. Om användaren inte är inloggad, returnera en Challenge-resultat som kommer att omdirigera användaren till inloggningssidan.

        if (user == null)
            return Challenge();

        var booking = await _context.Bookings
            .FirstOrDefaultAsync(x => x.Id == bookingId && x.UserId == user.Id);

        // Hämta bokningen som matchar det angivna boknings-ID:t och tillhör den inloggade användaren.
        // Om bokningen inte hittas, sätt ett felmeddelande i TempData och omdirigera användaren tillbaka till "MyBookings"-sidan.
        if (booking == null)
        {
            // Om bokningen inte hittas, sätt ett felmeddelande i TempData och omdirigera användaren tillbaka till "MyBookings"-sidan.
            TempData["ErrorMessage"] = "Bokningen kunde inte hittas.";
            return RedirectToAction(nameof(MyBookings));
        }

        // Ta bort bokningen från databasen och spara ändringarna. Sätt sedan ett framgångsmeddelande i TempData och omdirigera användaren tillbaka till "MyBookings"-sidan.
        _context.Bookings.Remove(booking);
        await _context.SaveChangesAsync();

        // Ta bort bokningen från databasen och spara ändringarna. Sätt sedan ett framgångsmeddelande i TempData och omdirigera användaren tillbaka till "MyBookings"-sidan.
        TempData["SuccessMessage"] = "Bokningen har avbokats.";
        return RedirectToAction(nameof(MyBookings));
    }
}