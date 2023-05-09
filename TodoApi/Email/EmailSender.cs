using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

public class EmailSender
{
  public async Task SendEmailAsync(string email, string subject, string message)
  {
    var emailMessage = new MimeMessage();

    emailMessage.From.Add(new MailboxAddress("Farida Fadilah", "farida.f@kiranatama.com"));
    emailMessage.To.Add(new MailboxAddress("", email));
    emailMessage.Subject = subject;
    emailMessage.Body = new TextPart("plain") { Text = message };

    using var client = new SmtpClient();
    await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
    await client.AuthenticateAsync("farida.f@kiranatama.com", "xxxxxxxx");
    await client.SendAsync(emailMessage);
    await client.DisconnectAsync(true);
  }
}