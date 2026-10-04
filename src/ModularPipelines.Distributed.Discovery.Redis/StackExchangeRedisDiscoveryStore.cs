using StackExchange.Redis;

namespace ModularPipelines.Distributed.Discovery.Redis;

/// <summary>
/// Stores the endpoint through a connection owned by discovery. The connection opens on first use,
/// asynchronously, so building the service provider never blocks on Redis.
/// </summary>
internal sealed class StackExchangeRedisDiscoveryStore : IRedisDiscoveryStore, IAsyncDisposable
{
    private readonly Func<CancellationToken, Task<IConnectionMultiplexer>> _connect;
    private readonly SemaphoreSlim _connectLock = new(1, 1);
    private IConnectionMultiplexer? _connection;
    private bool _ownsConnection;

    public StackExchangeRedisDiscoveryStore(RedisDiscoveryOptions options)
    {
        _ownsConnection = true;
        _connect = async cancellationToken =>
        {
            var configuration = string.IsNullOrWhiteSpace(options.ConnectionString)
                ? new ConfigurationOptions()
                : ConfigurationOptions.Parse(options.ConnectionString);
            options.ConfigureConnection?.Invoke(configuration);
            return await ConnectionMultiplexer.ConnectAsync(configuration)
                .WaitAsync(cancellationToken)
                .ConfigureAwait(false);
        };
    }

    internal StackExchangeRedisDiscoveryStore(IConnectionMultiplexer connection)
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
        await connection.GetDatabase().StringSetAsync(key, value, ttl)
            .WaitAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<string?> GetAsync(string key, CancellationToken cancellationToken)
    {
        var connection = await GetConnectionAsync(cancellationToken).ConfigureAwait(false);
        var value = await connection.GetDatabase().StringGetAsync(key)
            .WaitAsync(cancellationToken)
            .ConfigureAwait(false);
        return value.HasValue ? value.ToString() : null;
    }

    public async ValueTask DisposeAsync()
    {
        if (_ownsConnection && _connection is not null)
        {
            await _connection.DisposeAsync().ConfigureAwait(false);
        }

        _ownsConnection = false;
    }

    private async Task<IConnectionMultiplexer> GetConnectionAsync(CancellationToken cancellationToken)
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
