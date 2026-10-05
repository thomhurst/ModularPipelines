using System.Diagnostics.CodeAnalysis;
using System.Net;
using FluentFTP;

namespace ModularPipelines.Ftp;

[ExcludeFromCodeCoverage]
public record FtpOptions(
    string Host,
    NetworkCredential Credentials
)
{
    /// <summary>
    /// Gets whether the connection requires explicit FTPS, or implicit FTPS when configured.
    /// Encrypts both control and data connections without falling back to unencrypted FTP.
    /// Defaults to false for compatibility with servers that only support FTP.
    /// </summary>
    /// <remarks>
    /// When enabled, this policy takes precedence over <see cref="ClientConfigurator"/>:
    /// Auto and None encryption modes become Explicit, EncryptAuthenticationOnly is disabled,
    /// and DataConnectionEncryption is enabled. A configured Implicit mode is preserved.
    /// </remarks>
    public bool RequireEncryption { get; init; }

    public Action<IAsyncFtpClient>? ClientConfigurator { get; init; }
}
