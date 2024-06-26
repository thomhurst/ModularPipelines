using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Node.Extensions;
using ModularPipelines.TestHelpers;
using TUnit.Assertions.Extensions;

namespace ModularPipelines.UnitTests.Helpers;

public class NodeTests : TestBase
{
    private class NodeVersionModule : Module<CommandResult>
    {
        protected override async Task<CommandResult?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            return await context.Node().Version(cancellationToken: cancellationToken);
        }
    }

    [Test]
    public async Task Has_Not_Errored()
    {
        var module = await RunModule<NodeVersionModule>();

        var moduleResult = await module;
        
        await using (Assert.Multiple())
        {
            await Assert.That(moduleResult.ModuleResultType).Is.EqualTo(ModuleResultType.Success);
            await Assert.That(moduleResult.Exception).Is.Null();
            await Assert.That(moduleResult.Value).Is.Not.Null();
        }
    }

    [Test]
    public async Task Standard_Output_Is_Version_Number()
    {
        var module = await RunModule<NodeVersionModule>();

        var moduleResult = await module;

        await using (Assert.Multiple())
        {
            await Assert.That(moduleResult.Value!.StandardError).Is.Null().Or.Is.Empty();
            await Assert.That(moduleResult.Value.StandardOutput).Does.Match("v\\d+");
        }
    }
}