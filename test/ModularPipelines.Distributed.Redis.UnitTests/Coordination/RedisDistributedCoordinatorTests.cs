using ModularPipelines.Distributed.Redis;
using ModularPipelines.Distributed.Redis.Coordination;
using Moq;
using Respire;

namespace ModularPipelines.Distributed.Redis.UnitTests.Coordination;

/// <summary>
/// Behaviour that needs no Redis server. Queue, lease and pub/sub behaviour is covered against a real
/// server by <see cref="RedisDistributedCoordinatorContractTests"/>.
/// </summary>
public class RedisDistributedCoordinatorTests
{
    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Master_Heartbeats_Continue_During_Artifact_Renewal_And_Stop_On_Disposal(bool failFirstScan)
    {
        var scanStarted = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var heartbeatsContinued = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var scanStopped = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var scanCount = 0;
        var heartbeatCount = 0;
        var scripts = new Mock<IRespireScriptCommands>(MockBehavior.Strict);
        scripts.Setup(commands => commands.ExecuteIntegerAsync(
                It.IsAny<RespireScript>(), It.IsAny<RespireKey[]>(), It.IsAny<RespireValue[]>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(() =>
            {
                if (scanStarted.Task.IsCompleted && Interlocked.Increment(ref heartbeatCount) >= 3)
                {
                    heartbeatsContinued.TrySetResult();
                }

                return 1L;
            });
        var sets = new Mock<IRespireSetCommands>(MockBehavior.Strict);
        sets.Setup(commands => commands.ScanAsync(It.IsAny<RespireKey>(), It.IsAny<string?>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .Returns((RespireKey _, string? _, int _, CancellationToken token) => BlockScanAsync(token));
        var client = new Mock<IRespireClient>(MockBehavior.Strict);
        client.SetupGet(connection => connection.Scripts).Returns(scripts.Object);
        client.SetupGet(connection => connection.Sets).Returns(sets.Object);
        client.Setup(connection => connection.ExpireAsync(
                It.IsAny<RespireKey>(), It.IsAny<RespireExpiry>(), It.IsAny<RespireExpireWhen>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        await using var coordinator = new RedisDistributedCoordinator(
            client.Object, new RedisKeyBuilder("modpipe", "heartbeat-renewal"), new RedisOptions());

        coordinator.StartMasterHeartbeat(TimeSpan.FromMilliseconds(10));
        await scanStarted.Task.WaitAsync(TimeSpan.FromSeconds(10));
        await heartbeatsContinued.Task.WaitAsync(TimeSpan.FromSeconds(10));
        await coordinator.DisposeAsync();

        await Assert.That(scanStopped.Task.IsCompletedSuccessfully).IsTrue();
        await Assert.That(scanCount).IsEqualTo(failFirstScan ? 2 : 1);

        async IAsyncEnumerable<string> BlockScanAsync(
            [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken token)
        {
            if (Interlocked.Increment(ref scanCount) == 1 && failFirstScan)
            {
                throw new IOException("Artifact registry scan failed");
            }

            scanStarted.TrySetResult();
            try
            {
                await Task.Delay(Timeout.InfiniteTimeSpan, token);
            }
            finally
            {
                scanStopped.TrySetResult();
            }

            yield break;
        }
    }

    [Test]
    public async Task QueueOrder_Prefers_UserPriority_Then_CriticalPathWeight()
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

        // Queue members sort ascending by ordinal bytes, so a smaller order is claimed first.
        await Assert.That(string.CompareOrdinal(RedisQueueMember.GetOrder(highPriority), RedisQueueMember.GetOrder(longNormalPath)))
            .IsLessThan(0);
        await Assert.That(string.CompareOrdinal(RedisQueueMember.GetOrder(longNormalPath), RedisQueueMember.GetOrder(shortNormalPath)))
            .IsLessThan(0);
    }

    [Test]
    public async Task QueueOrder_Prefers_Earlier_Enqueues_Within_Equal_Priority_And_Weight()
    {
        var earlier = CreateAssignment("Earlier") with { EnqueuedAt = new DateTimeOffset(2026, 1, 1, 0, 0, 0, TimeSpan.Zero) };
        var later = earlier with { ModuleId = new ModuleId("Later"), EnqueuedAt = earlier.EnqueuedAt.AddMilliseconds(1) };

        await Assert.That(string.CompareOrdinal(RedisQueueMember.GetOrder(earlier), RedisQueueMember.GetOrder(later)))
            .IsLessThan(0);
    }

    [Test]
    public async Task QueueMember_Layout_Matches_The_Lua_Offsets()
    {
        var assignment = CreateAssignment("Layout") with
        {
            Priority = ModulePriority.Critical,
            CriticalPathWeight = TimeSpan.MaxValue,
            EnqueuedAt = DateTimeOffset.MaxValue,
        };
        var json = System.Text.Json.JsonSerializer.Serialize(assignment);
        var classId = RedisQueueMember.GetClassId(System.Text.Json.JsonSerializer.Serialize(RedisQueueMember.GetClass(assignment)));

        var member = RedisQueueMember.Create(classId, assignment, json);

        using (Assert.Multiple())
        {
            // The Claim script reads the order at Lua positions 18-84 and the assignment from position 86.
            await Assert.That(classId.Length).IsEqualTo(RedisQueueMember.ClassIdLength);
            await Assert.That(member[RedisQueueMember.ClassIdLength]).IsEqualTo('|');
            await Assert.That(member[RedisQueueMember.AssignmentOffset - 1]).IsEqualTo('|');
            await Assert.That(member[RedisQueueMember.AssignmentOffset..]).IsEqualTo(json);
            await Assert.That(RedisQueueMember.ParseAssignment(member).ModuleId).IsEqualTo(assignment.ModuleId);
        }
    }

    [Test]
    public async Task Assignments_With_The_Same_Requirements_Share_A_Class()
    {
        var first = CreateAssignment("First");
        var second = CreateAssignment("Second") with { Priority = ModulePriority.High };
        var alwaysRun = CreateAssignment("AlwaysRun") with { AlwaysRun = true };

        static string ClassId(ModuleAssignment assignment) =>
            RedisQueueMember.GetClassId(System.Text.Json.JsonSerializer.Serialize(RedisQueueMember.GetClass(assignment)));

        await Assert.That(ClassId(first)).IsEqualTo(ClassId(second));
        await Assert.That(ClassId(first)).IsNotEqualTo(ClassId(alwaysRun));
    }

    [Test]
    public async Task Key_Expiration_Must_Outlast_The_Result_Timeout()
    {
        var options = new RedisOptions { TimeToLive = TimeSpan.FromMinutes(30) };

        await Assert.That(() => RedisDistributedCoordinator.ValidateKeyExpiration(
                options,
                new DistributedOptions { ModuleResultTimeout = TimeSpan.FromMinutes(45) }))
            .Throws<InvalidOperationException>();
    }

    [Test]
    public async Task Key_Expiration_Must_Outlast_The_Worker_Timeout()
    {
        var options = new RedisOptions { TimeToLive = TimeSpan.FromSeconds(10) };

        await Assert.That(() => RedisDistributedCoordinator.ValidateKeyExpiration(
                options,
                new DistributedOptions
                {
                    ModuleResultTimeout = Timeout.InfiniteTimeSpan,
                    WorkerTimeout = TimeSpan.FromSeconds(30),
                    MasterTimeout = TimeSpan.FromSeconds(5),
                }))
            .Throws<InvalidOperationException>();
    }

    [Test]
    public async Task Key_Expiration_Must_Outlast_The_Master_Timeout()
    {
        var options = new RedisOptions { TimeToLive = TimeSpan.FromMinutes(1) };

        await Assert.That(() => RedisDistributedCoordinator.ValidateKeyExpiration(
                options,
                new DistributedOptions
                {
                    ModuleResultTimeout = Timeout.InfiniteTimeSpan,
                    WorkerTimeout = TimeSpan.FromSeconds(10),
                    MasterTimeout = TimeSpan.FromMinutes(1),
                }))
            .Throws<InvalidOperationException>()
            .WithMessageContaining(nameof(DistributedOptions.MasterTimeout));
    }

    [Test]
    public async Task Infinite_Result_Timeout_Preserves_Finite_Key_Expiration()
    {
        var options = new RedisOptions { TimeToLive = TimeSpan.FromMinutes(5) };
        RedisDistributedCoordinator.ValidateKeyExpiration(options, new DistributedOptions
        {
            ModuleResultTimeout = Timeout.InfiniteTimeSpan,
            WorkerTimeout = TimeSpan.FromMinutes(1),
            MasterTimeout = TimeSpan.FromMinutes(1),
        });

        await Assert.That(options.TimeToLive).IsEqualTo(TimeSpan.FromMinutes(5));
    }

    [Test]
    public async Task Default_Key_Expiration_Is_Valid()
    {
        RedisDistributedCoordinator.ValidateKeyExpiration(new RedisOptions(), new DistributedOptions());

        await Assert.That(new RedisOptions().TimeToLive)
            .IsGreaterThan(new DistributedOptions().ModuleResultTimeout);
    }

    private static ModuleAssignment CreateAssignment(string moduleId) => new()
    {
        ModuleId = new ModuleId(moduleId),
        RequiredCapabilities = CapabilityRequirement.None,
        PipelineSchemaVersion = "schema",
    };
}
