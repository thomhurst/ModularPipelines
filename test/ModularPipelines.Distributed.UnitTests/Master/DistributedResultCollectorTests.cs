using Moq;
using ModularPipelines.Distributed;
using ModularPipelines.Distributed.Master;
using ModularPipelines.Distributed.Serialization;
using ModularPipelines.Engine;
using ModularPipelines.Enums;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using OptionsFactory = Microsoft.Extensions.Options.Options;

namespace ModularPipelines.Distributed.UnitTests.Master;

public class DistributedResultCollectorTests
{
    private class TestResult
    {
        public string Value { get; set; } = string.Empty;
    }

    private class TestModule : Module<TestResult>
    {
        protected internal override Task<TestResult> ExecuteAsync(
            ModularPipelines.Context.IModuleContext context,
            CancellationToken cancellationToken)
        {
            return Task.FromResult<TestResult>(new TestResult());
        }
    }

    [Test]
    public async Task WaitForResult_Returns_Deserialized_Result()
    {
        var registry = new ModuleTypeRegistry();
        registry.Register(typeof(TestModule));
        var serializer = new ModuleResultSerializer(registry);

        // Create a serialized result by manually constructing the JSON
        // We need to build a SerializedModuleResult that the serializer can deserialize
        var now = DateTimeOffset.UtcNow;
        var successResult = new ModuleResult<TestResult>.Success(new TestResult { Value = "hello" })
        {
            Name = "TestModule",
            TypeName = typeof(TestModule).FullName,
            Duration = TimeSpan.FromSeconds(1),
            StartTime = now,
            EndTime = now.AddSeconds(1),
            Status = ModuleStatus.Succeeded
        };

        var serialized = serializer.Serialize(
            successResult, typeof(TestModule).FullName!, typeof(TestResult).FullName!, 1);

        var coordinatorMock = new Mock<IDistributedCoordinator>();
        coordinatorMock.Setup(c => c.WaitForResultAsync(typeof(TestModule).FullName!, It.IsAny<CancellationToken>()))
            .ReturnsAsync(serialized);

        var collector = new DistributedResultCollector(coordinatorMock.Object, serializer);

        var result = await collector.WaitForResultAsync(typeof(TestModule).FullName!, CancellationToken.None);

        await Assert.That(result).IsNotNull();
        await Assert.That(result!.Status).IsEqualTo(ModuleStatus.Succeeded);
        await Assert.That(result.Name).IsEqualTo("TestModule");
    }

    [Test]
    public async Task WaitForResult_Propagates_Cancellation()
    {
        var coordinatorMock = new Mock<IDistributedCoordinator>();
        coordinatorMock.Setup(c => c.WaitForResultAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Returns<string, CancellationToken>(async (_, ct) =>
            {
                await Task.Delay(Timeout.Infinite, ct);
                return null!;
            });

        var registry = new ModuleTypeRegistry();
        var serializer = new ModuleResultSerializer(registry);
        var collector = new DistributedResultCollector(coordinatorMock.Object, serializer);

        using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(100));

        var threw = false;
        try
        {
            await collector.WaitForResultAsync("Test.Module", cts.Token);
        }
        catch (OperationCanceledException)
        {
            threw = true;
        }

        await Assert.That(threw).IsTrue();
    }

    [Test]
    public async Task WaitForRemoteResult_AggregatesCommandCount()
    {
        var registry = new ModuleTypeRegistry();
        registry.Register(typeof(TestModule));
        var serializer = new ModuleResultSerializer(registry);
        var result = new ModuleResult<TestResult>.Success(new TestResult())
        {
            Name = nameof(TestModule),
            TypeName = typeof(TestModule).FullName,
            Duration = TimeSpan.Zero,
            StartTime = DateTimeOffset.UtcNow,
            EndTime = DateTimeOffset.UtcNow,
            Status = ModuleStatus.Succeeded,
        };
        var serialized = serializer.Serialize(
            result,
            typeof(TestModule).FullName!,
            typeof(TestResult).FullName!,
            workerIndex: 1) with
        {
            CommandCount = 4,
        };
        var coordinator = new Mock<IDistributedCoordinator>();
        coordinator.Setup(x => x.WaitForResultAsync(typeof(TestModule).FullName!, It.IsAny<CancellationToken>()))
            .ReturnsAsync(serialized);
        var commandExecutionCounter = new CommandExecutionCounter();
        var collector = new DistributedResultCollector(
            coordinator.Object,
            serializer,
            commandExecutionCounter,
            OptionsFactory.Create(new DistributedOptions { InstanceIndex = 0 }));

        var collected = await collector.WaitForResultAsync(
            typeof(TestModule).FullName!,
            CancellationToken.None);

        using (Assert.Multiple())
        {
            await Assert.That(commandExecutionCounter.TotalCount).IsEqualTo(4);
            await Assert.That(commandExecutionCounter.GetCount(typeof(TestModule))).IsEqualTo(4);
            await Assert.That(commandExecutionCounter.GetRemoteModuleCounts()[(1, typeof(TestModule))])
                .IsEqualTo(4);
            await Assert.That(collected!.TypeName).IsEqualTo(ModuleTypeIdentifier.Get(typeof(TestModule)));
        }
    }

    [Test]
    public async Task WaitForLocalResult_DoesNotDoubleCountCommands()
    {
        var registry = new ModuleTypeRegistry();
        registry.Register(typeof(TestModule));
        var serializer = new ModuleResultSerializer(registry);
        var result = new ModuleResult<TestResult>.Success(new TestResult())
        {
            Name = nameof(TestModule),
            TypeName = typeof(TestModule).FullName,
            Duration = TimeSpan.Zero,
            StartTime = DateTimeOffset.UtcNow,
            EndTime = DateTimeOffset.UtcNow,
            Status = ModuleStatus.Succeeded,
        };
        var serialized = serializer.Serialize(
            result,
            typeof(TestModule).FullName!,
            typeof(TestResult).FullName!,
            workerIndex: 0) with
        {
            CommandCount = 2,
        };
        var coordinator = new Mock<IDistributedCoordinator>();
        coordinator.Setup(x => x.WaitForResultAsync(typeof(TestModule).FullName!, It.IsAny<CancellationToken>()))
            .ReturnsAsync(serialized);
        var commandExecutionCounter = new CommandExecutionCounter();
        commandExecutionCounter.Add(typeof(TestModule), 2);
        var collector = new DistributedResultCollector(
            coordinator.Object,
            serializer,
            commandExecutionCounter,
            OptionsFactory.Create(new DistributedOptions { InstanceIndex = 0 }));

        await collector.WaitForResultAsync(typeof(TestModule).FullName!, CancellationToken.None);

        await Assert.That(commandExecutionCounter.TotalCount).IsEqualTo(2);
    }
}
