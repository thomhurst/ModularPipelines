using ModularPipelines.Context;
using ModularPipelines.Exceptions;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests;

public class ModuleReferencingSelfTests : TestBase
{
    private class ModuleReferencingSelf : Module<CommandResult>
    {
        public override async Task<CommandResult?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await context.GetModuleAsync<ModuleReferencingSelf>();
            return null;
        }
    }

    [Test]
    public async Task Throws_Exception()
    {
        var exception = await Assert.ThrowsAsync<ModuleFailedException>(async () => await RunModule<ModuleReferencingSelf>());
        await Assert.That(exception.InnerException).IsTypeOf<ModuleReferencingSelfException>();
    }
}