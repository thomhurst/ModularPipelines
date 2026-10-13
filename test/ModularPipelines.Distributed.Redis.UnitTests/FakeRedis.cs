using System.Text;
using Respire;
using Respire.Testing;

namespace ModularPipelines.Distributed.Redis.UnitTests;

/// <summary>
/// A real Respire client connected to Respire's in-memory test server, so storage tests exercise the
/// commands the package sends without Docker or a Redis server.
/// </summary>
internal sealed class FakeRedis : IAsyncDisposable
{
    private FakeRedis(RespireFakeServer server, RespireClient client)
    {
        Server = server;
        Client = client;
    }

    public RespireFakeServer Server { get; }

    public RespireClient Client { get; }

    public static async Task<FakeRedis> StartAsync(TimeSpan? commandTimeout = null, RespireRetryPolicy? retryPolicy = null)
    {
        var server = new RespireFakeServer(TimeProvider.System);
        try
        {
            var options = server.CreateOptions() with
            {
                Connections = 1,
                CommandTimeout = commandTimeout ?? TimeSpan.FromSeconds(10),
                RetryPolicy = retryPolicy,
            };
            return new FakeRedis(server, await RespireClient.ConnectAsync(options));
        }
        catch
        {
            await server.DisposeAsync();
            throw;
        }
    }

    /// <summary>Applies <paramref name="fault"/> to commands whose first argument is <paramref name="key"/>.</summary>
    public RespireFakeFaultScope InjectFault(string command, string key, RespireFakeFault fault, int? occurrences = 1) =>
        Server.InjectFault(command, fault, occurrences, new ReadOnlyMemory<byte>(Encoding.UTF8.GetBytes(key)));

    public async ValueTask DisposeAsync()
    {
        await Client.DisposeAsync();
        await Server.DisposeAsync();
    }
}
