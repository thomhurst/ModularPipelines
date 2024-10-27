using ModularPipelines.Cmd.Extensions;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;
using ModularPipelines.UnitTests.Attributes;

namespace ModularPipelines.UnitTests.Helpers;

[WindowsOnlyTest]
public class CmdTests : TestBase
{
    private class CmdEchoModule : Module<CommandResult>
    {
        protected override async Task<CommandResult?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            return await context.Cmd().Script(new("echo Foo bar!"), cancellationToken: cancellationToken);
        }
    }

    [Test]
    public async Task Has_Not_Errored()
    {
        var module = await RunModule<CmdEchoModule>();

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
        var module = await RunModule<CmdEchoModule>();

        var moduleResult = await module;
        
        using (Assert.Multiple())
        {
            await Assert.That(moduleResult.Value!.StandardError).IsNull().Or.IsEmpty();
            await Assert.That(moduleResult.Value.StandardOutput.Trim()).IsEqualTo("Foo bar!");
        }
    }
}