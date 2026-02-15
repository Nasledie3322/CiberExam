using Application.Interfaces;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace Application.Services;

public class EmailService : IEmailService
{
    private readonly EmailSettings _settings;

    public EmailService(IOptions<EmailSettings> settings)
    {
        _settings = settings.Value;
    }

    public async Task SendAsync(string to, string subject, string body)
    {
        var message = new MailMessage();
        message.To.Add(to);
        message.Subject = subject;
        message.Body = body;
        message.From = new MailAddress(_settings.Email);

        using var client = new SmtpClient(_settings.Host, _settings.Port)
        {
            Credentials = new NetworkCredential(
                _settings.Email,
                _settings.Password),
            EnableSsl = true
        };

        await client.SendMailAsync(message);
    }
}

public class EmailSettings
{
    public string Email { get; set; } = null!;
    public string Host { get; set; } = null!;
    public int Port { get; set; }
    public string Password { get; set; } = null!;
}
