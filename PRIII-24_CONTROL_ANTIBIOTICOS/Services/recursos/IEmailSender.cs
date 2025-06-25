namespace PRIII_24_CONTROL_ANTIBIOTICOS.Services.recursos
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string to, string subject, string htmlMessage);
    }
}
