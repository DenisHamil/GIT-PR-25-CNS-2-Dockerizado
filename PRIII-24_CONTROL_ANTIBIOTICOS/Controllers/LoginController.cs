using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PRIII_24_CONTROL_ANTIBIOTICOS.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using PRIII_24_CONTROL_ANTIBIOTICOS.Services.recursos;

namespace PRIII_24_CONTROL_ANTIBIOTICOS.Controllers
{
    public class LoginController : Controller
    {
        private readonly BdProa1Context _context;
        private readonly IConfiguration _configuration;
        readonly IUserService _userService;

        public LoginController(BdProa1Context context, IConfiguration configuration, IUserService userService)
        {
            _context = context;
            _configuration = configuration;
            _userService = userService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string usernameOrEmail, string password)
        {
            if (string.IsNullOrEmpty(usernameOrEmail) || string.IsNullOrEmpty(password))
            {
                return View();
            }

            string encryptedPassword = Utilidades.EncriptarClave(password);

            var user = await _context.Users
                .FirstOrDefaultAsync(u =>
                    (u.Username == usernameOrEmail || u.Email == usernameOrEmail) &&
                    u.Password == encryptedPassword &&
                    u.IsActive == true);

            if (user == null)
            {
                ModelState.AddModelError("", "Usuario inválido, desactivado o contraseña incorrecta.");
                return View();
            }

            var person = await _context.People.FirstOrDefaultAsync(x => x.IdPerson == user.IdUser);

            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, user.Username),
        new Claim(ClaimTypes.NameIdentifier, user.IdUser.ToString()),
        new Claim(ClaimTypes.GivenName, $"{person?.Name} {person?.LastName}")
    };

            if (!string.IsNullOrWhiteSpace(user.Role))
            {
                claims.Add(new Claim(ClaimTypes.Role, user.Role.ToUpper()));
            }

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            ClaimsPrincipal principal = new ClaimsPrincipal(claimsIdentity);

            var authProperties = new AuthenticationProperties
            {
                AllowRefresh = true,
                IsPersistent = true
            };

            await HttpContext.SignInAsync(principal, authProperties);

            return RedirectToAction("Index", "Home");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login", "Login");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
