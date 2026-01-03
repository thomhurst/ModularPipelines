using System.Diagnostics;
using ModularPipelines.Context;
using ModularPipelines.Exceptions;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Options;
using ModularPipelines.TestHelpers;
using ModularPipelines.TestHelpers.Assertions;

namespace ModularPipelines.UnitTests.Helpers;

public class CommandTests : TestBase
{
    private class CommandEchoModule : Module<CommandResult>
    {
        public override async Task<CommandResult?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            return await context.Command.ExecuteCommandLineTool(
                new GenericCommandLineToolOptions("pwsh")
                {
                    Arguments = ["-Command", "echo 'Foo bar!'"],
                },
                cancellationToken: cancellationToken);
        }
    }

    private class CommandEchoTimeoutModule : Module<string>
    {
        public override async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return TestConstants.TestString;
        }
    }

    [Test]
    public async Task Has_Not_Errored()
    {
        var moduleResult = await await RunModule<CommandEchoModule>();

        await ModuleResultAssertions.AssertSuccessWithValue(moduleResult);
    }

    [Test]
    public async Task Standard_Output_Equals_Foo_Bar()
    {
        var moduleResult = await await RunModule<CommandEchoModule>();

        await ModuleResultAssertions.AssertCommandOutput(moduleResult, TestConstants.TestString);
    }

    [Test]
    public async Task Standard_Output_Equals_Foo_Bar_With_Timeout()
    {
        var moduleResult = await await RunModule<CommandEchoTimeoutModule>();

        await Assert.That(moduleResult.Value!.Trim()).IsEqualTo(TestConstants.TestString);
    }
}
