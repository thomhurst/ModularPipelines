using ModularPipelines.DotNet.Options;
using static ModularPipelines.TestHelpers.OptionsRenderingTestHelper;

namespace ModularPipelines.DotNet.UnitTests;

public class DotNetBuildOptionsCompatibilityTests
{
    [Test]
    public async Task Nologo_Forwards_To_NoLogo_And_Renders_Stable_Switch()
    {
#pragma warning disable CS0618
        var options = new DotNetBuildOptions { Nologo = true };
#pragma warning restore CS0618

        var arguments = BuildArguments(options);

        await Assert.That(options.NoLogo).IsTrue();
        await Assert.That(arguments).IsEquivalentTo(["--nologo"], TUnit.Assertions.Enums.CollectionOrdering.Matching);
    }

    [Test]
    public async Task Debug_Is_Retained_But_Not_Rendered()
    {
#pragma warning disable CS0618
        var options = new DotNetBuildOptions { Debug = true };
#pragma warning restore CS0618

        var arguments = BuildArguments(options);

        await Assert.That(arguments).Count().IsEqualTo(0);
    }

    [Test]
    [Arguments("clean")]
    [Arguments("pack")]
    [Arguments("publish")]
    public async Task Sibling_Nologo_Properties_Forward_To_NoLogo(string command)
    {
#pragma warning disable CS0618
        object options = command switch
        {
            "clean" => new DotNetCleanOptions { Nologo = true },
            "pack" => new DotNetPackOptions { Nologo = true },
            "publish" => new DotNetPublishOptions { Nologo = true },
            _ => throw new ArgumentOutOfRangeException(nameof(command)),
        };
#pragma warning restore CS0618

        var arguments = BuildArguments(options);
        var noLogo = options switch
        {
            DotNetCleanOptions cleanOptions => cleanOptions.NoLogo,
            DotNetPackOptions packOptions => packOptions.NoLogo,
            DotNetPublishOptions publishOptions => publishOptions.NoLogo,
            _ => null,
        };

        await Assert.That(noLogo).IsTrue();
        await Assert.That(arguments).Contains("--nologo");
    }
}
