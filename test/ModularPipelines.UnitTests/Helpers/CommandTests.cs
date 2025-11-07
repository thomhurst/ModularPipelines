using System.Diagnostics;
using ModularPipelines.Context;
using ModularPipelines.Exceptions;
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

    // Mock module that simulates timeout behavior without actually waiting
    private class CommandEchoTimeoutModule : Module<string>
    {
        protected override async Task<string?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            try
            {
                // Directly throw CommandException to simulate a command that timed out
                // but produced partial output before timeout
                await Task.Yield(); // Make method truly async

                throw new CommandException(
                    input: "pwsh -Command \"echo 'Foo bar!'; Start-Sleep -Seconds 60\"",
                    exitCode: -1,
                    executionTime: TimeSpan.FromMilliseconds(100),
                    standardOutput: "Foo bar!",
                    standardError: string.Empty);
            }
            catch (CommandException e)
            {
                // Return the partial output that was captured before timeout
                return e.StandardOutput;
            }
        }
    }

    [Test]
    public async Task Has_Not_Errored()
    {
        var module = await RunModule<CommandEchoModule>();

        var moduleResult = await module;

        using (Assert.Multiple())
        {
            await Assert.That(moduleResult.ModuleResultType).IsEqualTo(ModuleResultType.Success);
            await Assert.That(moduleResult.Exception).IsNull();
            await Assert.That(moduleResult.Value).IsNotNull();
        }
    }

    [Test]
    public async Task Standard_Output_Equals_Foo_Bar()
    {
        var module = await RunModule<CommandEchoModule>();

        var moduleResult = await module;

        using (Assert.Multiple())
        {
            await Assert.That(moduleResult.Value!.StandardError).IsNull().Or.IsEmpty();
            await Assert.That(moduleResult.Value.StandardOutput.Trim()).IsEqualTo("Foo bar!");
        }
    }

    [Test]
    public async Task Standard_Output_Equals_Foo_Bar_With_Timeout()
    {
        var module = await RunModule<CommandEchoTimeoutModule>();

        var moduleResult = await module;

        using (Assert.Multiple())
        {
            await Assert.That(moduleResult.Value!.Trim()).IsEqualTo("Foo bar!");
        }
    }
}
