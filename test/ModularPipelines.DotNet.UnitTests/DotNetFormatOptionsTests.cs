using ModularPipelines.DotNet.Options;
using static ModularPipelines.TestHelpers.OptionsRenderingTestHelper;

namespace ModularPipelines.DotNet.UnitTests;

public class DotNetFormatOptionsTests
{
    [Test]
    public async Task ExcludeDiagnostics_Passes_Each_Id_Separately()
    {
        var options = new DotNetFormatOptions
        {
            ExcludeDiagnostics = ["CS0246", "CS1503"],
        };

        var args = BuildArguments(options);

        await Assert.That(args).IsEquivalentTo(new[]
        {
            "--exclude-diagnostics", "CS0246",
            "--exclude-diagnostics", "CS1503",
        }, TUnit.Assertions.Enums.CollectionOrdering.Matching);
    }
}
