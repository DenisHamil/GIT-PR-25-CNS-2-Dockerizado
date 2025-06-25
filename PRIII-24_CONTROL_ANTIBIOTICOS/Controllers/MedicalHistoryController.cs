using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PRIII_24_CONTROL_ANTIBIOTICOS.Models;


using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace PRIII_24_CONTROL_ANTIBIOTICOS.Controllers;
[Authorize]
public class MedicalHistoryController : Controller
{
    private readonly BdProa1Context _context;

    public MedicalHistoryController(BdProa1Context context)
    {
        _context = context;
    }
    // GET: MedicalHistoryController
    public async Task<IActionResult> Index(int id)
    {
        var medicalHistories = await _context.MedicalHistories
            .Where(m => m.IdPatient == id)
            .OrderByDescending(x => x.IdMedicalHistory)
            .ToListAsync();

        ViewData["PatientId"] = id; // Asegúrate de pasar el IdPatient
        return View(medicalHistories);
    }

    [HttpGet]
    public async Task<IActionResult> Create(int id)
    {
        //verificar
        var data = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
        int idUser = Convert.ToInt32(data?.Value);

        int countMeticalCouncil = _context.Users.Where(x=>x.IdMedicalCouncil == 1 && x.IdUser != idUser).Count();

        if (countMeticalCouncil >= 5)
        {


            var model = new MedicalHistory
            {
                IdPatient = id,
                IdPatientNavigation = _context.Patients.FirstOrDefault(p => p.IdPatient == id) // Asignar el objeto de navegación
            };
            ViewData["Allergies"] = new SelectList(await _context.Allergies.ToListAsync(), "IdAllergy", "Name");
            ViewData["Laboratories"] = new SelectList(await _context.Laboratories.ToListAsync(), "IdLaboratory", "TestName");
            ViewData["Medications"] = new SelectList(await _context.Medications.ToListAsync(), "IdMedication", "Name");
            ViewData["MedicationCategories"] = new SelectList(await _context.MedicationCategories.ToListAsync(), "IdCategory", "CategoryName");
            ViewData["Diagnosis"] = new SelectList(await _context.Diagnoses.ToListAsync(), "IdDiagnosis", "Name");

            ViewData["PatientId"] = id; // Pasa el IdPatient a la vista
            if (model.IdPatientNavigation == null)
            {
                return NotFound();
            }

            return View(model);
        }
        else
        {
            return RedirectToAction("ErrorMedicalCouncil");
        }
    }


    [HttpPost]
    //[ValidateAntiForgeryToken]
    public IActionResult Save(
        [FromBody] MedicalHistoryModel medicalHistor)
    {
        string x = "";
        ReturnModel<string> returnModel = new ReturnModel<string>
        {
            Success = true,
            Message = "Registro guardado con exito."
        };
        // Decodificar el archivo Base64
        byte[] archivoBytes = null;
        if (!string.IsNullOrEmpty(medicalHistor.ArchivoBase64))
        {
            archivoBytes = Convert.FromBase64String(medicalHistor.ArchivoBase64);
        }

        var generatedId = 0;

        using (var transaction = _context.Database.BeginTransaction())
        {
            try
            {
                // Primera operación: Agregar una nueva persona
                //procesar la informacion
                MedicalHistory medicalHistory = new MedicalHistory
                {
                    Weight = medicalHistor.Weight,
                    Bmi = medicalHistor.Bmi,
                    Height = medicalHistor.Height,
                    Bmiclassification = medicalHistor.Bmiclassification,
                    IdDiagnosis = medicalHistor.IdDiagnosis,
                    Justificative = medicalHistor.Justificative,
                    Archivepdf = archivoBytes,
                    IdPatient = medicalHistor.IdPatient,
                    RegisterDate = DateTime.Now
                };
                _context.Add(medicalHistory);
                _context.SaveChanges();

                // Segunda operación: Agregar un nuevo producto

                if (medicalHistor.Medicaciones != null)
                {
                    foreach (MedicacionCEA item in medicalHistor.Medicaciones)
                    {
                        HabitualConsumptionMedication habitualConsumptionMedication = new HabitualConsumptionMedication
                        {
                            Frecuency = item.Frecuency,
                            Administration = item.Administration,
                            Dosage = item.Dosis,
                            IdMedicalHistory = medicalHistory.IdMedicalHistory,
                            IdMedication = item.idMedication,
                            //IdMedicationNavigation = _context.Medications.FirstOrDefault(x=>x.IdMedication == item.idMedication),
                            //IdMedicalHistoryNavigation = _context.MedicalHistories.FirstOrDefault(x=>x.IdMedicalHistory == medicalHistory.IdMedicalHistory)
                            //IdPatientNavigation = _context.Patients.FirstOrDefault(p => p.IdPatient == id)
                        };
                        _context.Add(habitualConsumptionMedication);
                    }
                    if (medicalHistor.Medicaciones.Count > 0)
                    {
                        _context.SaveChanges();
                    }
                }

                // Tercera operacion

                if (medicalHistor.Allergies != null)
                {
                    foreach (short idAllergy in medicalHistor.Allergies)
                    {
                        MedicalHistoryAllergy medicalHistoryAllergy = new MedicalHistoryAllergy
                        {
                            IdAllergy = idAllergy,
                            IdMedicalHistory = medicalHistory.IdMedicalHistory
                        };
                        _context.Add(medicalHistoryAllergy);
                    }
                    if (medicalHistor.Allergies.Count > 0)
                    {
                        _context.SaveChanges();
                    }
                }

                //Cuarta operacion: subir la solicitud de administracion

                if (medicalHistor.Laboratories != null)
                {
                    foreach (int idLaboratory in medicalHistor.Laboratories)
                    {
                        MedicalHistoryLaboratory medicalHistoryLaboratory = new MedicalHistoryLaboratory
                        {
                            IdLaboratory = idLaboratory,
                            IdMedicalHistory = medicalHistory.IdMedicalHistory
                        };
                        _context.Add(medicalHistoryLaboratory);
                    }
                    if (medicalHistor.Laboratories.Count > 0)
                    {
                        _context.SaveChanges();
                    }
                }

                generatedId = medicalHistory.IdMedicalHistory;

                // Confirmar la transacción si todo sale bien
                transaction.Commit();
            }
            catch (Exception ex)
            {
                // En caso de error, revertir los cambios
                transaction.Rollback();
                //return BadRequest($"Error: {ex.Message}");
                returnModel.Success = false;
                returnModel.Message = "Error no controlado...";
            }
        }

        //return Json(returnModel);
        return Json(new { success = true, generatedId = generatedId });
    }

    public ActionResult Edit(int id)
    {
        return View();
    }

    // POST: MedicalHistoryController/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit(int id, IFormCollection collection)
    {
        try
        {
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            return View();
        }
    }

    // GET: MedicalHistoryController/Delete/5
    public ActionResult Delete(int id)
    {
        return View();
    }

    // POST: MedicalHistoryController/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Delete(int id, IFormCollection collection)
    {
        try
        {
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            return View();
        }
    }
    // Acción para descargar el archivo PDF
    public IActionResult DownloadPdf(int id)
    {
        // Obtener el registro de la base de datos por ID
        var medicalHistory = _context.MedicalHistories.Find(id);

        if (medicalHistory == null || medicalHistory.Archivepdf == null)
        {
            // Si no existe el registro o no tiene archivo, devuelve un error
            return NotFound();
        }

        // Crear un FileStreamResult para devolver el PDF
        return File(medicalHistory.Archivepdf, "application/pdf", "MedicalHistory.pdf");
    }
    // GET: MedicalHistoryController/Details/5
    public async Task<IActionResult> Details(int id)
    {
        // Buscar el historial médico por su ID
        var medicalHistory = await _context.MedicalHistories
            .Include(m => m.IdPatientNavigation) // Incluye la navegación al paciente
            .ThenInclude(p => p.IdPersonNavigation) // Incluye la navegación a la persona relacionada con el paciente
            .Include(m => m.IdDiagnosisNavigation) // Incluye la navegación al diagnóstico
            .FirstOrDefaultAsync(m => m.IdMedicalHistory == id);

        //Buscar medicaciones 

        List<MedicacionCEA> habitualHistories = await _context.HabitualConsumptionMedications
            .Include(m => m.IdMedicationNavigation) // Incluye la navegación al paciente
            .Where(m => m.IdMedicalHistory == id)
            .Select(m => new MedicacionCEA
            {
                idMedication=m.IdMedication,
                Administration=m.Administration,
                Dosis=m.Dosage,
                Frecuency=m.Frecuency,
                Medicamento=m.IdMedicationNavigation.Name
            }).ToListAsync();


        List<AllergyViewModel> allergies = await _context.MedicalHistoryAllergies
            .Where(x=>x.IdMedicalHistory == id)
            .Join(_context.Allergies,
                mha=>mha.IdAllergy,
                alergy => alergy.IdAllergy,
                (mha, alergy) => new AllergyViewModel
                {
                    IdAllergy = alergy.IdAllergy,
                    AllergyName = alergy.Name,
                })
            .ToListAsync();

        // Preparar el modelo para la vista
        var model = new MedicalHistoryDetailsViewModel
        {
            IdMedicalHistory = medicalHistory.IdMedicalHistory,
            Weight = medicalHistory.Weight,
            Height = medicalHistory.Height,
            Bmi = medicalHistory.Bmi,
            Bmiclassification = medicalHistory.Bmiclassification,
            RegisterDate = medicalHistory.RegisterDate,
            Justificative = medicalHistory.Justificative,
            Patient = new PatientViewModel
            {
                IdPatient = medicalHistory.IdPatientNavigation.IdPatient,
                Name = medicalHistory.IdPatientNavigation.IdPersonNavigation.Name,
                LastName = medicalHistory.IdPatientNavigation.IdPersonNavigation.LastName,
                Ci = medicalHistory.IdPatientNavigation.IdPersonNavigation.Ci,
                DateOfBirth = medicalHistory.IdPatientNavigation.IdPersonNavigation.DateOfBirth
            },

            Diagnosis = medicalHistory.IdDiagnosisNavigation?.Name,
            Allergies = allergies,
            Medicaciones = habitualHistories
        };

        return View(model);
    }

    public async Task<IActionResult> TreatmentObservation(int? id)
    {
        ViewData["MedicalHistoryId"] = id; // Asegúrate de pasar el IdPatient
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> TreatmentObservation([Bind("IdTO,Observation,Status")] TreatmentObservation treatmentObservation)
    {
        if (ModelState.IsValid)
        {
            treatmentObservation.RegisterDate = DateTime.Now;
            _context.Add(treatmentObservation);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", new { id = treatmentObservation.IdTO });
        }
        ViewData["MedicalHistoryId"] = treatmentObservation.IdTO;
        return View(treatmentObservation); // Devuelve el modelo para mostrar errores
    }
    public ActionResult ErrorMedicalCouncil()
    {
        return View();
    }



}
