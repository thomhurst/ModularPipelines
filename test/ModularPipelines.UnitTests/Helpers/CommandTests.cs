using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Options;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests.Helpers;

public class CommandTests : TestBase
{
    private class CommandEchoModule : Module<CommandResult>
    {
        protected override async Task<CommandResult?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            return await context.Command.ExecuteCommandLineTool(
                new CommandLineToolOptions(
                    "pwsh",
                    "-Command", "echo 'Foo bar!'"
                ),
                cancellationToken: cancellationToken);
        }
    }

    [Test]
    public async Task Has_Not_Errored()
    {
        var module = await RunModule<CommandEchoModule>();

        var moduleResult = await module;

        Assert.Multiple(() =>
        {
            Assert.That(moduleResult.ModuleResultType, Is.EqualTo(ModuleResultType.Success));
            Assert.That(moduleResult.Exception, Is.Null);
            Assert.That(moduleResult.Value, Is.Not.Null);
        });
    }

    [Test]
    public async Task Standard_Output_Equals_Foo_Bar()
    {
        var module = await RunModule<CommandEchoModule>();

        var moduleResult = await module;

        Assert.Multiple(() =>
        {
            Assert.That(moduleResult.Value!.StandardError, Is.Null.Or.Empty);
            Assert.That(moduleResult.Value.StandardOutput.Trim(), Is.EqualTo("Foo bar!"));
        });
    }
}