using System.Text.Json;
using ModularPipelines.Distributed.Serialization;
using ModularPipelines.Distributed.Redis;
using ModularPipelines.Distributed.Redis.Coordination;
using Moq;
using StackExchange.Redis;

namespace ModularPipelines.Distributed.Redis.UnitTests.Coordination;

public class RedisDistributedCoordinatorTests
{
    private const long ServerTimeSeconds = 1_700_000_000;
    private const long ServerTimeMicroseconds = 456_000;
    private const long ServerTimeMilliseconds = 1_700_000_000_456;

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
            KeyExpiration = TimeSpan.FromHours(1),
        };
        _dbMock.Setup(db => db.ExecuteAsync(
                "TIME",
                It.IsAny<ICollection<object>?>(),
                It.IsAny<CommandFlags>()))
            .ReturnsAsync(RedisResult.Create(
            [
                RedisResult.Create((RedisValue) ServerTimeSeconds),
                RedisResult.Create((RedisValue) ServerTimeMicroseconds),
            ]));
        _coordinator = new RedisDistributedCoordinator(_dbMock.Object, _subscriberMock.Object, _keys, _options);
    }

    [Test]
    public async Task EnqueueModuleAsync_AddsToSortedSetAndSetsExpiry()
    {
        var assignment = CreateAssignment("Test.Module");

        await _coordinator.EnqueueModuleAsync(assignment, CancellationToken.None);

        _dbMock.Verify(db => db.SortedSetAddAsync(
            _keys.WorkQueue,
            It.Is<RedisValue>(v => v.ToString().Contains("Test.Module")),
            It.IsAny<double>(),
            It.IsAny<SortedSetWhen>(),
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
    public async Task EnqueueModuleAsync_Preserves_Identical_Assignments()
    {
        var members = new List<RedisValue>();
        _dbMock.Setup(db => db.SortedSetAddAsync(
                _keys.WorkQueue,
                It.IsAny<RedisValue>(),
                It.IsAny<double>(),
                It.IsAny<SortedSetWhen>(),
                It.IsAny<CommandFlags>()))
            .Callback<RedisKey, RedisValue, double, SortedSetWhen, CommandFlags>(
                (_, member, _, _, _) => members.Add(member));
        var assignment = CreateAssignment("Test.Module");

        await _coordinator.EnqueueModuleAsync(assignment, CancellationToken.None);
        await _coordinator.EnqueueModuleAsync(assignment, CancellationToken.None);

        await Assert.That(members.Count).IsEqualTo(2);
        await Assert.That(members[0]).IsNotEqualTo(members[1]);
        await Assert.That(members.All(member => member.ToString().Contains("Test.Module"))).IsTrue();
    }

    [Test]
    public async Task QueueScore_Prefers_UserPriority_Then_CriticalPathWeight()
    {
        var highPriority = CreateAssignment("High") with
        {
            Priority = ModulePriority.High,
            CriticalPathWeight = TimeSpan.FromSeconds(1),
        };
        var longNormalPath = CreateAssignment("Normal") with
        {
            Priority = ModulePriority.Normal,
            CriticalPathWeight = TimeSpan.MaxValue,
        };
        var shortNormalPath = CreateAssignment("Short") with
        {
            Priority = ModulePriority.Normal,
            CriticalPathWeight = TimeSpan.FromSeconds(1),
        };

        await Assert.That(RedisDistributedCoordinator.GetQueueScore(highPriority))
            .IsGreaterThan(RedisDistributedCoordinator.GetQueueScore(longNormalPath));
        await Assert.That(RedisDistributedCoordinator.GetQueueScore(longNormalPath))
            .IsGreaterThan(RedisDistributedCoordinator.GetQueueScore(shortNormalPath));
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
            .ReturnsAsync(RedisResult.Create((RedisValue) json));

        var result = await _coordinator.DequeueModuleAsync(
            new HashSet<Capability>(), CancellationToken.None);

        await Assert.That(result).IsNotNull();
        await Assert.That(result!.ModuleTypeName).IsEqualTo("Test.Module");

        _dbMock.Verify(db => db.ScriptEvaluateAsync(
            It.Is<string>(script =>
                script.Contains("redis.call('TIME')", StringComparison.Ordinal)
                && script.Contains("/ 1000", StringComparison.Ordinal)
                && !script.Contains("redis.call('EXISTS'", StringComparison.Ordinal)),
            It.Is<RedisKey[]?>(keys => keys!.Length == 2
                && keys[0] == _keys.WorkQueue
                && keys[1] == _keys.Workers),
            It.Is<RedisValue[]?>(values => values != null
                && (double) values[1] == TimeSpan.FromSeconds(30).TotalMilliseconds),
            It.IsAny<CommandFlags>()), Times.Once);
    }

    [Test]
    public async Task DequeueModuleAsync_Deserializes_Unique_Queue_Member()
    {
        var assignment = CreateAssignment("Test.Module");
        var json = JsonSerializer.Serialize(assignment, JsonOptions);
        _dbMock.Setup(db => db.ScriptEvaluateAsync(
                It.IsAny<string>(),
                It.IsAny<RedisKey[]?>(),
                It.IsAny<RedisValue[]?>(),
                It.IsAny<CommandFlags>()))
            .ReturnsAsync(RedisResult.Create((RedisValue) $"{Guid.NewGuid():N}|{json}"));

        var result = await _coordinator.DequeueModuleAsync(
            new HashSet<Capability>(), CancellationToken.None);

        await Assert.That(result).IsNotNull();
        await Assert.That(result!.ModuleTypeName).IsEqualTo("Test.Module");
    }

    [Test]
    public async Task DequeueModuleAsync_Preserves_Pipe_In_Legacy_Queue_Member()
    {
        var assignment = CreateAssignment("Test|Module");
        var json = JsonSerializer.Serialize(assignment, JsonOptions);
        _dbMock.Setup(db => db.ScriptEvaluateAsync(
                It.IsAny<string>(),
                It.IsAny<RedisKey[]?>(),
                It.IsAny<RedisValue[]?>(),
                It.IsAny<CommandFlags>()))
            .ReturnsAsync(RedisResult.Create((RedisValue) json));

        var result = await _coordinator.DequeueModuleAsync(
            new HashSet<Capability>(), CancellationToken.None);

        await Assert.That(result).IsNotNull();
        await Assert.That(result!.ModuleTypeName).IsEqualTo("Test|Module");
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
            new HashSet<Capability> { "linux" }, cts.Token);

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
            new HashSet<Capability>(), cts.Token);

        await Assert.That(result).IsNull();
    }

    [Test]
    public async Task PublishResultAsync_SetsHashAndPublishes()
    {
        var serializedResult = CreateResult("Test.Module");

        await _coordinator.PublishResultAsync(serializedResult, CancellationToken.None);

        _dbMock.Verify(db => db.HashSetAsync(
            _keys.Results,
            (RedisValue) "Test.Module",
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

        _dbMock.Setup(db => db.HashGetAsync(_keys.Results, (RedisValue) "Test.Module", It.IsAny<CommandFlags>()))
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
            (RedisValue) "1",
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
    public async Task SendHeartbeatAsync_StoresTimestampInWorkersHash()
    {
        await _coordinator.SendHeartbeatAsync(7, CancellationToken.None);

        _dbMock.Verify(db => db.HashSetAsync(
            _keys.Workers,
            (RedisValue) "heartbeat:7",
            It.Is<RedisValue>(value => (long) value == ServerTimeMilliseconds),
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
                new HashEntry("heartbeat:1", ServerTimeMilliseconds),
                new HashEntry("heartbeat:2", ServerTimeMilliseconds),
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
            new HashSet<Capability>(), CancellationToken.None);

        await Assert.That(result).IsNull();
    }

    private static ModuleAssignment CreateAssignment(
        string moduleTypeName,
        HashSet<Capability>? requiredCapabilities = null)
    {
        return new ModuleAssignment(
            ModuleTypeName: moduleTypeName,
            ResultTypeName: "System.String",
            RequiredCapabilities: requiredCapabilities ?? new HashSet<Capability>(),
            AssignedAt: DateTimeOffset.UtcNow,
            Configuration: new ModuleAssignmentConfiguration(null, false));
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
            Capabilities: new HashSet<Capability> { "linux" },
            RegisteredAt: DateTimeOffset.UtcNow);
    }
}
