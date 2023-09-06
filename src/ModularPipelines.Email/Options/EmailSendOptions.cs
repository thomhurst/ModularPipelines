using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace ModularPipelines.Email.Options;

[ExcludeFromCodeCoverage]
public record EmailSendOptions(
    string From,
    IEnumerable<string> To,
    string Subject,
    string Body,
    Uri SmtpServer,
    ICredentials Credentials
)
{
    public IEnumerable<string>? Cc { get; init; }
    public IEnumerable<string>? Bcc { get; init; }
}
