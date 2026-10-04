using ModularPipelines.Distributed.Redis;
using ModularPipelines.Distributed.Redis.Coordination;

namespace ModularPipelines.Distributed.Redis.UnitTests.Coordination;

/// <summary>
/// Behaviour that needs no Redis server. Queue, lease and pub/sub behaviour is covered against a real
/// server by <see cref="RedisDistributedCoordinatorContractTests"/>.
/// </summary>
public class RedisDistributedCoordinatorTests
{
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
