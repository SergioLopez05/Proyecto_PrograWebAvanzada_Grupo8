using Microsoft.AspNetCore.Mvc;
using EventosCR.Web.Services;
using EventosCR.Web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using EventosCR.Web.Helpers;

public class AccountController : Controller
{
    private readonly ApiAuthService _authService;

    public AccountController(ApiAuthService authService)
    {
        _authService = authService;
    }

    // GET: /Account/Login
    public IActionResult Login()
    {
        return View();
    }

    // POST: /Account/Login
    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var loginResult = await _authService.LoginAsync(model);
        if (loginResult == null)
        {
            ModelState.AddModelError("", "Credenciales incorrectas.");
            return View(model);
        }

        var claims = new List<Claim>();

        var name = loginResult.UserName ?? model.Email;
        if (!string.IsNullOrEmpty(name))
            claims.Add(new Claim(ClaimTypes.Name, name));

        if (!string.IsNullOrEmpty(loginResult.UserId))
            claims.Add(new Claim(ClaimTypes.NameIdentifier, loginResult.UserId));

        if (!string.IsNullOrEmpty(loginResult.Role))
            claims.Add(new Claim(ClaimTypes.Role, loginResult.Role));

        claims.Add(new Claim("AccessToken", loginResult.Token));

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(claimsIdentity);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);


        TokenHandler.SaveToken(HttpContext, loginResult.Token);

        return RedirectToAction("Index", "Eventos");
    }

    // GET: /Account/Register
    public IActionResult Register()
    {
        return View();
    }

    // POST: /Account/Register
    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var success = await _authService.RegisterAsync(model);
        if (!success)
        {
            ModelState.AddModelError("", "No se pudo registrar el usuario.");
            return View(model);
        }

        return RedirectToAction("Login");
    }

    // GET: /Account/Logout
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login");
    }
}
