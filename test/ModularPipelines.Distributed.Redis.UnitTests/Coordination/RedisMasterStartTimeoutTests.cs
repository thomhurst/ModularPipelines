using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Time.Testing;
using ModularPipelines.Distributed.Coordination;
using ModularPipelines.Distributed.Redis.Coordination;
using Moq;
using Respire;

namespace ModularPipelines.Distributed.Redis.UnitTests.Coordination;

public class RedisMasterStartTimeoutTests
{
    [Test]
    public async Task Default_Deadline_Is_Five_Minutes()
    {
        await Assert.That(new RedisOptions().MasterStartTimeout).IsEqualTo(TimeSpan.FromMinutes(5));
    }

    [Test]
    public async Task Deadline_Starts_At_Registration_And_Reconnect_Does_Not_Reset_It()
    {
        var clock = new FakeTimeProvider();
        var logger = new Mock<ILogger<RedisDistributedCoordinator>>();
        var (client, _) = CreateClient(3);
        await using var coordinator = CreateCoordinator(client.Object, clock, logger.Object);
        clock.Advance(TimeSpan.FromHours(1));
        await RegisterAsync(coordinator);
        clock.Advance(TimeSpan.FromSeconds(29));
        await Assert.That(await coordinator.GetMasterStateAsync(CancellationToken.None)).IsEqualTo(DistributedMasterState.Running);
        await RegisterAsync(coordinator);
        clock.Advance(TimeSpan.FromSeconds(1));
        await Assert.That(await coordinator.GetMasterStateAsync(CancellationToken.None)).IsEqualTo(DistributedMasterState.Lost);
        logger.Verify(log => log.Log(
            LogLevel.Error, It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((state, _) => (state != null && state.ToString()!.Contains("did not start", StringComparison.Ordinal))),
            It.IsAny<Exception?>(), It.IsAny<Func<It.IsAnyType, Exception?, string>>()), Times.Once);
    }

    [Test]
    [Arguments(0, DistributedMasterState.Running)]
    [Arguments(1, DistributedMasterState.Completed)]
    [Arguments(2, DistributedMasterState.Lost)]
    public async Task Started_Or_Completed_Master_Is_Not_Subject_To_Startup_Deadline(long state, DistributedMasterState expected)
    {
        var clock = new FakeTimeProvider();
        var (client, _) = CreateClient(state);
        await using var coordinator = CreateCoordinator(client.Object, clock);
        await RegisterAsync(coordinator);
        clock.Advance(TimeSpan.FromMinutes(1));
        await Assert.That(await coordinator.GetMasterStateAsync(CancellationToken.None)).IsEqualTo(expected);
    }

    [Test]
    public async Task A_Started_Master_Whose_Keys_Disappear_Is_Lost_Immediately()
    {
        var clock = new FakeTimeProvider();
        var (client, scripts) = CreateClient(0);
        await using var coordinator = CreateCoordinator(client.Object, clock);
        await RegisterAsync(coordinator);
        await Assert.That(await coordinator.GetMasterStateAsync(CancellationToken.None)).IsEqualTo(DistributedMasterState.Running);
        SetupState(scripts, 3);
        await Assert.That(await coordinator.GetMasterStateAsync(CancellationToken.None)).IsEqualTo(DistributedMasterState.Lost);
    }

    [Test]
    [Arguments(0)]
    [Arguments(-1)]
    [Arguments(-2)]
    public async Task Startup_Validation_Rejects_Nonpositive_Or_Infinite_Deadlines(int milliseconds)
    {
        var validator = new RedisOptionsValidator(string.Empty,
            Microsoft.Extensions.Options.Options.Create(new DistributedOptions()));
        var result = validator.Validate(string.Empty, new RedisOptions
        {
            ConnectionString = "localhost:6379",
            MasterStartTimeout = TimeSpan.FromMilliseconds(milliseconds),
        });
        await Assert.That(result.Failed).IsTrue();
        await Assert.That(result.Failures).Contains("RedisOptions.MasterStartTimeout must be positive and finite.");
    }

    private static RedisDistributedCoordinator CreateCoordinator(
        IRespireClient client, TimeProvider clock, ILogger<RedisDistributedCoordinator>? logger = null) =>
        new(client, new RedisKeyBuilder("test", "startup"),
            new RedisOptions { MasterStartTimeout = TimeSpan.FromSeconds(30) }, logger: logger, timeProvider: clock);

    private static Task RegisterAsync(RedisDistributedCoordinator coordinator) =>
        coordinator.RegisterWorkerAsync(new WorkerRegistration
        {
            WorkerId = new WorkerId("early-worker"),
            Capabilities = [],
            RegisteredAt = DateTimeOffset.UnixEpoch,
        }, CancellationToken.None);

    private static (Mock<IRespireClient> Client, Mock<IRespireScriptCommands> Scripts) CreateClient(long state)
    {
        var scripts = new Mock<IRespireScriptCommands>(MockBehavior.Strict);
        scripts.Setup(commands => commands.ExecuteStringAsync(
                It.IsAny<RespireScript>(), It.IsAny<RespireKey[]>(), It.IsAny<RespireValue[]>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync("registered");
        SetupState(scripts, state);
        var client = new Mock<IRespireClient>(MockBehavior.Strict);
        client.SetupGet(connection => connection.Scripts).Returns(scripts.Object);
        return (client, scripts);
    }

    private static void SetupState(Mock<IRespireScriptCommands> scripts, long state) =>
        scripts.Setup(commands => commands.ExecuteIntegerAsync(
                It.IsAny<RespireScript>(), It.IsAny<RespireKey[]>(), It.IsAny<RespireValue[]>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(state);
}
