using System.Net;
using System.Net.Mail;

public class EmailSender
{
    public async Task SendEmailAsync(string senderEmail, string subject, string message)
    {
        var recipientEmail = "WeCinema21@gmail.com";
        var recipientPassword = "jrcj rmsf fooj vedd";

        using var client = new SmtpClient("smtp.gmail.com", 587)
        {
            EnableSsl = true,
            Credentials = new NetworkCredential(recipientEmail, recipientPassword)
        };

        var mailMessage = new MailMessage(senderEmail, recipientEmail, subject, message);

        await client.SendMailAsync(mailMessage);
    }
}