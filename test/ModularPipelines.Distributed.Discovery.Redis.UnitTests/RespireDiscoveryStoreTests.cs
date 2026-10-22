using Respire;
using Respire.Testing;

namespace ModularPipelines.Distributed.Discovery.Redis.UnitTests;

public class RespireDiscoveryStoreTests
{
    [Test]
    public async Task Set_And_Get_RoundTrip_With_Expiry()
    {
        await using var server = new RespireFakeServer(TimeProvider.System);
#pragma warning disable RESP010 // This isolated test shares one client across all store operations.
        await using var client = await RespireClient.ConnectAsync(server.CreateOptions());
#pragma warning restore RESP010
        await using var store = new RespireDiscoveryStore(client);

        await store.SetAsync("modpipe:run:master-endpoint", "{\"url\":\"https://master\"}", TimeSpan.FromMinutes(5), CancellationToken.None);
        var stored = await store.GetAsync("modpipe:run:master-endpoint", CancellationToken.None);
        var expiry = await client.Keys.ExpiryAsync("modpipe:run:master-endpoint");

        using (Assert.Multiple())
        {
            await Assert.That(stored).IsEqualTo("{\"url\":\"https://master\"}");
            await Assert.That(expiry.TimeToLive).IsNotNull().And.IsLessThanOrEqualTo(TimeSpan.FromMinutes(5));
        }
    }

    [Test]
    [Arguments(0, false)]
    [Arguments(1, true)]
    public async Task Refresh_And_Delete_Send_Ownership_And_Expiry_Arguments(int result, bool expected)
    {
        await using var server = new RespireFakeServer(TimeProvider.System);
#pragma warning disable RESP010 // This isolated test owns its connection.
        await using var client = await RespireClient.ConnectAsync(server.CreateOptions());
#pragma warning restore RESP010
        await using var store = new RespireDiscoveryStore(client);
        const string key = "modpipe:run:master-endpoint";
        server.Expect("EVALSHA", [RespireFakeArgument.Any, "1", key, "owner", "1500"], 1, RespireFakeResponse.Integer(result));
        server.Expect("EVALSHA", [RespireFakeArgument.Any, "1", key, "owner"], 1, RespireFakeResponse.Integer(result));

        await Assert.That(await store.RefreshAsync(key, "owner", TimeSpan.FromMilliseconds(1500), CancellationToken.None)).IsEqualTo(expected);
        await store.DeleteAsync(key, "owner", CancellationToken.None);
        server.VerifyAllExpectations();
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
