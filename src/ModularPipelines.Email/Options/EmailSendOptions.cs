using System.Diagnostics.CodeAnalysis;
using System.Net;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace ModularPipelines.Email.Options;

[ExcludeFromCodeCoverage]
public record EmailSendOptions(
    string From,
    IEnumerable<string> To,
    string Subject,
    TextPart Body,
    string SmtpServerHost
)
{
    public IEnumerable<string>? Cc { get; init; }

    public IEnumerable<string>? Bcc { get; init; }

    public ICredentials? Credentials { get; init; }

    public int Port { get; init; } = 25;

    public Action<SmtpClient>? ClientConfigurator { get; init; }

    public SecureSocketOptions SecureSocketOptions { get; init; } = SecureSocketOptions.Auto;
}
