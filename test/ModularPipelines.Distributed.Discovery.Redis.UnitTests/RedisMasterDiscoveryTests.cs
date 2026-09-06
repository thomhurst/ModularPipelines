using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using ModularPipelines.Distributed;
using ModularPipelines.Distributed.Discovery.Redis;
using StackExchange.Redis;

namespace ModularPipelines.Distributed.Discovery.Redis.UnitTests;

public class RedisMasterDiscoveryTests
{
    [Test]
    public async Task AdvertiseMasterUrl_Writes_To_Redis()
    {
        // Arrange
        var db = new Mock<IDatabase>();

        // Setup all StringSetAsync overloads to capture the call
        // StackExchange.Redis 2.x has: StringSetAsync(RedisKey, RedisValue, TimeSpan?, ...)
        db.Setup(d => d.StringSetAsync(
                It.IsAny<RedisKey>(), It.IsAny<RedisValue>(),
                It.IsAny<TimeSpan?>(), It.IsAny<bool>(), It.IsAny<When>(), It.IsAny<CommandFlags>()))
            .ReturnsAsync(true);

        var connection = new Mock<IConnectionMultiplexer>();
        connection.Setup(c => c.GetDatabase(It.IsAny<int>(), It.IsAny<object>())).Returns(db.Object);

        var options = new RedisDiscoveryOptions
        {
            KeyPrefix = "test-prefix",
            TtlSeconds = 600,
        };

        var discovery = new RedisMasterDiscovery(
            connection.Object, options, RunOptions(), NullLogger<RedisMasterDiscovery>.Instance);

        // Act — should not throw
        await discovery.AdvertiseMasterEndpointAsync("http://master:5099", CancellationToken.None);
    }

    [Test]
    public async Task DiscoverMasterUrl_Returns_When_Key_Exists()
    {
        // Arrange
        var db = new Mock<IDatabase>();
        db.Setup(d => d.StringGetAsync(It.IsAny<RedisKey>(), It.IsAny<CommandFlags>()))
            .ReturnsAsync(new RedisValue("http://master:5099"));

        var connection = new Mock<IConnectionMultiplexer>();
        connection.Setup(c => c.GetDatabase(It.IsAny<int>(), It.IsAny<object>())).Returns(db.Object);

        var options = new RedisDiscoveryOptions
        {
            KeyPrefix = "test-prefix",
        };

        var discovery = new RedisMasterDiscovery(
            connection.Object, options, RunOptions(), NullLogger<RedisMasterDiscovery>.Instance);

        // Act
        var result = await discovery.DiscoverMasterEndpointAsync(CancellationToken.None);

        // Assert
        await Assert.That(result).IsEqualTo("http://master:5099");
    }

    [Test]
    public async Task DiscoverMasterUrl_Polls_Until_Available()
    {
        // Arrange
        var callCount = 0;
        var db = new Mock<IDatabase>();
        db.Setup(d => d.StringGetAsync(It.IsAny<RedisKey>(), It.IsAny<CommandFlags>()))
            .ReturnsAsync(() =>
            {
                callCount++;
                return callCount >= 3 ? new RedisValue("http://master:5099") : RedisValue.Null;
            });

        var connection = new Mock<IConnectionMultiplexer>();
        connection.Setup(c => c.GetDatabase(It.IsAny<int>(), It.IsAny<object>())).Returns(db.Object);

        var options = new RedisDiscoveryOptions
        {
            KeyPrefix = "test-prefix",
            PollIntervalMs = 50,
        };

        var discovery = new RedisMasterDiscovery(
            connection.Object, options, RunOptions(), NullLogger<RedisMasterDiscovery>.Instance);

        // Act
        var result = await discovery.DiscoverMasterEndpointAsync(CancellationToken.None);

        // Assert
        await Assert.That(result).IsEqualTo("http://master:5099");
        await Assert.That(callCount).IsGreaterThanOrEqualTo(3);
    }

    [Test]
    public async Task DiscoverMasterUrl_Times_Out()
    {
        // Arrange
        var db = new Mock<IDatabase>();
        db.Setup(d => d.StringGetAsync(It.IsAny<RedisKey>(), It.IsAny<CommandFlags>()))
            .ReturnsAsync(RedisValue.Null);

        var connection = new Mock<IConnectionMultiplexer>();
        connection.Setup(c => c.GetDatabase(It.IsAny<int>(), It.IsAny<object>())).Returns(db.Object);

        var options = new RedisDiscoveryOptions
        {
            KeyPrefix = "test-prefix",
            DiscoveryTimeoutSeconds = 1,
            PollIntervalMs = 100,
        };

        var discovery = new RedisMasterDiscovery(
            connection.Object, options, RunOptions(), NullLogger<RedisMasterDiscovery>.Instance);

        // Act & Assert
        await Assert.That(async () => await discovery.DiscoverMasterEndpointAsync(CancellationToken.None))
            .Throws<TimeoutException>();
    }

    [Test]
    public async Task DiscoverMasterUrl_Propagates_Caller_Cancellation()
    {
        // Arrange
        var db = new Mock<IDatabase>();
        db.Setup(d => d.StringGetAsync(It.IsAny<RedisKey>(), It.IsAny<CommandFlags>()))
            .ReturnsAsync(RedisValue.Null);

        var connection = new Mock<IConnectionMultiplexer>();
        connection.Setup(c => c.GetDatabase(It.IsAny<int>(), It.IsAny<object>())).Returns(db.Object);

        var options = new RedisDiscoveryOptions
        {
            KeyPrefix = "test-prefix",
            DiscoveryTimeoutSeconds = 30,
            PollIntervalMs = 50,
        };

        var discovery = new RedisMasterDiscovery(
            connection.Object, options, RunOptions(), NullLogger<RedisMasterDiscovery>.Instance);
        using var cancellation = new CancellationTokenSource(TimeSpan.FromMilliseconds(200));

        // Act & Assert
        await Assert.That(async () => await discovery.DiscoverMasterEndpointAsync(cancellation.Token))
            .Throws<OperationCanceledException>();
    }

    private static DistributedOptions RunOptions() => new() { RunId = "test-run" };
}
