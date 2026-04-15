using GymPortal.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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
        ModelState.AddModelError("", "Felaktig inloggning");
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
}