using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests;

public class GlobalDummyModule : SimpleTestModule<IDictionary<string, object>?>
{
    /// <inheritdoc/>
    protected override IDictionary<string, object>? Result => null;
}