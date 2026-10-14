using FluentFTP;

namespace ModularPipelines.Ftp;

public interface IFtp
{
    /// <summary>
    /// Creates a connected client owned by this context.
    /// </summary>
    /// <remarks>
    /// Once context disposal starts, new connections are rejected and connections still being set up
    /// are disposed instead of returned. Context disposal waits for setup and cleanup. Setup retains
    /// the caller's cancellation token and configured connection timeouts.
    /// </remarks>
    /// <exception cref="ObjectDisposedException">Context disposal has started.</exception>
    Task<AsyncFtpClient> GetFtpClientAsync(FtpOptions options, CancellationToken cancellationToken = default);
}
