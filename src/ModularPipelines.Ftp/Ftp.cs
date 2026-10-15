using FluentFTP;

namespace ModularPipelines.Ftp;

internal class Ftp : IAsyncDisposable, IFtp
{
    private readonly List<AsyncFtpClient> _clients = [];
    private readonly Lock _sync = new();
    private readonly TaskCompletionSource _connectionsDrained = new(TaskCreationOptions.RunContinuationsAsynchronously);
    private TaskCompletionSource? _disposalCompletion;
    private int _activeConnections;

    public async Task<AsyncFtpClient> GetFtpClientAsync(FtpOptions options, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        lock (_sync)
        {
            ObjectDisposedException.ThrowIf(_disposalCompletion is not null, this);
            _activeConnections++;
        }

        AsyncFtpClient? client = null;
        try
        {
            client = new AsyncFtpClient(options.Host, options.Credentials);
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
            }

            try
            {
                if (options.RequireEncryption)
                {
                    await client.Connect(cancellationToken).ConfigureAwait(false);
                }
                else
                {
                    await client.AutoConnect(cancellationToken).ConfigureAwait(false);
                }
            }
            catch (IOException exception) when (cancellationToken.IsCancellationRequested)
            {
                // FluentFTP can report a closed socket as an I/O failure during connection cancellation.
                throw new OperationCanceledException("The FTP connection was canceled.", exception, cancellationToken);
            }

            cancellationToken.ThrowIfCancellationRequested();
            lock (_sync)
            {
                // Disposal closes ownership before taking its snapshot. Late connections clean up in catch.
                ObjectDisposedException.ThrowIf(_disposalCompletion is not null, this);
                _clients.Add(client);
            }
            return client;
        }
        catch
        {
            if (client is not null)
            {
                await client.DisposeAsync().ConfigureAwait(false);
            }

            throw;
        }
        finally
        {
            lock (_sync)
            {
                _activeConnections--;
                if (_disposalCompletion is not null && _activeConnections == 0)
                {
                    _connectionsDrained.TrySetResult();
                }
            }
        }
    }

    public ValueTask DisposeAsync()
    {
        AsyncFtpClient[] clients;
        TaskCompletionSource completion;
        lock (_sync)
        {
            if (_disposalCompletion is not null)
            {
                return new ValueTask(_disposalCompletion.Task);
            }

            completion = _disposalCompletion = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
            clients = [.. _clients];
            _clients.Clear();
            if (_activeConnections == 0)
            {
                _connectionsDrained.SetResult();
            }
        }

        _ = DisposeClientsAsync(clients, completion);
        return new ValueTask(completion.Task);
    }

    private async Task DisposeClientsAsync(AsyncFtpClient[] clients, TaskCompletionSource completion)
    {
        // In-flight setup retains its caller's cancellation/timeout and must finish cleanup before disposal completes.
        await _connectionsDrained.Task.ConfigureAwait(false);
        List<Exception> failures = [];
        foreach (var client in clients)
        {
            try
            {
                await client.DisposeAsync().ConfigureAwait(false);
            }
            catch (Exception exception)
            {
                // A failed disposal must not prevent cleanup of the remaining owned clients.
                failures.Add(exception);
            }
        }

        if (failures.Count > 0)
        {
            completion.SetException(failures);
        }
        else
        {
            completion.SetResult();
        }
    }
}
