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

        db.Setup(d => d.StringSetAsync(
                It.IsAny<RedisKey>(),
                It.IsAny<RedisValue>(),
                It.IsAny<Expiration>(),
                It.IsAny<ValueCondition>(),
                It.IsAny<CommandFlags>()))
            .ReturnsAsync(true);

        var connection = new Mock<IConnectionMultiplexer>();
        connection.Setup(c => c.GetDatabase(It.IsAny<int>(), It.IsAny<object>())).Returns(db.Object);

        var options = new RedisDiscoveryOptions
        {
            KeyPrefix = "test-prefix",
            Ttl = TimeSpan.FromMinutes(10),
        };

        var discovery = new RedisMasterDiscovery(
            connection.Object, options, RunOptions(), NullLogger<RedisMasterDiscovery>.Instance);

        // Act
        await discovery.AdvertiseMasterEndpointAsync("http://master:5099", CancellationToken.None);

        // Assert
        db.Verify(d => d.StringSetAsync(
                It.IsAny<RedisKey>(),
                It.IsAny<RedisValue>(),
                It.Is<Expiration>(expiration =>
                    expiration.Equals(new Expiration(TimeSpan.FromMinutes(10)))),
                It.IsAny<ValueCondition>(),
                It.IsAny<CommandFlags>()),
            Times.Once);
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
            PollInterval = TimeSpan.FromMilliseconds(50),
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
            DiscoveryTimeout = TimeSpan.FromSeconds(1),
            PollInterval = TimeSpan.FromMilliseconds(100),
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
            DiscoveryTimeout = TimeSpan.FromSeconds(30),
            PollInterval = TimeSpan.FromMilliseconds(50),
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
