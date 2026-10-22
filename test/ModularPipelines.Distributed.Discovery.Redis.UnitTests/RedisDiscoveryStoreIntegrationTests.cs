using Respire;
using TUnit.Core.Exceptions;

namespace ModularPipelines.Distributed.Discovery.Redis.UnitTests;

public class RedisDiscoveryStoreIntegrationTests
{
    [Test]
    public async Task Scripts_Renew_And_Delete_Only_The_Owned_Advertisement()
    {
        var connectionString = Environment.GetEnvironmentVariable("MODULAR_PIPELINES_REDIS_TEST_CONNECTION_STRING");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new SkipTestException("Set MODULAR_PIPELINES_REDIS_TEST_CONNECTION_STRING to run real Redis discovery tests.");
        }

#pragma warning disable RESP010 // This integration test owns its isolated connection.
        await using var client = await RespireClient.ConnectAsync(connectionString);
#pragma warning restore RESP010
        await using var store = new RespireDiscoveryStore(client);
        var key = $"modpipe-discovery-test:{Guid.NewGuid():N}:master-endpoint";
        try
        {
            await store.SetAsync(key, "owner", TimeSpan.FromMinutes(1), CancellationToken.None);
            await Assert.That(await store.RefreshAsync(key, "owner", TimeSpan.FromMinutes(2), CancellationToken.None)).IsTrue();
            await Assert.That((await client.Keys.ExpiryAsync(key)).TimeToLive).IsNotNull().And.IsGreaterThan(TimeSpan.FromMinutes(1));
            await Assert.That(await store.RefreshAsync(key, "other", TimeSpan.FromMinutes(10), CancellationToken.None)).IsFalse();
            await store.DeleteAsync(key, "other", CancellationToken.None);
            await Assert.That(await store.GetAsync(key, CancellationToken.None)).IsEqualTo("owner");
            await store.DeleteAsync(key, "owner", CancellationToken.None);
            await Assert.That(await store.GetAsync(key, CancellationToken.None)).IsNull();
            await Assert.That(await store.RefreshAsync(key, "owner", TimeSpan.FromMinutes(2), CancellationToken.None)).IsFalse();
            await Assert.That(await store.GetAsync(key, CancellationToken.None)).IsNull();
        }
        finally
        {
            await client.DeleteAsync([key]);
        }
    }
}
