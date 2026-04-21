using GymPortal.Domain.Entities;
using GymPortal.Domain.Enums;
using GymPortal.Infrastructure.Data;
using GymPortal.Infrastructure.Identity;
using GymPortal.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GymPortal.Web.Controllers;

[Authorize] // Endast inloggade användare kan hantera medlemskap
public class MembershipController : Controller
{
    private readonly ApplicationDbContext _context; // För att interagera med databasen och hantera medlemskap
    private readonly UserManager<ApplicationUser> _userManager; // För att hantera användarrelaterade operationer, som att hämta den aktuella användaren

    // Konstruktor som tar emot ApplicationDbContext och UserManager via dependency injection
    public MembershipController(
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [HttpGet] // Visa medlemskapssidan där användaren kan se sitt nuvarande medlemskap eller skapa ett nytt
    // Om användaren inte har ett medlemskap visas ett formulär för att skapa ett nytt medlemskap
    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User); // Hämta den aktuella inloggade användaren

        // Om användaren inte är inloggad, utmana dem att logga in
        if (user == null)
            return Challenge();

        // Hämta användarens medlemskap från databasen
        var membership = await _context.Memberships // Hämta medlemskap från databasen
            .FirstOrDefaultAsync(x => x.UserId == user.Id); // Försök hitta ett medlemskap som matchar den aktuella användarens ID

        // Om användaren inte har något medlemskap, visa en tom vy där de kan skapa ett nytt medlemskap
        if (membership == null)
        {
            return View(new MembershipViewModel());
        }

        // Om användaren har ett medlemskap, skapa en MembershipViewModel baserat på medlemskapets data och visa det i vyn
        var model = new MembershipViewModel
        {
            Type = membership.Type,
            Status = membership.Status,
            StartDate = membership.StartDate
        };

        return View(model);
    }

    [HttpPost] // Hantera formuläret för att skapa ett nytt medlemskap
    [ValidateAntiForgeryToken] // Skydda mot CSRF-attacker genom att validera en anti-forgery token

    // När användaren skickar in formuläret för att skapa ett nytt medlemskap, validera modellen och skapa ett nytt medlemskap i databasen
    public async Task<IActionResult> Create(MembershipViewModel model)
    {
        // Validera modellen. Om den inte är giltig, visa formuläret igen med valideringsfel
        if (!ModelState.IsValid)
            return View("Index", model);

        var user = await _userManager.GetUserAsync(User); // Hämta den aktuella inloggade användaren

        //  Om användaren inte är inloggad, utmana dem att logga in
        if (user == null)
            return Challenge();

        // Kontrollera om användaren redan har ett medlemskap. Om de har det, visa ett felmeddelande och omdirigera tillbaka till indexsidan
        var existingMembership = await _context.Memberships
            .FirstOrDefaultAsync(x => x.UserId == user.Id); // Försök hitta ett befintligt medlemskap som matchar den aktuella användarens ID

        //  Om användaren redan har ett medlemskap, visa ett felmeddelande och omdirigera tillbaka till indexsidan
        if (existingMembership != null)
        {
            TempData["ErrorMessage"] = "Du har redan ett medlemskap.";
            return RedirectToAction(nameof(Index)); // Omdirigera tillbaka till indexsidan
        }

        var membership = new Membership(user.Id, model.Type); // Skapa ett nytt medlemskap baserat på den aktuella användarens ID och den valda medlemskapstypen från modellen

        _context.Memberships.Add(membership); // Lägg till det nya medlemskapet i databaskontexten
        await _context.SaveChangesAsync(); // Spara ändringarna i databasen

        TempData["SuccessMessage"] = "Ditt medlemskap har skapats."; // Sätt ett framgångsmeddelande i TempData som kan visas på nästa sida
        return RedirectToAction(nameof(Index)); // Omdirigera tillbaka till indexsidan där användaren kan se sitt nya medlemskap
    }
}