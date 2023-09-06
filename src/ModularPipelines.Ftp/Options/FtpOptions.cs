using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace ModularPipelines.Ftp.Options;

[ExcludeFromCodeCoverage]
public record FtpOptions(
    string Host,
    NetworkCredential Credentials
);
