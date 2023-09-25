using ModularPipelines.Context;
using ModularPipelines.Exceptions;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.UnitTests;

public class ModuleReferencingSelfTests : TestBase
{
    private class ModuleReferencingSelf : Module<CommandResult>
    {
        protected override async Task<CommandResult?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await GetModule<ModuleReferencingSelf>();
            return null;
        }
    }

    [Test]
    public void Throws_Exception()
    {
        var exception = Assert.ThrowsAsync<ModuleFailedException>(async () => await RunModule<ModuleReferencingSelf>());
        Assert.That(exception!.InnerException, Is.TypeOf<ModuleReferencingSelfException>());
    }
}