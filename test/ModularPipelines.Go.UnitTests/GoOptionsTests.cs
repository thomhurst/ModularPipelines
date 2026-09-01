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

    [Test]
    public async Task List_Json_Renders_Bare_Value()
    {
        var builder = await GetService<ICommandLineBuilder>();

        var commandLine = builder.Build(new GoListOptions
        {
            Json = CliOptionValue.Bare,
        });

        await Assert.That(commandLine.ToString()).IsEqualTo("go list -json");
    }

    [Test]
    public async Task List_Json_Renders_Selected_Fields()
    {
        var builder = await GetService<ICommandLineBuilder>();

        var commandLine = builder.Build(new GoListOptions
        {
            Json = "ImportPath,Name",
        });

        await Assert.That(commandLine.ToString()).IsEqualTo("go list -json=ImportPath,Name");
    }
}
