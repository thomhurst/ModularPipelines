using ModularPipelines.Go.Options;
using ModularPipelines.Models;
using static ModularPipelines.TestHelpers.OptionsRenderingTestHelper;

namespace ModularPipelines.Go.UnitTests;

public class GoOptionsTests
{
    [Test]
    public async Task Optional_Value_Options_Render_Bare_And_Explicit_Forms()
    {
        await AssertArguments(
            BuildArguments(new GoGetOptions { U = CliOptionValue.Bare }),
            ["-u"]);
        await AssertArguments(
            BuildArguments(new GoGetOptions { U = "patch" }),
            ["-u=patch"]);
        await AssertArguments(
            BuildArguments(new GoListOptions { Json = CliOptionValue.Bare }),
            ["-json"]);
        await AssertArguments(
            BuildArguments(new GoListOptions { Json = "Name,Dir" }),
            ["-json=Name,Dir"]);
    }
}
