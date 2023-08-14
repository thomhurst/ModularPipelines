using System.Net;

namespace ModularPipelines.Ftp.Options;

public record FtpOptions(
    string Host,
    NetworkCredential Credentials
);