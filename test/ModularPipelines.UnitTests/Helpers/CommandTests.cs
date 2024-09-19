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
    
    private class CommandEchoTimeoutModule : Module<string>
    {
        protected override async Task<string?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            try
            {
                using var cts = new CancellationTokenSource();
                cts.CancelAfter(TimeSpan.FromSeconds(30));
                
                return (await context.Command.ExecuteCommandLineTool(
                    new CommandLineToolOptions(
                        "pwsh",
                        "-Command", "echo 'Foo bar!'; Start-Sleep -Seconds 60"
                    ),
                    cancellationToken: cts.Token)).StandardOutput;
            }
            catch (CommandException e)
            {
                return e.StandardOutput;
            }
        }
    }

    [Test]
    public async Task Has_Not_Errored()
    {
        var module = await RunModule<CommandEchoModule>();

        var moduleResult = await module;
        
        await using (Assert.Multiple())
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
        
        await using (Assert.Multiple())
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
        
        await using (Assert.Multiple())
        {
            await Assert.That(moduleResult.Value!.Trim()).IsEqualTo("Foo bar!");
        }
    }
}
