using ModularPipelines.Distributed.Redis.Artifacts;
using Moq;
using Respire;

namespace ModularPipelines.Distributed.Redis.UnitTests.Artifacts;

public class RedisDistributedArtifactStoreFactoryTests
{
    [Test]
    public async Task CreateAsync_Retries_A_Transient_Connection_Failure()
    {
        var attempts = 0;
        var provider = new RedisConnectionProvider(
            () => Interlocked.Increment(ref attempts) == 1
                ? Task.FromException<IRespireClient>(new RespireConnectionException("It was not possible to connect"))
                : Task.FromResult(Mock.Of<IRespireClient>()),
            ownsConnection: false);
        var factory = CreateFactory(provider);

        var store = await factory.CreateAsync(CancellationToken.None);

        await Assert.That(store).IsTypeOf<RedisDistributedArtifactStore>();
        await Assert.That(attempts).IsEqualTo(2);
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task CreateAsync_Does_Not_Retry_A_Permanent_Error(bool authentication)
    {
        var attempts = 0;
        var provider = new RedisConnectionProvider(
            () =>
            {
                Interlocked.Increment(ref attempts);
                return Task.FromException<IRespireClient>(authentication
                    ? new RespireAuthenticationException("WRONGPASS invalid username-password pair")
                    : new ArgumentException("Invalid connection string."));
            },
            ownsConnection: false);
        var factory = CreateFactory(provider);

        await Assert.That(async () => await factory.CreateAsync(CancellationToken.None))
            .Throws<Exception>();
        await Assert.That(attempts).IsEqualTo(1);
    }

    private static RedisDistributedArtifactStoreFactory CreateFactory(RedisConnectionProvider provider) =>
        new(
            Microsoft.Extensions.Options.Options.Create(new RedisOptions { ConnectionString = "unused" }),
            Microsoft.Extensions.Options.Options.Create(new DistributedOptions { RunId = "factory-run" }),
            provider);
}
