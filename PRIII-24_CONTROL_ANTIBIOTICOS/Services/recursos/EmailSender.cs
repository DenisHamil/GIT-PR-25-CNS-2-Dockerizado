using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Net;
using PRIII_24_CONTROL_ANTIBIOTICOS.Services;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace PRIII_24_CONTROL_ANTIBIOTICOS.Services.recursos
{
    public class EmailSender : IEmailSender
    {
        private readonly SmtpSettings _smtpSettings;
        public EmailSender(IOptions<SmtpSettings> smtpSettings)
        {
            _smtpSettings = smtpSettings.Value;
        }
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress(_smtpSettings.Sender),
                Subject = subject,
                Body = htmlMessage,
                IsBodyHtml = true
            };
            mailMessage.To.Add(email);

            using (var client = new SmtpClient(_smtpSettings.Server, _smtpSettings.Port))
            {
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password);
                client.EnableSsl = true;

                try
                {
                    await client.SendMailAsync(mailMessage);
                }
                catch (Exception ex)
                {
                    // Log the exception for debugging
                    Console.WriteLine($"Error sending email: {ex.Message}");
                    throw; // Rethrow or handle as needed
                }
            }
        }


    }
}
