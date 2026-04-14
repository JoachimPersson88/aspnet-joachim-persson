using GymPortal.Infrastructure.Data;
using GymPortal.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

// Denna fil innehåller konfigurationen och startlogiken för ASP.NET Core-applikationen.
// Den skapar en webapplikation, konfigurerar tjänster som används i applikationen, och definierar hur HTTP-förfrågningar ska hanteras.
var builder = WebApplication.CreateBuilder(args);

// Lägg till tjänster i behållaren.
builder.Services.AddControllersWithViews();
// Genom att använda AddDbContext-metoden kan vi registrera ApplicationDbContext som en tjänst i applikationens beroendeinjektionscontainer,
// vilket gör det möjligt att använda den i hela applikationen för att interagera med databasen.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Genom att använda AddIdentity-metoden kan vi konfigurera ASP.NET Core Identity-tjänsterna, inklusive användarhantering, lösenordspolicyer och tokenhantering.
// Vi specificerar också att vi vill använda ApplicationUser som vår användarentitet och IdentityRole som vår rollentitet.
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    // Genom att ställa in RequireConfirmedAccount till false kan vi tillåta användare att logga in utan att behöva bekräfta sina konton via e-post eller andra metoder.
    options.SignIn.RequireConfirmedAccount = false;

    // Genom att konfigurera lösenordspolicyerna kan vi säkerställa att användarnas lösenord uppfyller vissa krav,
    // såsom att innehålla siffror, gemener, versaler, specialtecken och ha en viss längd.
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 8;
})

// Genom att använda AddEntityFrameworkStores-metoden kan vi specificera att vi vill använda Entity Framework Core för att lagra användar- och rollinformation i databasen,
// och vi anger ApplicationDbContext som vår databascontext.
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

var app = builder.Build();

// Konfigurera HTTP-förfrågningspipen.
if (!app.Environment.IsDevelopment())
{
    // Genom att använda UseExceptionHandler-metoden kan vi definiera en global felhanterare som fångar upp och hanterar undantag som inträffar i applikationen,
    // och i det här fallet omdirigerar användaren till en felhanteringssida som ligger på "/Home/Error".
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// Genom att använda UseHttpsRedirection-metoden kan vi tvinga alla HTTP-förfrågningar att omdirigeras till HTTPS,
// vilket förbättrar säkerheten för applikationen genom att kryptera data som överförs mellan klienten och servern.
app.UseHttpsRedirection();
app.UseStaticFiles();

// Genom att använda UseRouting-metoden kan vi aktivera routning i applikationen,
// vilket gör det möjligt att definiera hur inkommande HTTP-förfrågningar ska matchas mot kontroller och åtgärder i applikationen.
app.UseRouting();

// Genom att använda UseAuthentication-metoden kan vi aktivera autentisering i applikationen,
// vilket gör det möjligt för användare att logga in och få tillgång till skyddade resurser baserat på deras identitet och roller.
app.UseAuthentication();
app.UseAuthorization();

// Genom att använda MapControllerRoute-metoden kan vi definiera en standardrutt för våra MVC-kontroller,
// vilket gör det möjligt att hantera HTTP-förfrågningar som inte matchar några specifika rutter och dirigera dem till en standardkontroller och åtgärd,
// i det här fallet HomeController och Index-åtgärden.
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();