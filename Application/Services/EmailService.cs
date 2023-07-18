using Application.Commons;
using System.Net.Mail;
using System.Net;
using Application.IServices;

namespace Application.Services;

public class EmailService : IEmailService
{
    private readonly AppConfiguration _appConfiguration;

    public EmailService(AppConfiguration appConfiguration)
    {
        _appConfiguration = appConfiguration;
    }
    public async Task<bool> SendMailAsync(List<string> emails, string subject, string message)
    {
        try
        {
            var email = _appConfiguration.EmailSetting.Email;
            var password = _appConfiguration.EmailSetting.Password;
            var dispayName = _appConfiguration.EmailSetting.DisplayName;
            MailMessage myMessage = new MailMessage();
            foreach (var mail in emails)
            {
                myMessage.To.Add(mail);
            }
            myMessage.IsBodyHtml = true;
            myMessage.From = new MailAddress(email, dispayName);
            myMessage.Subject = subject;
            myMessage.Body = message;
            using (SmtpClient smtp = new SmtpClient())
            {
                smtp.EnableSsl = true;
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(email, password);
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.SendCompleted += (s, e) => { smtp.Dispose(); };
                await smtp.SendMailAsync(myMessage);
            }
            return true;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}
