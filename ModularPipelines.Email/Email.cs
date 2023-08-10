using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;
using ModularPipelines.Email.Options;

namespace ModularPipelines.Email;

internal class Email : IEmail
{
    public async Task SendAsync(EmailSendOptions options, CancellationToken cancellationToken = default)
    {
        var email = new MimeMessage
        {
            From = { MailboxAddress.Parse(options.From) },
            Subject = options.Subject,
            Body = new TextPart(TextFormat.Html) { Text = options.Body }
        };
        
        email.To.AddRange(options.To.Select(MailboxAddress.Parse));

        if (options.Cc?.Any() == true)
        {
            email.Cc.AddRange(options.Cc.Select(MailboxAddress.Parse));
        }
        
        if (options.Bcc?.Any() == true)
        {
            email.Bcc.AddRange(options.Bcc.Select(MailboxAddress.Parse));
        }
        
        using var smtp = new SmtpClient();
        await smtp.ConnectAsync(options.SmtpServer, cancellationToken);
        await smtp.AuthenticateAsync(options.Credentials, cancellationToken);
        await smtp.SendAsync(email, cancellationToken);
        await smtp.DisconnectAsync(true, cancellationToken);
    }
}
