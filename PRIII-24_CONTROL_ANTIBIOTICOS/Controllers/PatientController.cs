using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PRIII_24_CONTROL_ANTIBIOTICOS.Models;

namespace PRIII_24_CONTROL_ANTIBIOTICOS.Controllers;

[Authorize]
public class PatientController : Controller
{
    private readonly BdProa1Context _context;

    public PatientController(BdProa1Context context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var pacientes = await _context.Patients
            .Include(p => p.IdPersonNavigation)
            .ToListAsync();

        return View(pacientes);
    }

    public IActionResult Details(int id)
    {
        return View(); // Placeholder si necesitás detalle de paciente
    }

    public IActionResult RegisterPatient()
    {
        return View(); // No se usa si el registro está en PersonController
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(IFormCollection collection)
    {
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = "DIRECTOR,PERSONAL HOSPITALARIO")]
    public async Task<IActionResult> Edit(int id)
    {
        var patient = await _context.Patients
            .Include(p => p.IdPersonNavigation)
            .FirstOrDefaultAsync(p => p.IdPatient == id);

        if (patient == null)
        {
            return NotFound();
        }

        return View(patient);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "DIRECTOR,PERSONAL HOSPITALARIO")]
    public async Task<IActionResult> Edit(int id, [Bind("IdPatient,PatientCode,IdPersonNavigation")] Patient patient)
    {
        if (id != patient.IdPatient)
        {
            return NotFound();
        }

        var existingPatient = await _context.Patients
            .Include(p => p.IdPersonNavigation)
            .FirstOrDefaultAsync(p => p.IdPatient == id);

        if (existingPatient == null)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                existingPatient.IdPersonNavigation.Name = patient.IdPersonNavigation.Name;
                existingPatient.IdPersonNavigation.LastName = patient.IdPersonNavigation.LastName;
                existingPatient.IdPersonNavigation.SecondLastName = patient.IdPersonNavigation.SecondLastName;
                existingPatient.IdPersonNavigation.Ci = patient.IdPersonNavigation.Ci;
                existingPatient.IdPersonNavigation.DateOfBirth = patient.IdPersonNavigation.DateOfBirth;
                existingPatient.IdPersonNavigation.Gender = patient.IdPersonNavigation.Gender;

                _context.Update(existingPatient.IdPersonNavigation);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }

        return View(patient);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "DIRECTOR,PERSONAL HOSPITALARIO")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            var patient = await _context.Patients
                .Include(p => p.IdPersonNavigation)
                .Include(p => p.MedicalHistories)
                .FirstOrDefaultAsync(p => p.IdPatient == id);

            if (patient == null)
                return Json(new { success = false, message = "Paciente no encontrado." });

            if (patient.MedicalHistories.Any())
                return Json(new { success = false, message = "Este paciente tiene historiales médicos y no puede ser eliminado." });

            _context.Patients.Remove(patient);

            if (patient.IdPersonNavigation != null)
            {
                _context.People.Remove(patient.IdPersonNavigation);
            }

            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Paciente y persona eliminados exitosamente." });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = $"Error al eliminar: {ex.Message}" });
        }
    }
}
