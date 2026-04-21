using GymPortal.Infrastructure.Identity;
using GymPortal.Web.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace GymPortal.Web.Controllers;

public class AccountController : Controller
{
    // Beroendeinjektion av UserManager och SignInManager.
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    // Konstruktor som tar emot UserManager och SignInManager via beroendeinjektion.
    public AccountController(
        // UserManager används för att hantera användare, t.ex. skapa nya användare, hämta användare, etc.
        UserManager<ApplicationUser> userManager,
        // SignInManager används för att hantera inloggning och utloggning av användare.
        SignInManager<ApplicationUser> signInManager)
    {
        // Tilldela de injicerade tjänsterna till privata fält så att de kan användas i controller-metoderna.
        _userManager = userManager;
        // Tilldela SignInManager till det privata fältet.
        _signInManager = signInManager;
    }

    // ======================== REGISTER ======================== //

    // GET-metod för att visa registreringsformuläret.
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    // POST-metod för att hantera registrering.
    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        // Validera modellen. Om den inte är giltig, visa formuläret igen med valideringsfel.
        if (!ModelState.IsValid)
            return View(model);

        // Skapa en ny ApplicationUser baserat på data från modellen.
        var user = new ApplicationUser
        {
            UserName = model.Email,
            Email = model.Email,
            FirstName = model.FirstName,
            LastName = model.LastName
        };

        // Försök att skapa användaren i databasen med det angivna lösenordet.
        var result = await _userManager.CreateAsync(user, model.Password);

        // Om skapandet lyckades, logga in användaren och omdirigera till startsidan.
        if (result.Succeeded)
        {
            // Logga in användaren utan att göra inloggningen ihållande (isPersistent: false).
            await _signInManager.SignInAsync(user, isPersistent: false);
            return RedirectToAction("Index", "Home");
        }

        // Om det uppstod fel under skapandet, lägg till felen i ModelState och visa formuläret igen.
        foreach (var error in result.Errors)
            ModelState.AddModelError("", error.Description);

        return View(model);
    }

    // ======================== LOGIN ======================== //

    // GET-metod för att visa registreringsformuläret.
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    // POST-metod för att hantera inloggning.
    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        // Validera modellen. Om den inte är giltig, visa formuläret igen med valideringsfel.
        if (!ModelState.IsValid)
            return View(model);
        
        // Försök att logga in användaren med angivna uppgifter.
        var result = await _signInManager.PasswordSignInAsync(
            model.Email,
            model.Password,
            model.RememberMe,
            lockoutOnFailure: false);

        // Om inloggningen lyckades, omdirigera till startsidan.
        if (result.Succeeded)
            return RedirectToAction("Index", "Home");

        // Om inloggningen misslyckades, lägg till ett generiskt felmeddelande i ModelState och visa formuläret igen.
        ModelState.AddModelError(string.Empty, "Felaktig e-post eller lösenord.");
        return View(model);
    }

    // ======================== LOGOUT ======================== //

    // POST-metod för att hantera utloggning.
    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        // Logga ut användaren och omdirigera till startsidan.
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }

    // ======================== PROFILE ======================== //

    [Authorize] // GET-metod för att visa användarens profil.
    [HttpGet] // Denna metod kräver att användaren är inloggad (Authorize).
    public async Task<IActionResult> Profile()
    {
        var user = await _userManager.GetUserAsync(User); // Hämta den aktuella användaren baserat på den inloggade användarens kontext.

        if (user == null)
            return Challenge();

        var model = new ProfileViewModel
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email ?? string.Empty,
            PhoneNumber = user.PhoneNumber
        };

        return View(model);
    }

    [Authorize] // POST-metod för att uppdatera användarens profil.
    [HttpPost] // Denna metod kräver att användaren är inloggad (Authorize).
    public async Task<IActionResult> Profile(ProfileViewModel model)
    {
        if (!ModelState.IsValid) // Validera modellen. Om den inte är giltig, visa formuläret igen med valideringsfel.
            return View(model);

        var user = await _userManager.GetUserAsync(User); // Hämta den aktuella användaren baserat på den inloggade användarens kontext.

        if (user == null) // Om användaren inte hittas, returnera en utmaning (Challenge) som kan leda till att användaren omdirigeras till inloggningssidan.
            return Challenge();

        // Uppdatera användarens information baserat på data från modellen.
        user.FirstName = model.FirstName;
        user.LastName = model.LastName;
        user.Email = model.Email;
        user.UserName = model.Email;
        user.PhoneNumber = model.PhoneNumber;

        var result = await _userManager.UpdateAsync(user); // Försök att uppdatera användarens information i databasen.

        if (result.Succeeded) // Om uppdateringen lyckades, visa ett framgångsmeddelande och visa formuläret igen.
        {
            // Uppdatera säkerhetsstämplar för att säkerställa att användarens session är uppdaterad med de nya uppgifterna.
            ViewBag.Message = "Dina uppgifter har uppdaterats.";
            return View(model);
        }

        foreach (var error in result.Errors) // Om det uppstod fel under uppdateringen, lägg till felen i ModelState och visa formuläret igen.
            ModelState.AddModelError(string.Empty, error.Description); // Lägg till varje fel i ModelState så att de kan visas i vyn.

        return View(model);
    }

    // ======================== DELETE ACCOUNT ======================== //

    [Authorize] 
    [HttpPost]
    [ValidateAntiForgeryToken] // POST-metod för att hantera borttagning av användarkonto.
    public async Task<IActionResult> DeleteAccount()
    {
        var user = await _userManager.GetUserAsync(User); // Hämta den aktuella användaren baserat på den inloggade användarens kontext.

        // Om användaren inte hittas, returnera en utmaning (Challenge) som kan leda till att användaren omdirigeras till inloggningssidan.
        if (user == null)
            return Challenge();

        await _signInManager.SignOutAsync(); // Logga ut användaren innan kontot tas bort för att säkerställa att sessionen avslutas.

        var result = await _userManager.DeleteAsync(user); // Försök att ta bort användaren från databasen.

        if (!result.Succeeded)
        {
            // Om det uppstod fel under borttagningen, visa ett felmeddelande och omdirigera tillbaka till profilvyn.
            TempData["ErrorMessage"] = "Det gick inte att ta bort kontot.";
            return RedirectToAction(nameof(Profile));
        }

        // Om borttagningen lyckades, visa ett framgångsmeddelande och omdirigera till startsidan.
        TempData["SuccessMessage"] = "Ditt konto har tagits bort.";
        return RedirectToAction("Index", "Home");
    }
}
