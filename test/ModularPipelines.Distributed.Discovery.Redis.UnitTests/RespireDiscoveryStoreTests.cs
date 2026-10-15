using Respire;
using Respire.Testing;

namespace ModularPipelines.Distributed.Discovery.Redis.UnitTests;

public class RespireDiscoveryStoreTests
{
    [Test]
    public async Task Set_Get_And_Delete_RoundTrip_With_Expiry()
    {
        await using var server = new RespireFakeServer(TimeProvider.System);
#pragma warning disable RESP010 // This isolated test shares one client across all store operations.
        await using var client = await RespireClient.ConnectAsync(server.CreateOptions());
#pragma warning restore RESP010
        await using var store = new RespireDiscoveryStore(client);

        await store.SetAsync("modpipe:run:master-endpoint", "{\"url\":\"https://master\"}", TimeSpan.FromMinutes(5), CancellationToken.None);
        var stored = await store.GetAsync("modpipe:run:master-endpoint", CancellationToken.None);
        var expiry = await client.Keys.ExpiryAsync("modpipe:run:master-endpoint");
        await store.DeleteAsync("modpipe:run:master-endpoint", CancellationToken.None);

        using (Assert.Multiple())
        {
            await Assert.That(stored).IsEqualTo("{\"url\":\"https://master\"}");
            await Assert.That(expiry.TimeToLive).IsNotNull().And.IsLessThanOrEqualTo(TimeSpan.FromMinutes(5));
            await Assert.That(await store.GetAsync("modpipe:run:master-endpoint", CancellationToken.None)).IsNull();
        }
    }

    [Test]
    public async Task Store_Does_Not_Dispose_A_Borrowed_Connection()
    {
        await using var server = new RespireFakeServer(TimeProvider.System);
#pragma warning disable RESP010 // This test must own the borrowed connection to verify its lifetime.
        await using var client = await RespireClient.ConnectAsync(server.CreateOptions());
#pragma warning restore RESP010

        await using (new RespireDiscoveryStore(client))
        {
        }

        await Assert.That(await client.ExistsAsync("missing")).IsFalse();
    }
}
