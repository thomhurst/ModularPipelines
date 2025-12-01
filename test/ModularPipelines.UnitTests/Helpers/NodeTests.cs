using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Node.Extensions;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests.Helpers;

public class NodeTests : TestBase
{
    private class NodeVersionModule : IModule<CommandResult>
    {
        public async Task<CommandResult?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            return await context.Node().Version(cancellationToken: cancellationToken);
        }
    }

    [Test]
    public async Task Has_Not_Errored()
    {
        var moduleResult = await RunModuleWithResult<NodeVersionModule, CommandResult>();

        using (Assert.Multiple())
        {
            await Assert.That(moduleResult.ModuleResultType).IsEqualTo(ModuleResultType.Success);
            await Assert.That(moduleResult.Exception).IsNull();
            await Assert.That(moduleResult.Value).IsNotNull();
        }
    }

    [Test]
    public async Task Standard_Output_Is_Version_Number()
    {
        var moduleResult = await RunModuleWithResult<NodeVersionModule, CommandResult>();

        using (Assert.Multiple())
        {
            await Assert.That(moduleResult.Value!.StandardError).IsNull().Or.IsEmpty();
            await Assert.That(moduleResult.Value.StandardOutput).Matches("v\\d+");
        }
    }
}
