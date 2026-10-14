using System.Runtime.CompilerServices;
using Respire;

namespace ModularPipelines.Distributed.Redis;

/// <summary>
/// Lazily and asynchronously connects one <see cref="IRespireClient"/> for a
/// <see cref="RedisOptions"/> instance. Each feature registration owns its provider, so no
/// feature adopts a client registered by another package or by the application.
/// </summary>
internal sealed class RedisConnectionProvider : IAsyncDisposable
{
    // Keep the validated snapshot for the lifetime of each options instance without retaining
    // replaced options. Lazy ensures concurrent callers invoke the callback only once.
    private static readonly ConditionalWeakTable<RedisOptions, Lazy<RespireOptions>> Configurations = [];

    private readonly Func<Task<IRespireClient>> _connect;
    private readonly bool _ownsConnection;
    private readonly SemaphoreSlim _gate = new(1, 1);
    private IRespireClient? _connection;
    private Task<IRespireClient>? _pendingConnect;

    public RedisConnectionProvider(RedisOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        _connect = async () => await RespireClient
            .ConnectAsync(CreateConfiguration(options))
            .ConfigureAwait(false);
        _ownsConnection = true;
    }

    /// <summary>
    /// Wraps an existing connection that the caller keeps ownership of.
    /// </summary>
    internal RedisConnectionProvider(IRespireClient connection)
    {
        ArgumentNullException.ThrowIfNull(connection);
        _connection = connection;
        _connect = () => Task.FromResult(connection);
        _ownsConnection = false;
    }

    /// <summary>
    /// Connects with a caller-supplied function, for tests that simulate failed connection attempts.
    /// </summary>
    internal RedisConnectionProvider(Func<Task<IRespireClient>> connect, bool ownsConnection)
    {
        ArgumentNullException.ThrowIfNull(connect);
        _connect = connect;
        _ownsConnection = ownsConnection;
    }

    public async Task<IRespireClient> GetConnectionAsync(CancellationToken cancellationToken)
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
            // instead of opening (and leaking) a second client.
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

    internal static RespireOptions CreateConfiguration(RedisOptions options) =>
        CloneConfiguration(Configurations.GetValue(options, static value => new Lazy<RespireOptions>(
            () => BuildConfiguration(value))).Value);

    private static RespireOptions BuildConfiguration(RedisOptions options)
    {
        var configuration = string.IsNullOrWhiteSpace(options.ConnectionString)
            ? new RespireOptions()
            : RespireOptions.Parse(options.ConnectionString);

        // After a dropped connection, Respire fails the next command at once while it reconnects. The default
        // retry policy resends commands that were never written, and reads, so a brief disconnect does not fail
        // coordination. ConfigureConnection can replace it or set it to null.
        configuration = configuration with { RetryPolicy = configuration.RetryPolicy ?? new RespireRetryPolicy() };
        if (options.ConfigureConnection is { } configure)
        {
            configuration = configure(configuration)
                ?? throw new InvalidOperationException(
                    $"{nameof(RedisOptions)}.{nameof(RedisOptions.ConfigureConnection)} must not return null.");
        }

        // The callback may retain the options it returned. Neither it nor an individual connection
        // should be able to mutate the configuration that passed startup validation.
        return CloneConfiguration(configuration);
    }

    // RespireOptions is an immutable record except for its endpoint lists, which a copy would share.
    private static RespireOptions CloneConfiguration(RespireOptions configuration) => configuration with
    {
        Endpoints = [.. configuration.Endpoints],
        ReplicaEndpoints = [.. configuration.ReplicaEndpoints],
    };
}
