using ModularPipelines.Engine;

namespace ModularPipelines.UnitTests.Engine;

public class ModuleCompletionLogStateTests
{
    [Test]
    public async Task CompletionLog_PreservesStructuredDurationValues()
    {
        var duration = TimeSpan.FromMilliseconds(23_737);
        var state = new ModuleCompletionLogState("ExampleModule", duration, "(none)", 1, 2);

        var properties = state.ToDictionary(x => x.Key, x => x.Value);

        await Assert.That(state.Message).IsEqualTo(
            "Module ExampleModule completed after 23s & 737ms with lock keys: (none) (Active: Q=1, E=2)");
        await Assert.That(properties["Duration"]).IsTypeOf<TimeSpan>();
        await Assert.That(properties["Duration"]).IsEqualTo(duration);
        await Assert.That(properties["ExecutionTime"]).IsTypeOf<double>();
        await Assert.That(properties["ExecutionTime"]).IsEqualTo(duration.TotalMilliseconds);
    }
}
