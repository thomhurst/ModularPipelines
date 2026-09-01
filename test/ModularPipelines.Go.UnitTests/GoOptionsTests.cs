using ModularPipelines.Context;
using ModularPipelines.Go.Options;
using ModularPipelines.Models;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.Go.UnitTests;

public class GoOptionsTests : TestBase
{
    [Test]
    public async Task Get_U_Renders_Bare_Value()
    {
        var builder = await GetService<ICommandLineBuilder>();

        var commandLine = builder.Build(new GoGetOptions
        {
            U = CliOptionValue.Bare,
        });

        await Assert.That(commandLine.ToString()).IsEqualTo("go get -u");
    }

    [Test]
    public async Task Get_U_Renders_Patch_Value()
    {
        var builder = await GetService<ICommandLineBuilder>();

        var commandLine = builder.Build(new GoGetOptions
        {
            U = "patch",
        });

        await Assert.That(commandLine.ToString()).IsEqualTo("go get -u=patch");
    }

    [Test]
    public async Task Test_Args_Renders_Payload_After_Packages()
    {
        var builder = await GetService<ICommandLineBuilder>();

        var commandLine = builder.Build(new GoTestOptions
        {
            Packages = ["./pkg"],
            Args = ["payload", "-test.v"],
        });

        await Assert.That(commandLine.ToString()).IsEqualTo("go test ./pkg -args payload -test.v");
    }
}
