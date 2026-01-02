using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests.Modules;

public class TestModule1 : SimpleTestModule<IDictionary<string, object>?>
{
    /// <inheritdoc/>
    protected override IDictionary<string, object>? Result => null;
}