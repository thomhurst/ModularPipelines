using FluentFTP;

namespace ModularPipelines.Ftp;

internal class Ftp : IAsyncDisposable, IFtp
{
    private readonly List<AsyncFtpClient> _clients = new();

    public async Task<AsyncFtpClient> GetFtpClientAsync(FtpOptions options, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var client = new AsyncFtpClient(options.Host, options.Credentials);
        try
        {
            options.ClientConfigurator?.Invoke(client);
            await client.AutoConnect(cancellationToken).ConfigureAwait(false);
            cancellationToken.ThrowIfCancellationRequested();
            _clients.Add(client);
            return client;
        }
        catch
        {
            await client.DisposeAsync().ConfigureAwait(false);
            throw;
        }
    }

    public async ValueTask DisposeAsync()
    {
        foreach (var asyncFtpClient in _clients)
        {
            await asyncFtpClient.DisposeAsync().ConfigureAwait(false);
        }
    }
}
