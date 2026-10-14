using Respire;

namespace ModularPipelines.Distributed.Discovery.Redis;

/// <summary>
/// Stores the endpoint through a Respire connection owned by discovery. The connection opens on first use,
/// asynchronously, so building the service provider never blocks on Redis.
/// </summary>
internal sealed class RespireDiscoveryStore : IRedisDiscoveryStore, IAsyncDisposable
{
    private readonly Func<CancellationToken, Task<IRespireClient>> _connect;
    private readonly SemaphoreSlim _connectLock = new(1, 1);
    private IRespireClient? _connection;
    private bool _ownsConnection;

    public RespireDiscoveryStore(RedisDiscoveryOptions options)
    {
        _ownsConnection = true;
        _connect = async cancellationToken =>
        {
            var configuration = options.GetConnectionConfiguration();
            return await RespireClient.ConnectAsync(configuration, cancellationToken).ConfigureAwait(false);
        };
    }

    internal RespireDiscoveryStore(IRespireClient connection)
    {
        _connection = connection;
        _connect = _ => Task.FromResult(connection);
    }

    public async Task SetAsync(
        string key,
        string value,
        TimeSpan ttl,
        CancellationToken cancellationToken)
    {
        var connection = await GetConnectionAsync(cancellationToken).ConfigureAwait(false);
        await connection.SetAsync(key, (RespireValue) value, ttl, cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<string?> GetAsync(string key, CancellationToken cancellationToken)
    {
        var connection = await GetConnectionAsync(cancellationToken).ConfigureAwait(false);
        return await connection.GetStringAsync(key, cancellationToken).ConfigureAwait(false);
    }

    public async Task DeleteAsync(string key, CancellationToken cancellationToken)
    {
        var connection = await GetConnectionAsync(cancellationToken).ConfigureAwait(false);
        await connection.DeleteAsync([key], cancellationToken).ConfigureAwait(false);
    }

    public async ValueTask DisposeAsync()
    {
        if (_ownsConnection && _connection is not null)
        {
            await _connection.DisposeAsync().ConfigureAwait(false);
        }

        _ownsConnection = false;
    }

    private async Task<IRespireClient> GetConnectionAsync(CancellationToken cancellationToken)
    {
        if (_connection is { } connection)
        {
            return connection;
        }

        await _connectLock.WaitAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            return _connection ??= await _connect(cancellationToken).ConfigureAwait(false);
        }
        finally
        {
            _connectLock.Release();
        }
    }
}
