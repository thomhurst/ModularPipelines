using Moq;
using ModularPipelines.Distributed;
using ModularPipelines.Distributed.Master;
using ModularPipelines.Distributed.Serialization;
using ModularPipelines.Enums;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Distributed.UnitTests.Master;

public class DistributedResultCollectorTests
{
    private class TestResult
    {
        public string Value { get; set; } = string.Empty;
    }

    private class TestModule : Module<TestResult>
    {
        protected internal override Task<TestResult?> ExecuteAsync(
            ModularPipelines.Context.IModuleContext context,
            CancellationToken cancellationToken)
        {
            return Task.FromResult<TestResult?>(new TestResult());
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
            ModuleName = "TestModule",
            ModuleTypeName = typeof(TestModule).FullName,
            ModuleDuration = TimeSpan.FromSeconds(1),
            ModuleStart = now,
            ModuleEnd = now.AddSeconds(1),
            ModuleStatus = Status.Successful
        };

        var serialized = serializer.Serialize(
            successResult, typeof(TestModule).FullName!, typeof(TestResult).FullName!, 1);

        var coordinatorMock = new Mock<IDistributedCoordinator>();
        coordinatorMock.Setup(c => c.WaitForResultAsync(typeof(TestModule).FullName!, It.IsAny<CancellationToken>()))
            .ReturnsAsync(serialized);

        var collector = new DistributedResultCollector(coordinatorMock.Object, serializer);

        var result = await collector.WaitForResultAsync(typeof(TestModule).FullName!, CancellationToken.None);

        await Assert.That(result).IsNotNull();
        await Assert.That(result!.IsSuccess).IsTrue();
        await Assert.That(result.ModuleName).IsEqualTo("TestModule");
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
}
