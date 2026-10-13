using Respire;
using Respire.Testing;

namespace ModularPipelines.Distributed.Redis.UnitTests;

public class RedisConnectionProviderTests
{
    [Test]
    public async Task Configuration_Defaults_To_Retrying_Unsent_Commands()
    {
        var configuration = RedisConnectionProvider.CreateConfiguration(new RedisOptions { ConnectionString = "localhost:6379" });

        await Assert.That(configuration.RetryPolicy).IsEqualTo(new RespireRetryPolicy());
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task ConfigureConnection_Can_Replace_Or_Remove_The_Retry_Policy(bool remove)
    {
        var custom = new RespireRetryPolicy { MaxAttempts = 5 };
        var configuration = RedisConnectionProvider.CreateConfiguration(new RedisOptions
        {
            ConnectionString = "localhost:6379",
            ConfigureConnection = connection => connection with { RetryPolicy = remove ? null : custom },
        });

        await Assert.That(configuration.RetryPolicy).IsEqualTo(remove ? null : custom);
    }

    [Test]
    public async Task Default_Retry_Policy_Hides_A_Dropped_Connection()
    {
        // Without a policy, the first command after a drop fails while the connection is replaced.
        var policy = RedisConnectionProvider.CreateConfiguration(new RedisOptions { ConnectionString = "localhost:6379" }).RetryPolicy;
        await using var redis = await FakeRedis.StartAsync(retryPolicy: policy);
        await redis.Client.SetAsync("key", (RespireValue) "value");
        using var fault = redis.InjectFault("GET", "key", RespireFakeFault.Disconnect(), occurrences: 2);

        await Assert.That(await redis.Client.GetStringAsync("key")).IsEqualTo("value");
        await Assert.That(await redis.Client.GetStringAsync("key")).IsEqualTo("value");
        await Assert.That(fault.MatchedCount).IsEqualTo(2);
    }

    [Test]
    public async Task Provider_Shares_One_Connection_And_Retries_A_Failed_Attempt()
    {
        await using var redis = await FakeRedis.StartAsync();
        var attempts = 0;
        await using var provider = new RedisConnectionProvider(
            () => Interlocked.Increment(ref attempts) == 1
                ? Task.FromException<IRespireClient>(new RespireConnectionException("refused"))
                : Task.FromResult<IRespireClient>(redis.Client),
            ownsConnection: false);

        await Assert.That(async () => await provider.GetConnectionAsync(CancellationToken.None))
            .Throws<RespireConnectionException>();
        var first = await provider.GetConnectionAsync(CancellationToken.None);
        var second = await provider.GetConnectionAsync(CancellationToken.None);

        await Assert.That(first).IsSameReferenceAs(redis.Client);
        await Assert.That(second).IsSameReferenceAs(first);
        await Assert.That(attempts).IsEqualTo(2);
    }
}
