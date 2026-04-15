using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.Json;
using Zajezdnia.Data;
using Zajezdnia.Models;
using Zajezdnia.Services;

namespace Zajezdnia.Controllers;

public class AccountController(AppDbContext db, PasswordService pwdService) : Controller
{
    private const string CookieName = "ZajezdniaAuth";

    public IActionResult Rejestruj() => View();


    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Rejestruj(RejestrujViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        if (await db.Uzytkownicy.AnyAsync(u => u.Login == model.Login))
        {
            ModelState.AddModelError("Login", "Ten login jest już zajęty.");
            return View(model);
        }
        if (await db.Uzytkownicy.AnyAsync(u => u.Email == model.Email))
        {
            ModelState.AddModelError("Email", "Ten adres email jest już zarejestrowany.");
            return View(model);
        }

        var user = new Uzytkownik
        {
            Login           = model.Login,
            HasloHash       = pwdService.HashHasla(model.Haslo),
            Imie            = model.Imie,
            Nazwisko        = model.Nazwisko,
            Email           = model.Email,
            DataRejestracji = DateTime.UtcNow
        };

        db.Uzytkownicy.Add(user);
        await db.SaveChangesAsync();

        UstawCookie(user, false);
        TempData["Sukces"] = $"Witaj, {user.Imie}! Konto zostało utworzone.";
        return RedirectToAction("Index", "Zajezdnie");
    }

    public IActionResult Zaloguj(string? returnUrl = null)
    {
        ViewBag.ReturnUrl = returnUrl;
        return View();
    }
    
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Zaloguj(ZalogujViewModel model, string? returnUrl = null)
    {
        ViewBag.ReturnUrl = returnUrl;
        if (!ModelState.IsValid) return View(model);

        var user = await db.Uzytkownicy.FirstOrDefaultAsync(u => u.Login == model.Login);

        if (user == null || !pwdService.WeryfikujHaslo(model.Haslo, user.HasloHash))
        {
            ModelState.AddModelError("", "Nieprawidłowy login lub hasło.");
            return View(model);
        }

        UstawCookie(user, model.ZapamiętajMnie);

        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            return Redirect(returnUrl);

        TempData["Sukces"] = $"Zalogowano jako {user.Imie} {user.Nazwisko}.";
        return RedirectToAction("Index", "Zajezdnie");
    }


    [HttpPost, ValidateAntiForgeryToken]
    public IActionResult Wyloguj()
    {
        Response.Cookies.Delete(CookieName);
        TempData["Sukces"] = "Zostałeś pomyślnie wylogowany.";
        return RedirectToAction("Index", "Zajezdnie");
    }

    public IActionResult BrakDostepu() => View();



    private void UstawCookie(Uzytkownik u, bool zapamiętaj)
    {
        var payload = new { u.Id, u.Login, u.Imie, u.Nazwisko };
        var encoded = Convert.ToBase64String(
            Encoding.UTF8.GetBytes(JsonSerializer.Serialize(payload)));

        Response.Cookies.Append(CookieName, encoded, new CookieOptions
        {
            HttpOnly = true,
            Secure   = false,   
            SameSite = SameSiteMode.Strict,
            Expires  = zapamiętaj ? DateTimeOffset.UtcNow.AddDays(14) : null
        });
    }
}