using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PRIII_24_CONTROL_ANTIBIOTICOS.Models;
using System.Security.Claims;

namespace PRIII_24_CONTROL_ANTIBIOTICOS.Controllers;

[Authorize]
public class VoteController : Controller
{
    private readonly BdProa1Context _context;

    public VoteController(BdProa1Context context)
    {
        _context = context;
    }
    //public IActionResult Index()
    //{
    //    return View();
    //}
    public async Task<IActionResult> Index()
    {
        // Obtener todas las solicitudes de administración con información del paciente y del doctor asignado
        var administrationRequests = await _context.AdministrationRequests
            .Include(ar => ar.IdMedicalHistoryNavigation) // Incluye el historial médico
                .ThenInclude(mh => mh.IdPatientNavigation) // Incluye el paciente
                    .ThenInclude(p => p.IdPersonNavigation) // Incluye los datos de la persona (nombre, apellido)
            .Include(ar => ar.IdUserNavigation) // Incluye el usuario (doctor)
                .ThenInclude(u => u.IdPersonNavigation) // Incluye los datos de la persona (nombre, apellido)
            .Where(ar => ar.Status == "Pendiente")
            .OrderBy(ar => ar.Priority == "Emergencia" ? 1 : ar.Priority == "Urgente" ? 2 : 3)
            .ThenBy(ar => ar.RegisterDate)
            .ToListAsync();

        return View(administrationRequests);
    }

    public async Task<IActionResult> Details(int id)
    {
        //Buscar lo que debo votar
        Vote? vote = await _context.Votes.Where(x=>x.IdVote== id).FirstOrDefaultAsync();

        //Buscar La Administracion
        AdministrationRequest? request = await _context.AdministrationRequests.Where(x => x.IdRequest == vote.IdRequest).FirstOrDefaultAsync();

        // Buscar el historial médico por su ID
        var medicalHistory = await _context.MedicalHistories
            .Include(m => m.IdPatientNavigation) // Incluye la navegación al paciente
            .ThenInclude(p => p.IdPersonNavigation) // Incluye la navegación a la persona relacionada con el paciente
            .Include(m => m.IdDiagnosisNavigation) // Incluye la navegación al diagnóstico
            .FirstOrDefaultAsync(m => m.IdMedicalHistory == request.IdMedicalHistory);

        //Buscar medicaciones 

        List<MedicacionCEA> habitualHistories = await _context.HabitualConsumptionMedications
            .Include(m => m.IdMedicationNavigation) // Incluye la navegación al paciente
            .Where(m => m.IdMedicalHistory == id)
            .Select(m => new MedicacionCEA
            {
                idMedication = m.IdMedication,
                Administration = m.Administration,
                Dosis = m.Dosage,
                Frecuency = m.Frecuency,
                Medicamento = m.IdMedicationNavigation.Name
            }).ToListAsync();


        List<AllergyViewModel> allergies = await _context.MedicalHistoryAllergies
            .Where(x => x.IdMedicalHistory == id)
            .Join(_context.Allergies,
                mha => mha.IdAllergy,
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
            Medicaciones = habitualHistories,
            IdVote = vote.IdVote
        };

        return View(model);
    }
    public async Task<ActionResult> SaveVotation(VoteModel voteModel)
    {
        
        Vote? vote = await _context.Votes.Where(x=>x.IdVote==voteModel.IdVote).FirstOrDefaultAsync();
        if(vote != null)
        {
            decimal point = (decimal)0.0;
            if (voteModel.administracion.ToLower() == "normalidad")
            {
                point = (decimal)1.1;
            }
            else if (voteModel.administracion.ToLower() == "precaucion")
            {
                point = (decimal)1.0;
            }

            if (vote.StatusVote.ToLower() == "VOTADO".ToLower())
            {
                return RedirectToAction("Error");
            }
            else
            {
                vote.Administration = voteModel.administracion;
                vote.doses = voteModel.dosis;
                vote.Recommendation = voteModel.Recommendation;
                vote.StatusVote = "VOTADO";
                vote.PointVote = point;

                await _context.SaveChangesAsync();

                //
                //Verificar voto
                List<Vote> votes = _context.Votes.Where(x => x.IdRequest == vote.IdRequest && x.StatusVote == "VOTADO").ToList();
                int countVotes = votes.Count;
                if (countVotes > 4)
                {
                    decimal puntacion = votes.Sum(x => x.PointVote);
                    string statusVote = "";
                    if (puntacion < (decimal)3.0)
                    {
                        statusVote = "No recomendado";
                    }
                    else if (puntacion >= (decimal)3.0 && puntacion <= (decimal)3.1)
                    {
                        statusVote = "Precaucion";
                    }
                    else if (puntacion > (decimal)3.1)
                    {
                        statusVote = "Proceda";
                    }

                    var administrationRequest = _context.AdministrationRequests.Where(x => x.IdRequest == vote.IdRequest).FirstOrDefault();
                    if (administrationRequest != null)
                    {
                        administrationRequest.Status = statusVote;
                        _context.SaveChanges();
                    }
                }

                return RedirectToAction("Index");
            }
        }
        return RedirectToAction("Error");
    }

    public ActionResult Error() 
    {
        return View();
    }
    public ActionResult HasVote()
    {
        return View();
    }
    public async Task<IActionResult> GetVotes()
    {
        var data = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
        int id = Convert.ToInt32(data?.Value);
        var result = _context.Votes
        .Where(v => v.StatusVote == "SIN VOTO" && v.IdUser == id)
        .Join(_context.AdministrationRequests,
              v => v.IdRequest,
              a => a.IdRequest,
              (v, a) => new
              {
                  Id = v.IdVote,
                  a.MedicalJustification,
                  a.Priority
              })
        .ToList();

        return Json(result);
    }

    public async Task<IActionResult> Summary(int id)
    {
        ////Buscar lo que debo votar
        //Vote? vote = await _context.Votes.Where(x => x.IdVote == id).FirstOrDefaultAsync();

        ////Buscar La Administracion
        //AdministrationRequest? request = await _context.AdministrationRequests.Where(x => x.IdRequest == vote.IdRequest).FirstOrDefaultAsync();

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
                idMedication = m.IdMedication,
                Administration = m.Administration,
                Dosis = m.Dosage,
                Frecuency = m.Frecuency,
                Medicamento = m.IdMedicationNavigation.Name
            }).ToListAsync();


        List<AllergyViewModel> allergies = await _context.MedicalHistoryAllergies
            .Where(x => x.IdMedicalHistory == id)
            .Join(_context.Allergies,
                mha => mha.IdAllergy,
                alergy => alergy.IdAllergy,
                (mha, alergy) => new AllergyViewModel
                {
                    IdAllergy = alergy.IdAllergy,
                    AllergyName = alergy.Name,
                })
            .ToListAsync();

        List<VoteSumaryModel> votes =  _context.Votes
            .Join(_context.AdministrationRequests,
                  v => v.IdRequest,
                  a => a.IdRequest,
                  (v, a) => new { Vote = v, AdminRequest = a })
            .Where(va => va.AdminRequest.IdMedicalHistory == medicalHistory.IdMedicalHistory)
            .Join(_context.Users,
                  va => va.Vote.IdUser,
                  r => r.IdUser,
                  (va, r) => new { va.Vote, va.AdminRequest, User = r })
            .Join(_context.People,
                  varu => varu.User.IdUser,
                  p => p.IdPerson,
                  (varu, p) => new VoteSumaryModel
                  {
                      statusVote = varu.Vote.StatusVote,
                      pointVote = varu.Vote.PointVote,
                      Administration = varu.Vote.Administration,
                      Doses = varu.Vote.doses,
                      Recommendation = varu.Vote.Recommendation,
                      name = p.Name,
                      lastName = p.LastName,
                      secondLastName = p.SecondLastName
                  })
            .ToList();

        List<RequestMeditacionCEA> requestMeditacionCEAs = _context.RequestMedications
                    .Where(r => r.IdRequest == 1019)
                    .Join(
                        _context.Medications,
                        r => r.IdMedication,
                        m => m.IdMedication,
                        (r, m) => new RequestMeditacionCEA
                        {
                            Dosage = r.Dosage,
                            Frecuency = r.Frecuency,
                            Name = m.Name
                        }
                    )
                    .ToList();


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
            Medicaciones = habitualHistories,
            RequestMedications = requestMeditacionCEAs,
            //IdVote = vote.IdVote
            Votes = votes
        };

        return View(model);
    }
}
