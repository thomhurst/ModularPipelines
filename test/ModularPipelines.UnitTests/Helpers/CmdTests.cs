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
        
        await using (Assert.Multiple())
        {
            await Assert.That(moduleResult.ModuleResultType).Is.EqualTo(ModuleResultType.Success);
            await Assert.That(moduleResult.Exception).Is.Null();
            await Assert.That(moduleResult.Value).Is.Not.Null();
        }
    }

    [Test]
    public async Task Standard_Output_Equals_Foo_Bar()
    {
        var module = await RunModule<CmdEchoModule>();

        var moduleResult = await module;
        
        await using (Assert.Multiple())
        {
            await Assert.That(moduleResult.Value!.StandardError).Is.Null().Or.Is.Empty();
            await Assert.That(moduleResult.Value.StandardOutput.Trim()).Is.EqualTo("Foo bar!");
        }
    }
}