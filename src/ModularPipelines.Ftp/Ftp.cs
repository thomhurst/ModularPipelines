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
            if (options.RequireEncryption)
            {
                // Auto-detection can authenticate in plaintext before returning a profile.
                // Explicit FTPS rejects an unsupported TLS upgrade before sending credentials.
                if (client.Config.EncryptionMode != FtpEncryptionMode.Implicit)
                {
                    client.Config.EncryptionMode = FtpEncryptionMode.Explicit;
                }

                client.Config.EncryptAuthenticationOnly = false;
                client.Config.DataConnectionEncryption = true;
                await client.Connect(cancellationToken).ConfigureAwait(false);
            }
            else
            {
                await client.AutoConnect(cancellationToken).ConfigureAwait(false);
            }

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
