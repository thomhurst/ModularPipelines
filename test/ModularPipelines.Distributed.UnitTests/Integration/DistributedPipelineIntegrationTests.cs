using ModularPipelines.Distributed.Coordination;
using ModularPipelines.Distributed.Master;
using ModularPipelines.Distributed.Serialization;
using ModularPipelines.Engine;
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
        protected internal override Task<SimpleResult> ExecuteAsync(
            ModularPipelines.IModuleContext context,
            CancellationToken cancellationToken)
        {
            return Task.FromResult<SimpleResult>(new SimpleResult { Message = "A done" });
        }
    }

    private class ModuleB : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(
            ModularPipelines.IModuleContext context,
            CancellationToken cancellationToken)
        {
            return Task.FromResult<string>("B done");
        }
    }

    private class ModuleC : Module<int>
    {
        protected internal override Task<int> ExecuteAsync(
            ModularPipelines.IModuleContext context,
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
        var resultRegistry = new ModuleResultRegistry();
        var publisher = new DistributedWorkPublisher(coordinator, registry, serializer, resultRegistry);
        var collector = new DistributedResultCollector(coordinator, serializer);

        // Master publishes work
        var moduleA = new ModuleA();
        var assignment = publisher.CreateAssignment(moduleA);
        await publisher.PublishAsync(assignment, CancellationToken.None);

        // Simulate worker: dequeue the assignment
        var workerAssignment = await coordinator.DequeueModuleAsync(
            new HashSet<Capability>(), CancellationToken.None);
        await Assert.That(workerAssignment).IsNotNull();

        // Simulate worker producing a serialized result
        var now = DateTimeOffset.UtcNow;
        var successResult = new ModuleResult<SimpleResult>.Success(new SimpleResult { Message = "A done" })
        {
            Name = "ModuleA",
            TypeName = typeof(ModuleA).FullName,
            Duration = TimeSpan.FromSeconds(1),
            StartTime = now,
            EndTime = now.AddSeconds(1),
            Status = ModuleStatus.Succeeded
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
        await Assert.That(result!.Status).IsEqualTo(ModuleStatus.Succeeded);
        await Assert.That(result.Name).IsEqualTo("ModuleA");
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
        var resultRegistry = new ModuleResultRegistry();
        var publisher = new DistributedWorkPublisher(coordinator, registry, serializer, resultRegistry);
        var collector = new DistributedResultCollector(coordinator, serializer);

        // Publish all 3 modules
        var moduleA = new ModuleA();
        var moduleB = new ModuleB();
        var moduleC = new ModuleC();
        await publisher.PublishAsync(publisher.CreateAssignment(moduleA), CancellationToken.None);
        await publisher.PublishAsync(publisher.CreateAssignment(moduleB), CancellationToken.None);
        await publisher.PublishAsync(publisher.CreateAssignment(moduleC), CancellationToken.None);

        // Simulate worker results for each
        var now = DateTimeOffset.UtcNow;

        var resultA = new ModuleResult<SimpleResult>.Success(new SimpleResult { Message = "A" })
        {
            Name = "ModuleA",
            TypeName = typeof(ModuleA).FullName,
            Duration = TimeSpan.FromSeconds(1),
            StartTime = now,
            EndTime = now.AddSeconds(1),
            Status = ModuleStatus.Succeeded
        };
        var serializedA = serializer.Serialize(resultA, typeof(ModuleA).FullName!, typeof(SimpleResult).FullName!, 1);
        await coordinator.PublishResultAsync(serializedA, CancellationToken.None);

        var resultB = new ModuleResult<string>.Success("B")
        {
            Name = "ModuleB",
            TypeName = typeof(ModuleB).FullName,
            Duration = TimeSpan.FromSeconds(1),
            StartTime = now,
            EndTime = now.AddSeconds(1),
            Status = ModuleStatus.Succeeded
        };
        var serializedB = serializer.Serialize(resultB, typeof(ModuleB).FullName!, typeof(string).FullName!, 1);
        await coordinator.PublishResultAsync(serializedB, CancellationToken.None);

        var resultC = new ModuleResult<int>.Success(42)
        {
            Name = "ModuleC",
            TypeName = typeof(ModuleC).FullName,
            Duration = TimeSpan.FromSeconds(1),
            StartTime = now,
            EndTime = now.AddSeconds(1),
            Status = ModuleStatus.Succeeded
        };
        var serializedC = serializer.Serialize(resultC, typeof(ModuleC).FullName!, typeof(int).FullName!, 1);
        await coordinator.PublishResultAsync(serializedC, CancellationToken.None);

        // Collect all 3
        var collectedA = await collector.WaitForResultAsync(typeof(ModuleA).FullName!, CancellationToken.None);
        var collectedB = await collector.WaitForResultAsync(typeof(ModuleB).FullName!, CancellationToken.None);
        var collectedC = await collector.WaitForResultAsync(typeof(ModuleC).FullName!, CancellationToken.None);

        await Assert.That(collectedA).IsNotNull();
        await Assert.That(collectedA!.Status).IsEqualTo(ModuleStatus.Succeeded);
        await Assert.That(collectedA.Name).IsEqualTo("ModuleA");

        await Assert.That(collectedB).IsNotNull();
        await Assert.That(collectedB!.Status).IsEqualTo(ModuleStatus.Succeeded);
        await Assert.That(collectedB.Name).IsEqualTo("ModuleB");

        await Assert.That(collectedC).IsNotNull();
        await Assert.That(collectedC!.Status).IsEqualTo(ModuleStatus.Succeeded);
        await Assert.That(collectedC.Name).IsEqualTo("ModuleC");
    }
}
