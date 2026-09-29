using ModularPipelines.Distributed.Redis.Artifacts;
using Moq;
using StackExchange.Redis;

namespace ModularPipelines.Distributed.Redis.UnitTests.Artifacts;

public class RedisDistributedArtifactStoreFactoryTests
{
    [Test]
    public async Task CreateAsync_Retries_A_Transient_Connection_Failure()
    {
        var connection = new Mock<IConnectionMultiplexer>();
        connection.Setup(value => value.GetDatabase(It.IsAny<int>(), It.IsAny<object>()))
            .Returns(Mock.Of<IDatabase>());
        var attempts = 0;
        var provider = new RedisConnectionProvider(
            () => Interlocked.Increment(ref attempts) == 1
                ? Task.FromException<IConnectionMultiplexer>(new RedisConnectionException(
                    ConnectionFailureType.UnableToConnect, CommandFlags.None, "It was not possible to connect", null, CommandStatus.Unknown))
                : Task.FromResult(connection.Object),
            ownsConnection: false);
        var factory = CreateFactory(provider);

        var store = await factory.CreateAsync(CancellationToken.None);

        await Assert.That(store).IsTypeOf<RedisDistributedArtifactStore>();
        await Assert.That(attempts).IsEqualTo(2);
    }

    [Test]
    public async Task CreateAsync_Does_Not_Retry_A_Configuration_Error()
    {
        var attempts = 0;
        var provider = new RedisConnectionProvider(
            () =>
            {
                Interlocked.Increment(ref attempts);
                return Task.FromException<IConnectionMultiplexer>(new ArgumentException("Invalid connection string."));
            },
            ownsConnection: false);
        var factory = CreateFactory(provider);

        await Assert.That(async () => await factory.CreateAsync(CancellationToken.None))
            .Throws<ArgumentException>();
        await Assert.That(attempts).IsEqualTo(1);
    }

    private static RedisDistributedArtifactStoreFactory CreateFactory(RedisConnectionProvider provider) =>
        new(
            Microsoft.Extensions.Options.Options.Create(new RedisOptions { ConnectionString = "unused" }),
            Microsoft.Extensions.Options.Options.Create(new DistributedOptions { RunId = "factory-run" }),
            provider);
}
