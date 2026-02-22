using ModularPipelines.Distributed.Coordination;
using ModularPipelines.Distributed.Master;
using ModularPipelines.Distributed.Serialization;
using ModularPipelines.Enums;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Distributed.UnitTests.Integration;

public class DistributedPipelineIntegrationTests
{
    private class SimpleResult
    {
        public string Message { get; set; } = string.Empty;
    }

    private class ModuleA : Module<SimpleResult>
    {
        protected internal override Task<SimpleResult?> ExecuteAsync(
            ModularPipelines.Context.IModuleContext context,
            CancellationToken cancellationToken)
        {
            return Task.FromResult<SimpleResult?>(new SimpleResult { Message = "A done" });
        }
    }

    private class ModuleB : Module<string>
    {
        protected internal override Task<string?> ExecuteAsync(
            ModularPipelines.Context.IModuleContext context,
            CancellationToken cancellationToken)
        {
            return Task.FromResult<string?>("B done");
        }
    }

    private class ModuleC : Module<int>
    {
        protected internal override Task<int> ExecuteAsync(
            ModularPipelines.Context.IModuleContext context,
            CancellationToken cancellationToken)
        {
            return Task.FromResult(42);
        }
    }

    [Test]
    public async Task End_To_End_Publish_And_Collect_Result()
    {
        var coordinator = new InMemoryDistributedCoordinator();
        var registry = new ModuleTypeRegistry();
        registry.Register(typeof(ModuleA));
        var serializer = new ModuleResultSerializer(registry);
        var publisher = new DistributedWorkPublisher(coordinator, registry);
        var collector = new DistributedResultCollector(coordinator, serializer);

        // Master publishes work
        var assignment = publisher.CreateAssignment(typeof(ModuleA));
        await publisher.PublishAsync(assignment, CancellationToken.None);

        // Simulate worker: dequeue the assignment
        var workerAssignment = await coordinator.DequeueModuleAsync(
            new HashSet<string>(), CancellationToken.None);
        await Assert.That(workerAssignment).IsNotNull();

        // Simulate worker producing a serialized result
        var now = DateTimeOffset.UtcNow;
        var successResult = new ModuleResult<SimpleResult>.Success(new SimpleResult { Message = "A done" })
        {
            ModuleName = "ModuleA",
            ModuleTypeName = typeof(ModuleA).FullName,
            ModuleDuration = TimeSpan.FromSeconds(1),
            ModuleStart = now,
            ModuleEnd = now.AddSeconds(1),
            ModuleStatus = Status.Successful
        };

        var serialized = serializer.Serialize(
            successResult,
            typeof(ModuleA).FullName!,
            typeof(SimpleResult).FullName!,
            1);

        await coordinator.PublishResultAsync(serialized, CancellationToken.None);

        // Collector waits for result
        var result = await collector.WaitForResultAsync(typeof(ModuleA).FullName!, CancellationToken.None);

        await Assert.That(result).IsNotNull();
        await Assert.That(result!.IsSuccess).IsTrue();
        await Assert.That(result.ModuleName).IsEqualTo("ModuleA");
    }

    [Test]
    public async Task Cancellation_Propagates_Through_Coordinator()
    {
        var coordinator = new InMemoryDistributedCoordinator();
        using var cts = new CancellationTokenSource();

        // Start waiting for result that won't come
        var waitTask = coordinator.WaitForResultAsync("NonExistent.Module", cts.Token);

        // Cancel after a short delay
        cts.CancelAfter(100);

        // Should throw OperationCanceledException
        var threw = false;
        try
        {
            await waitTask;
        }
        catch (OperationCanceledException)
        {
            threw = true;
        }

        await Assert.That(threw).IsTrue();
    }

    [Test]
    public async Task Multiple_Modules_Published_And_Collected()
    {
        var coordinator = new InMemoryDistributedCoordinator();
        var registry = new ModuleTypeRegistry();
        registry.Register(typeof(ModuleA));
        registry.Register(typeof(ModuleB));
        registry.Register(typeof(ModuleC));
        var serializer = new ModuleResultSerializer(registry);
        var publisher = new DistributedWorkPublisher(coordinator, registry);
        var collector = new DistributedResultCollector(coordinator, serializer);

        // Publish all 3 modules
        await publisher.PublishAsync(publisher.CreateAssignment(typeof(ModuleA)), CancellationToken.None);
        await publisher.PublishAsync(publisher.CreateAssignment(typeof(ModuleB)), CancellationToken.None);
        await publisher.PublishAsync(publisher.CreateAssignment(typeof(ModuleC)), CancellationToken.None);

        // Simulate worker results for each
        var now = DateTimeOffset.UtcNow;

        var resultA = new ModuleResult<SimpleResult>.Success(new SimpleResult { Message = "A" })
        {
            ModuleName = "ModuleA",
            ModuleTypeName = typeof(ModuleA).FullName,
            ModuleDuration = TimeSpan.FromSeconds(1),
            ModuleStart = now,
            ModuleEnd = now.AddSeconds(1),
            ModuleStatus = Status.Successful
        };
        var serializedA = serializer.Serialize(resultA, typeof(ModuleA).FullName!, typeof(SimpleResult).FullName!, 1);
        await coordinator.PublishResultAsync(serializedA, CancellationToken.None);

        var resultB = new ModuleResult<string>.Success("B")
        {
            ModuleName = "ModuleB",
            ModuleTypeName = typeof(ModuleB).FullName,
            ModuleDuration = TimeSpan.FromSeconds(1),
            ModuleStart = now,
            ModuleEnd = now.AddSeconds(1),
            ModuleStatus = Status.Successful
        };
        var serializedB = serializer.Serialize(resultB, typeof(ModuleB).FullName!, typeof(string).FullName!, 1);
        await coordinator.PublishResultAsync(serializedB, CancellationToken.None);

        var resultC = new ModuleResult<int>.Success(42)
        {
            ModuleName = "ModuleC",
            ModuleTypeName = typeof(ModuleC).FullName,
            ModuleDuration = TimeSpan.FromSeconds(1),
            ModuleStart = now,
            ModuleEnd = now.AddSeconds(1),
            ModuleStatus = Status.Successful
        };
        var serializedC = serializer.Serialize(resultC, typeof(ModuleC).FullName!, typeof(int).FullName!, 1);
        await coordinator.PublishResultAsync(serializedC, CancellationToken.None);

        // Collect all 3
        var collectedA = await collector.WaitForResultAsync(typeof(ModuleA).FullName!, CancellationToken.None);
        var collectedB = await collector.WaitForResultAsync(typeof(ModuleB).FullName!, CancellationToken.None);
        var collectedC = await collector.WaitForResultAsync(typeof(ModuleC).FullName!, CancellationToken.None);

        await Assert.That(collectedA).IsNotNull();
        await Assert.That(collectedA!.IsSuccess).IsTrue();
        await Assert.That(collectedA.ModuleName).IsEqualTo("ModuleA");

        await Assert.That(collectedB).IsNotNull();
        await Assert.That(collectedB!.IsSuccess).IsTrue();
        await Assert.That(collectedB.ModuleName).IsEqualTo("ModuleB");

        await Assert.That(collectedC).IsNotNull();
        await Assert.That(collectedC!.IsSuccess).IsTrue();
        await Assert.That(collectedC.ModuleName).IsEqualTo("ModuleC");
    }
}
