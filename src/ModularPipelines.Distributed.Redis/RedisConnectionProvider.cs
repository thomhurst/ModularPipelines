using StackExchange.Redis;

namespace ModularPipelines.Distributed.Redis;

/// <summary>
/// Lazily and asynchronously connects one <see cref="IConnectionMultiplexer"/> for a
/// <see cref="RedisOptions"/> instance. Each feature registration owns its provider, so no
/// feature adopts a multiplexer registered by another package or by the application.
/// </summary>
internal sealed class RedisConnectionProvider : IAsyncDisposable
{
    private readonly Func<Task<IConnectionMultiplexer>> _connect;
    private readonly bool _ownsConnection;
    private readonly SemaphoreSlim _gate = new(1, 1);
    private IConnectionMultiplexer? _connection;
    private Task<IConnectionMultiplexer>? _pendingConnect;

    public RedisConnectionProvider(RedisOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        _connect = async () => await ConnectionMultiplexer
            .ConnectAsync(CreateConfiguration(options))
            .ConfigureAwait(false);
        _ownsConnection = true;
    }

    /// <summary>
    /// Wraps an existing connection that the caller keeps ownership of.
    /// </summary>
    internal RedisConnectionProvider(IConnectionMultiplexer connection)
    {
        ArgumentNullException.ThrowIfNull(connection);
        _connection = connection;
        _connect = () => Task.FromResult(connection);
        _ownsConnection = false;
    }

    /// <summary>
    /// Connects with a caller-supplied function, for tests that simulate failed connection attempts.
    /// </summary>
    internal RedisConnectionProvider(Func<Task<IConnectionMultiplexer>> connect, bool ownsConnection)
    {
        ArgumentNullException.ThrowIfNull(connect);
        _connect = connect;
        _ownsConnection = ownsConnection;
    }

    public async Task<IConnectionMultiplexer> GetConnectionAsync(CancellationToken cancellationToken)
    {
        if (Volatile.Read(ref _connection) is { } existing)
        {
            return existing;
        }

        await _gate.WaitAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            if (_connection is not null)
            {
                return _connection;
            }

            // A canceled caller leaves the attempt running so the next caller reuses it
            // instead of opening (and leaking) a second multiplexer.
            var pending = _pendingConnect ??= _connect();
            try
            {
                var connection = await pending.WaitAsync(cancellationToken).ConfigureAwait(false);
                Volatile.Write(ref _connection, connection);
                return connection;
            }
            catch (Exception) when (pending.IsFaulted || pending.IsCanceled)
            {
                // Let a later call retry a failed connection attempt.
                _pendingConnect = null;
                throw;
            }
        }
        finally
        {
            _gate.Release();
        }
    }

    public async ValueTask DisposeAsync()
    {
        var pending = _pendingConnect;
        if (_ownsConnection && pending is not null)
        {
            try
            {
                var connection = await pending.ConfigureAwait(false);
                await connection.DisposeAsync().ConfigureAwait(false);
            }
            catch (Exception) when (pending.IsFaulted || pending.IsCanceled)
            {
                // The connection never opened, so there is nothing to release.
            }
        }

        _gate.Dispose();
    }

    internal static ConfigurationOptions CreateConfiguration(RedisOptions options)
    {
        var configuration = string.IsNullOrWhiteSpace(options.ConnectionString)
            ? new ConfigurationOptions()
            : ConfigurationOptions.Parse(options.ConnectionString);
        options.ConfigureConnection?.Invoke(configuration);
        return configuration;
    }
}
