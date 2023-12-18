using System.Diagnostics.CodeAnalysis;
using System.Net;
using FluentFTP;

namespace ModularPipelines.Ftp.Options;

[ExcludeFromCodeCoverage]
public record FtpOptions(
    string Host,
    NetworkCredential Credentials
)
{
    public Action<IAsyncFtpClient>? ClientConfigurator { get; init; }
}