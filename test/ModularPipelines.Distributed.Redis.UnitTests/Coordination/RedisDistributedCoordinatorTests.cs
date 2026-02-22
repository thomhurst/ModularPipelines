using System.Text.Json;
using ModularPipelines.Distributed.Redis.Configuration;
using ModularPipelines.Distributed.Redis.Coordination;
using Moq;
using StackExchange.Redis;

namespace ModularPipelines.Distributed.Redis.UnitTests.Coordination;

public class RedisDistributedCoordinatorTests
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        Converters = { new ReadOnlySetJsonConverter() },
    };

    private Mock<IDatabase> _dbMock = null!;
    private Mock<ISubscriber> _subscriberMock = null!;
    private RedisKeyBuilder _keys = null!;
    private RedisDistributedOptions _options = null!;
    private RedisDistributedCoordinator _coordinator = null!;

    [Before(Test)]
    public void Setup()
    {
        _dbMock = new Mock<IDatabase>();
        _subscriberMock = new Mock<ISubscriber>();
        _keys = new RedisKeyBuilder("modpipe", "test-run");
        _options = new RedisDistributedOptions
        {
            KeyExpirationSeconds = 3600,
            DequeuePollDelayMilliseconds = 10,
        };
        _coordinator = new RedisDistributedCoordinator(_dbMock.Object, _subscriberMock.Object, _keys, _options);
    }

    [Test]
    public async Task EnqueueModuleAsync_PushesToListAndSetsExpiry()
    {
        var assignment = CreateAssignment("Test.Module");

        await _coordinator.EnqueueModuleAsync(assignment, CancellationToken.None);

        _dbMock.Verify(db => db.ListLeftPushAsync(
            _keys.WorkQueue,
            It.Is<RedisValue>(v => v.ToString().Contains("Test.Module")),
            It.IsAny<When>(),
            It.IsAny<CommandFlags>()), Times.Once);

        _dbMock.Verify(db => db.KeyExpireAsync(
            _keys.WorkQueue,
            TimeSpan.FromSeconds(3600),
            It.IsAny<ExpireWhen>(),
            It.IsAny<CommandFlags>()), Times.Once);
    }

    [Test]
    public async Task DequeueModuleAsync_ReturnsAssignment_WhenCapabilitiesMatch()
    {
        var assignment = CreateAssignment("Test.Module");
        var json = JsonSerializer.Serialize(assignment, JsonOptions);

        _dbMock.Setup(db => db.ListRangeAsync(_keys.WorkQueue, It.IsAny<long>(), It.IsAny<long>(), It.IsAny<CommandFlags>()))
            .ReturnsAsync([json]);

        _dbMock.Setup(db => db.ListRemoveAsync(_keys.WorkQueue, (RedisValue)json, 1, It.IsAny<CommandFlags>()))
            .ReturnsAsync(1);

        var result = await _coordinator.DequeueModuleAsync(
            new HashSet<string>(), CancellationToken.None);

        await Assert.That(result).IsNotNull();
        await Assert.That(result!.ModuleTypeName).IsEqualTo("Test.Module");
    }

    [Test]
    public async Task DequeueModuleAsync_SkipsItem_WhenCapabilitiesDontMatch()
    {
        var assignment = CreateAssignment("Docker.Module", requiredCapabilities: new HashSet<string> { "docker" });
        var json = JsonSerializer.Serialize(assignment, JsonOptions);

        _dbMock.Setup(db => db.ListRangeAsync(_keys.WorkQueue, It.IsAny<long>(), It.IsAny<long>(), It.IsAny<CommandFlags>()))
            .ReturnsAsync([json]);

        using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(100));
        var result = await _coordinator.DequeueModuleAsync(
            new HashSet<string> { "linux" }, cts.Token);

        await Assert.That(result).IsNull();

        // Verify item was never removed (capabilities didn't match, item stays in queue)
        _dbMock.Verify(db => db.ListRemoveAsync(
            _keys.WorkQueue,
            It.IsAny<RedisValue>(),
            It.IsAny<long>(),
            It.IsAny<CommandFlags>()), Times.Never);
    }

    [Test]
    public async Task DequeueModuleAsync_ReturnsNull_WhenCancelled()
    {
        // ListRangeAsync unmocked returns empty array — nothing to dequeue

        using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(50));
        var result = await _coordinator.DequeueModuleAsync(
            new HashSet<string>(), cts.Token);

        await Assert.That(result).IsNull();
    }

    [Test]
    public async Task PublishResultAsync_SetsHashAndPublishes()
    {
        var serializedResult = CreateResult("Test.Module");

        await _coordinator.PublishResultAsync(serializedResult, CancellationToken.None);

        _dbMock.Verify(db => db.HashSetAsync(
            _keys.Results,
            (RedisValue)"Test.Module",
            It.Is<RedisValue>(v => v.ToString().Contains("Test.Module")),
            It.IsAny<When>(),
            It.IsAny<CommandFlags>()), Times.Once);

        _subscriberMock.Verify(s => s.PublishAsync(
            It.Is<RedisChannel>(c => c.ToString() == _keys.ResultChannel("Test.Module")),
            It.IsAny<RedisValue>(),
            It.IsAny<CommandFlags>()), Times.Once);
    }

    [Test]
    public async Task WaitForResultAsync_ReturnsImmediately_WhenResultExists()
    {
        var serializedResult = CreateResult("Test.Module");
        var json = JsonSerializer.Serialize(serializedResult, JsonOptions);

        _dbMock.Setup(db => db.HashGetAsync(_keys.Results, (RedisValue)"Test.Module", It.IsAny<CommandFlags>()))
            .ReturnsAsync(json);

        var result = await _coordinator.WaitForResultAsync("Test.Module", CancellationToken.None);

        await Assert.That(result.ModuleTypeName).IsEqualTo("Test.Module");
        await Assert.That(result.WorkerIndex).IsEqualTo(1);
    }

    [Test]
    public async Task RegisterWorkerAsync_SetsHashAndExpiry()
    {
        var registration = CreateWorkerRegistration(1);

        await _coordinator.RegisterWorkerAsync(registration, CancellationToken.None);

        _dbMock.Verify(db => db.HashSetAsync(
            _keys.Workers,
            (RedisValue)"1",
            It.Is<RedisValue>(v => v.ToString().Contains("\"WorkerIndex\":1")),
            It.IsAny<When>(),
            It.IsAny<CommandFlags>()), Times.Once);

        _dbMock.Verify(db => db.KeyExpireAsync(
            _keys.Workers,
            TimeSpan.FromSeconds(3600),
            It.IsAny<ExpireWhen>(),
            It.IsAny<CommandFlags>()), Times.Once);
    }

    [Test]
    public async Task SendHeartbeatAsync_SetsHeartbeatHash()
    {
        _dbMock.Setup(db => db.HashGetAsync(_keys.Workers, (RedisValue)"1", It.IsAny<CommandFlags>()))
            .ReturnsAsync(RedisValue.Null);

        await _coordinator.SendHeartbeatAsync(1, CancellationToken.None);

        _dbMock.Verify(db => db.HashSetAsync(
            _keys.Heartbeats,
            (RedisValue)"1",
            It.IsAny<RedisValue>(),
            It.IsAny<When>(),
            It.IsAny<CommandFlags>()), Times.Once);
    }

    [Test]
    public async Task SendHeartbeatAsync_UpdatesWorkerStatus_FromConnectedToActive()
    {
        var worker = CreateWorkerRegistration(1);
        var workerJson = JsonSerializer.Serialize(worker, JsonOptions);

        _dbMock.Setup(db => db.HashGetAsync(_keys.Workers, (RedisValue)"1", It.IsAny<CommandFlags>()))
            .ReturnsAsync(workerJson);

        await _coordinator.SendHeartbeatAsync(1, CancellationToken.None);

        // Verify worker hash was updated with Status:1 (Active enum value)
        _dbMock.Verify(db => db.HashSetAsync(
            _keys.Workers,
            (RedisValue)"1",
            It.Is<RedisValue>(v => v.ToString().Contains("\"Status\":1")),
            It.IsAny<When>(),
            It.IsAny<CommandFlags>()), Times.Once);
    }

    [Test]
    public async Task GetRegisteredWorkersAsync_ReturnsAllWorkers()
    {
        var worker1 = CreateWorkerRegistration(1);
        var worker2 = CreateWorkerRegistration(2);

        _dbMock.Setup(db => db.HashGetAllAsync(_keys.Workers, It.IsAny<CommandFlags>()))
            .ReturnsAsync(
            [
                new HashEntry("1", JsonSerializer.Serialize(worker1, JsonOptions)),
                new HashEntry("2", JsonSerializer.Serialize(worker2, JsonOptions)),
            ]);

        var workers = await _coordinator.GetRegisteredWorkersAsync(CancellationToken.None);

        await Assert.That(workers.Count).IsEqualTo(2);
    }

    [Test]
    public async Task BroadcastCancellationAsync_PublishesToChannel()
    {
        await _coordinator.BroadcastCancellationAsync("test reason", CancellationToken.None);

        _subscriberMock.Verify(s => s.PublishAsync(
            It.Is<RedisChannel>(c => c.ToString() == _keys.CancellationChannel),
            It.Is<RedisValue>(v => v.ToString().Contains("test reason")),
            It.IsAny<CommandFlags>()), Times.Once);
    }

    [Test]
    public async Task BroadcastCancellationAsync_StoresSignalInRedis()
    {
        // After broadcast, IsCancellationRequested should find the signal
        // Setup the StringGet to return what StringSet would have stored
        var signal = new CancellationSignal("test reason", DateTimeOffset.UtcNow);
        var json = JsonSerializer.Serialize(signal, JsonOptions);

        _dbMock.Setup(db => db.StringGetAsync(_keys.Cancellation, It.IsAny<CommandFlags>()))
            .ReturnsAsync(json);

        var result = await _coordinator.IsCancellationRequestedAsync(CancellationToken.None);

        await Assert.That(result).IsNotNull();
        await Assert.That(result!.Reason).IsEqualTo("test reason");
    }

    [Test]
    public async Task IsCancellationRequestedAsync_ReturnsNull_WhenNotCancelled()
    {
        _dbMock.Setup(db => db.StringGetAsync(_keys.Cancellation, It.IsAny<CommandFlags>()))
            .ReturnsAsync(RedisValue.Null);

        var result = await _coordinator.IsCancellationRequestedAsync(CancellationToken.None);

        await Assert.That(result).IsNull();
    }

    [Test]
    public async Task IsCancellationRequestedAsync_ReturnsSignal_WhenCancelled()
    {
        var signal = new CancellationSignal("failure", DateTimeOffset.UtcNow);
        var json = JsonSerializer.Serialize(signal, JsonOptions);

        _dbMock.Setup(db => db.StringGetAsync(_keys.Cancellation, It.IsAny<CommandFlags>()))
            .ReturnsAsync(json);

        var result = await _coordinator.IsCancellationRequestedAsync(CancellationToken.None);

        await Assert.That(result).IsNotNull();
        await Assert.That(result!.Reason).IsEqualTo("failure");
    }

    private static ModuleAssignment CreateAssignment(
        string moduleTypeName,
        IReadOnlySet<string>? requiredCapabilities = null)
    {
        return new ModuleAssignment(
            ModuleTypeName: moduleTypeName,
            ResultTypeName: "System.String",
            RequiredCapabilities: requiredCapabilities ?? new HashSet<string>(),
            MatrixTarget: null,
            AssignedAt: DateTimeOffset.UtcNow,
            Configuration: new ModuleAssignmentConfig(null, 0, false));
    }

    private static SerializedModuleResult CreateResult(string moduleTypeName)
    {
        return new SerializedModuleResult(
            ModuleTypeName: moduleTypeName,
            ResultTypeName: "System.String",
            WorkerIndex: 1,
            SerializedJson: "{}",
            CompletedAt: DateTimeOffset.UtcNow);
    }

    private static WorkerRegistration CreateWorkerRegistration(int workerIndex)
    {
        return new WorkerRegistration(
            WorkerIndex: workerIndex,
            Capabilities: new HashSet<string> { "linux" },
            RegisteredAt: DateTimeOffset.UtcNow,
            Status: WorkerStatus.Connected,
            CurrentModule: null);
    }
}
