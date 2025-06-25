using Microsoft.AspNetCore.Mvc;
using PRIII_24_CONTROL_ANTIBIOTICOS.Models;
using PRIII_24_CONTROL_ANTIBIOTICOS.Services.recursos;
using System.Security.Claims;

namespace PRIII_24_CONTROL_ANTIBIOTICOS.Controllers
{
    public class AdministrationRequestController : Controller
    {
        private readonly BdProa1Context _context;
        private readonly IEmailSender _emailService;


        public AdministrationRequestController(BdProa1Context context, IEmailSender emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SubmitRequest([FromBody] AdministrationRequest administrationRequest )
        {
            administrationRequest.RegisterDate = DateTime.Now;
            string x = "";
            var data = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            int idUser = Convert.ToInt32(data?.Value);

            ReturnModel<string> returnModel = new ReturnModel<string>
            {
                Success = true,
                Message = "Solicitud enviada con exito."
            };

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    AdministrationRequest newAdminReq = new AdministrationRequest()
                    {
                        IdRequest = administrationRequest.IdRequest,
                        IdUser = idUser,
                        MedicalJustification = administrationRequest.MedicalJustification,
                        Priority = administrationRequest.Priority,
                        IdMedicalHistory = administrationRequest.IdMedicalHistory,
                        Status = administrationRequest.Status,
                        Response = null,
                        RegisterDate = DateTime.Now
                    };
                    _context.Add(newAdminReq);
                    _context.SaveChanges();

                    foreach (var item in administrationRequest.RequestMedications)
                    {
                        RequestMedication rm = new RequestMedication()
                        {
                            IdMedication = item.IdMedication,
                            IdRequest = newAdminReq.IdRequest,
                            Dosage = item.Dosage,
                            Frecuency = item.Frecuency,
                            Administration = item.Administration
                        };
                        
                        _context.Add(rm);
                    }
                    _context.SaveChanges();


                    //Crear el voto
                    List<User> users = _context.Users.Where(u => u.IdMedicalCouncil != null && u.IdUser != idUser).ToList();
                    int count = 0;
                    foreach (var user in users)
                    {
                        if (count <= 5)
                        {
                            Vote vote = new Vote()
                            {
                                IdRequest = newAdminReq.IdRequest,
                                IdUser = user.IdUser,
                                PointVote = 0,
                                StatusVote = "SIN VOTO",
                                Administration = ""
                            };
                            _context.Add(vote);
                            count++;
                        }
                        else
                        {
                            break;
                        }
                    }
                    _context.SaveChanges();


                    transaction.Commit();

                    // Enviar correos a los usuarios del consejo médico
                    try
                    {
                        string emailSubject = "Nuevo Mensaje Importante para Votar";
                        string emailBody = $"Se le notifica que hay una nueva solicitud de administración de antibióticos que requiere su voto.<br>" +
                                            $"Prioridad: {newAdminReq.Priority}<br>Justificación médica: {newAdminReq.MedicalJustification}";

                        foreach (var user in users)
                        {
                            // Enviar el correo a cada miembro del consejo
                            _emailService.SendEmailAsync(user.Email, emailSubject, emailBody).Wait();
                        }
                    }
                    catch (Exception emailEx)
                    {
                        Console.WriteLine($"Error al enviar los correos: {emailEx.Message}");
                    }


                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    returnModel.Success = false;
                    returnModel.Message = "Error en el envio de solicitud...";
                }
            }

            return Json( returnModel);
        }
    }
}
