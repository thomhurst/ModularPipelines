using ModularPipelines.Context;
using ModularPipelines.Go.Options;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.Go.UnitTests;

public class GoGlobalOptionsTests : TestBase
{
    [Test]
    public async Task Build_Places_Directory_Before_Command_And_Local_Flags()
    {
        var builder = await GetService<ICommandLineBuilder>();
        var command = builder.Build(new GoBuildOptions
        {
            WorkingDirectory = "source folder",
            Race = true,
            Packages = ["./..."],
        });

        await Assert.That(command.Tool).IsEqualTo("go");
        await Assert.That(command.Arguments.SequenceEqual(["-C", "source folder", "build", "-race", "./..."])).IsTrue();
        await Assert.That(typeof(GoBuildOptions).GetProperty(nameof(GoOptions.WorkingDirectory))!.DeclaringType)
            .IsEqualTo(typeof(GoOptions));
    }

    [Test]
    public async Task Nested_Command_Inherits_Directory()
    {
        var command = await RenderCommand(new GoModGraphOptions
        {
            WorkingDirectory = "module",
            X = true,
        });

        await Assert.That(command).IsEqualTo("go -C module mod graph -x");
    }

    [Test]
    public async Task Test_Preserves_Compile_Flag_And_Terminal_Payload()
    {
        var builder = await GetService<ICommandLineBuilder>();
        var command = builder.Build(new GoTestOptions
        {
            WorkingDirectory = "module",
            LowerC = true,
            Packages = ["./pkg"],
            Args = ["-C", "payload directory"],
        });

        await Assert.That(command.Arguments.SequenceEqual(
            ["-C", "module", "test", "-c", "./pkg", "-args", "-C", "payload directory"])).IsTrue();
    }

    [Test]
    public async Task Version_Command_Inherits_Directory()
    {
        var command = await RenderCommand(new GoVersionOptions { WorkingDirectory = "module" });

        await Assert.That(command).IsEqualTo("go -C module version");
    }

    [Test]
    public async Task Root_Places_Directory_Before_Arguments()
    {
        var builder = await GetService<ICommandLineBuilder>();
        var command = builder.Build(new RootGoOptions
        {
            WorkingDirectory = "source folder",
            Arguments = ["help"],
        });

        await Assert.That(command.Arguments.SequenceEqual(["-C", "source folder", "help"])).IsTrue();
    }

    [Test]
    public async Task Commands_Inherit_One_Directory_Option_Without_Local_Duplicates()
    {
        var commandTypes = typeof(GoOptions).Assembly.GetTypes()
            .Where(type => type.IsSubclassOf(typeof(GoOptions)));

        foreach (var commandType in commandTypes)
        {
            await Assert.That(commandType.GetProperty(nameof(GoOptions.WorkingDirectory))!.DeclaringType)
                .IsEqualTo(typeof(GoOptions));
            await Assert.That(commandType.GetProperty("UpperC")).IsNull();
            await Assert.That(commandType.GetProperty("C")).IsNull();
        }
    }

    private sealed record RootGoOptions : GoOptions;
}
