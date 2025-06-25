using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PRIII_24_CONTROL_ANTIBIOTICOS.Models;

namespace PRIII_24_CONTROL_ANTIBIOTICOS.Controllers;

[Authorize]
public class MedicalCouncilController : Controller
{
    private readonly BdProa1Context _context;
    public MedicalCouncilController(BdProa1Context context)
    {
        _context = context;
    }
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public IActionResult GetAdvisors()
    {
        var result = _context.Users
        .Where(us => us.IdMedicalCouncil == 1)
        .Join(_context.People,
              us => us.IdUser,
              p => p.IdPerson,
              (us, p) => new
              {
                  us.IdUser,
                  p.Name,
                  p.LastName,
                  p.SecondLastName,
                  IsMedicalCouncil = "SI" // Se utiliza `true` como equivalente de `convert(bit,1)`
              })
        .ToList();

        return Json(result);
    }

    [HttpGet]
    public IActionResult GetUsers()
    {
        var users = _context.Users
        .Where(us => us.Role == "Personal Hospitalario")
        .Join(_context.People,
              us => us.IdUser,
              p => p.IdPerson,
              (us, p) => new
              {
                  us.IdUser,
                  UserName = $"{p.Name} {p.LastName} {p.SecondLastName}"
              })
        .ToList();

        return Json(users);
    }
    [HttpPost]
    public IActionResult AddAdvisor(int userId)
    {
        var advisors = _context.Users.Where(x => x.IdMedicalCouncil == 1).ToList();
        if (advisors.Count >= 8)
        {
            return Json(new { success = false, message = "No puedes agregar más de 8 consejeros." });
        }

        if (advisors.Any(a => a.IdUser == userId))
        {
            return Json(new { success = false, message = "Este usuario ya ha sido agregado como consejero." });
        }

        var userMedicalCouncl = _context.Users.Where(x => x.IdUser == userId).FirstOrDefault();
        if(userMedicalCouncl != null)
        {
            userMedicalCouncl.IdMedicalCouncil = 1;
            _context.SaveChanges();
        }
        return Json(new { success = true });
    }

    [HttpPost]
    public IActionResult RemoveAdvisor(int userId)
    {
        var advisor = _context.Users.FirstOrDefault(a => a.IdUser == userId);
        if (advisor != null)
        {
            advisor.IdMedicalCouncil = null;
            _context.SaveChanges();
            return Json(new { success = true });
        }
        return Json(new { success = false, message = "Consejero no encontrado." });
    }
}
