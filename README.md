# GymPortal

GymPortal är en webbapplikation byggd med ASP.NET Core MVC där användare kan registrera sig, hantera medlemskap och boka träningspass.

## Funktionalitet

- Registrering och inloggning (ASP.NET Identity)
- Profilhantering
- Skapa och avsluta medlemskap
- Visa träningspass
- Boka och avboka pass
- Begränsning av antal platser (capacity)
- "Min sida" med översikt över profil, medlemskap och bokningar

## Tekniker

- ASP.NET Core MVC
- Entity Framework Core
- SQL Server (LocalDB)
- ASP.NET Identity
- Clean Architecture (Domain, Application, Infrastructure, Web)

## Tester

Projektet innehåller enhetstester för domänlogik:

- Membership
- Booking
- GymClass

Tester är skrivna med xUnit.

## Hur man kör projektet

1. Klona projektet
2. Öppna i Visual Studio
3. Kör:
