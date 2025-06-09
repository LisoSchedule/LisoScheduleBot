using System.Net;
using System.Net.Mail;
using LisoScheduleBot.Config;
using LisoScheduleBot.Interfaces;

namespace LisoScheduleBot.Services;

public class EmailService : IEmailService
{
    private readonly string _password;

    public EmailService(AppConfig config)
    {
        _password = config.AppPassword;
    }

    public async Task SendMessage(string toAdress, string subject, string body)
    {
        var fromAddress = new MailAddress("lisoschedule@gmail.com", "ЛісоSchedule");
        var toAddress = new MailAddress(toAdress);

        var smtp = new SmtpClient
        {
            Host = "smtp.gmail.com",
            Port = 587,
            EnableSsl = true,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(fromAddress.Address, _password)
        };

        using var message = new MailMessage(fromAddress, toAddress)
        {
            Subject = subject,
            Body = body,
            IsBodyHtml = true
        };

        await Task.Run(() => smtp.Send(message));
    }
}
