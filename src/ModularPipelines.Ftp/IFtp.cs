using FluentFTP;

namespace ModularPipelines.Ftp;

public interface IFtp
{
    /// <summary>
    /// Creates and connects a client that is disposed with this FTP context.
    /// </summary>
    /// <param name="options">The connection settings and optional client configurator.</param>
    /// <param name="cancellationToken">Cancels connection establishment.</param>
    Task<AsyncFtpClient> GetFtpClientAsync(FtpOptions options, CancellationToken cancellationToken = default);
}