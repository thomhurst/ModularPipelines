using ModularPipelines.Helm.Options;
using static ModularPipelines.TestHelpers.OptionsRenderingTestHelper;

namespace ModularPipelines.Helm.UnitTests;

public class VersionOptionsTests
{
    [Test]
    public async Task Short_Version_Renders_Flag()
    {
        var arguments = BuildArguments(new HelmVersionOptions { Short = true });

        await AssertArguments(arguments, ["--short"]);
    }
}
