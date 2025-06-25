using Microsoft.AspNetCore.Mvc;
using PRIII_24_CONTROL_ANTIBIOTICOS.Models;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Security.Cryptography;
using PRIII_24_CONTROL_ANTIBIOTICOS.Services.recursos;
using Microsoft.AspNetCore.Authorization;

namespace PRIII_24_CONTROL_ANTIBIOTICOS.Controllers;

[Authorize]
public class RegistroPersonalController : Controller
{
    private readonly BdProa1Context _context;
    private readonly IEmailSender _emailService;

    public RegistroPersonalController(BdProa1Context context, IEmailSender emailService)
    {
        _context = context;
        _emailService = emailService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var users = await _context.Users.Include(u => u.IdPersonNavigation).ToListAsync();
        return View(users);
    }

    [HttpGet]
    public async Task<IActionResult> RegistroPersonal()
    {
        ViewBag.Specialties = await _context.MedicalSpecialties.ToListAsync();
        return View(new Person());
    }

    #region Encriptación y generación, envío de correo

    public static string EncriptarClave(string password)
    {
        StringBuilder sb = new StringBuilder();

        using (SHA256 hash = SHA256Managed.Create())
        {
            Encoding enc = Encoding.UTF8;
            byte[] result = hash.ComputeHash(enc.GetBytes(password));

            foreach (byte b in result)
            {
                sb.Append(b.ToString("x2"));
            }

            return sb.ToString();
        }
    }

    private string GeneratePassword(string name, string ci)
    {
        return $"{name.ToLower().Substring(0, Math.Min(name.Length, 3))}{ci}";
    }

    private string GenerateUsername(string name, string lastName)
    {
        return $"{name.ToLower().Substring(0, Math.Min(name.Length, 3))}{lastName}";
    }

    #endregion

    [HttpPost]
    public async Task<IActionResult> RegistroPersonal(Person person, User user, string email,
    string Gender, int IdSpecialty, string role)
    {
        try
        {
            var newPerson = new Person
            {
                Name = person.Name,
                LastName = person.LastName,
                SecondLastName = person.SecondLastName,
                Ci = person.Ci,
                DateOfBirth = person.DateOfBirth,
                Gender = Gender
            };

            _context.People.Add(newPerson);
            await _context.SaveChangesAsync();

            string usernameG = GenerateUsername(person.Name, person.LastName);
            while (await _context.Users.AnyAsync(u => u.Username == usernameG))
            {
                usernameG = GenerateUsername(person.Name, person.LastName + Guid.NewGuid().ToString("N").Substring(0, 4));
            }

            string password = GeneratePassword(person.Name, person.Ci);
            string emailBody = $"Tu nombre de usuario es: {usernameG}<br>Tu contraseña es: {password}";

            await _emailService.SendEmailAsync(email, "Tus credenciales de acceso", emailBody);

            var newUser = new User
            {
                IdUser = newPerson.IdPerson,
                Email = email,
                Username = usernameG,
                Role = role,
                Password = EncriptarClave(password),
                IdSpecialty = IdSpecialty,
                IsActive = true
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, "Ocurrió un error al procesar la solicitud.");
            Console.WriteLine($"Error: {ex.Message}");
        }

        ViewBag.Specialties = await _context.MedicalSpecialties.ToListAsync();
        ViewBag.User = user;
        return RedirectToAction("Index", "RegistroPersonal");
    }

    [HttpPost]
    public IActionResult ActivateUser(int id)
    {
        var user = _context.Users.Find(id);
        if (user == null)
        {
            return NotFound();
        }

        user.IsActive = true;
        _context.SaveChanges();
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult DeactivateUser(int id)
    {
        var user = _context.Users.Find(id);
        if (user == null)
        {
            return NotFound();
        }

        user.IsActive = false;
        _context.SaveChanges();
        return RedirectToAction("Index");
    }

    // ✅ MÉTODO ACTUALIZADO: Evita eliminar si está en el consejo
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        if (user.IdMedicalCouncil == 1)
        {
            TempData["ErrorMessage"] = "Este usuario forma parte del Consejo Médico. Elimínalo primero del consejo para poder darlo de baja.";
            return RedirectToAction("Index");
        }

        user.IsActive = false;
        _context.Users.Update(user);
        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] = "Usuario eliminado exitosamente.";
        return RedirectToAction("Index");
    }
}
