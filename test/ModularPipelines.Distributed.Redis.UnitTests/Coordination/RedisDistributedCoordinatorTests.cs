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

        // Verify pub/sub notification was sent
        _subscriberMock.Verify(s => s.PublishAsync(
            It.Is<RedisChannel>(c => c.ToString() == _keys.WorkAvailableChannel),
            It.IsAny<RedisValue>(),
            It.IsAny<CommandFlags>()), Times.Once);
    }

    [Test]
    public async Task DequeueModuleAsync_ReturnsAssignment_WhenCapabilitiesMatch()
    {
        var assignment = CreateAssignment("Test.Module");
        var json = JsonSerializer.Serialize(assignment, JsonOptions);

        _dbMock.Setup(db => db.ScriptEvaluateAsync(
                It.IsAny<string>(),
                It.IsAny<RedisKey[]?>(),
                It.IsAny<RedisValue[]?>(),
                It.IsAny<CommandFlags>()))
            .ReturnsAsync(RedisResult.Create((RedisValue)json));

        var result = await _coordinator.DequeueModuleAsync(
            new HashSet<string>(), CancellationToken.None);

        await Assert.That(result).IsNotNull();
        await Assert.That(result!.ModuleTypeName).IsEqualTo("Test.Module");
    }

    [Test]
    public async Task DequeueModuleAsync_SkipsItem_WhenCapabilitiesDontMatch()
    {
        // Lua script returns nil when no matching item found
        _dbMock.Setup(db => db.ScriptEvaluateAsync(
                It.IsAny<string>(),
                It.IsAny<RedisKey[]?>(),
                It.IsAny<RedisValue[]?>(),
                It.IsAny<CommandFlags>()))
            .ReturnsAsync(RedisResult.Create(RedisValue.Null));

        using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(100));
        var result = await _coordinator.DequeueModuleAsync(
            new HashSet<string> { "linux" }, cts.Token);

        await Assert.That(result).IsNull();
    }

    [Test]
    public async Task DequeueModuleAsync_ReturnsNull_WhenCancelled()
    {
        // ScriptEvaluateAsync returns nil — nothing to dequeue
        _dbMock.Setup(db => db.ScriptEvaluateAsync(
                It.IsAny<string>(),
                It.IsAny<RedisKey[]?>(),
                It.IsAny<RedisValue[]?>(),
                It.IsAny<CommandFlags>()))
            .ReturnsAsync(RedisResult.Create(RedisValue.Null));

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
    public async Task SignalCompletionAsync_SetsKeyExpiryAndPublishes()
    {
        await _coordinator.SignalCompletionAsync(CancellationToken.None);

        _dbMock.Verify(db => db.KeyExpireAsync(
            _keys.CompletionFlag,
            TimeSpan.FromSeconds(3600),
            It.IsAny<ExpireWhen>(),
            It.IsAny<CommandFlags>()), Times.Once);

        _subscriberMock.Verify(s => s.PublishAsync(
            It.Is<RedisChannel>(c => c.ToString() == _keys.CompletionChannel),
            It.IsAny<RedisValue>(),
            It.IsAny<CommandFlags>()), Times.Once);
    }

    [Test]
    public async Task DequeueModuleAsync_ReturnsNull_WhenCompletionAlreadySignalled()
    {
        _dbMock.Setup(db => db.StringGetAsync(_keys.CompletionFlag, It.IsAny<CommandFlags>()))
            .ReturnsAsync("1");

        var result = await _coordinator.DequeueModuleAsync(
            new HashSet<string>(), CancellationToken.None);

        await Assert.That(result).IsNull();
    }

    private static ModuleAssignment CreateAssignment(
        string moduleTypeName,
        HashSet<string>? requiredCapabilities = null)
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
            RegisteredAt: DateTimeOffset.UtcNow);
    }
}
