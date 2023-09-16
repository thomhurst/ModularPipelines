using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using ModularPipelines.Email.Options;

namespace ModularPipelines.Email;

internal class Email : IEmail
{
    public async Task<string> SendAsync(EmailSendOptions options, CancellationToken cancellationToken = default)
    {
        var email = new MimeMessage
        {
            From = { MailboxAddress.Parse(options.From) },
            Subject = options.Subject,
            Body = options.Body
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

        options.ClientConfigurator?.Invoke(smtp);
        
        await smtp.ConnectAsync(options.SmtpServerHost, options.Port, options.SecureSocketOptions,
            cancellationToken: cancellationToken);

        if (options.Credentials != null)
        {
            await smtp.AuthenticateAsync(options.Credentials, cancellationToken);
        }

        var response = await smtp.SendAsync(email, cancellationToken);
        await smtp.DisconnectAsync(true, cancellationToken);
        
        return response;
    }
}
