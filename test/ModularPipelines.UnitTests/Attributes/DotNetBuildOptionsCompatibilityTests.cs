using ModularPipelines.DotNet.Options;
using ModularPipelines.Helpers.Internal;

namespace ModularPipelines.UnitTests.Attributes;

public class DotNetBuildOptionsCompatibilityTests
{
    private readonly CommandModelProvider _modelProvider = new();
    private readonly CommandArgumentBuilder _argumentBuilder = new();

    [Test]
    public async Task Nologo_Forwards_To_NoLogo_And_Renders_Stable_Switch()
    {
#pragma warning disable CS0618
        var options = new DotNetBuildOptions { Nologo = true };
#pragma warning restore CS0618

        var arguments = BuildArguments(options);

        await Assert.That(options.NoLogo).IsTrue();
        await Assert.That(arguments).IsEquivalentTo(["--nologo"]);
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

    private List<string> BuildArguments(DotNetBuildOptions options)
    {
        var model = _modelProvider.GetCommandModel(typeof(DotNetBuildOptions));
        return _argumentBuilder.BuildArguments(model, options);
    }
}
