using ModularPipelines.Context;
using ModularPipelines.Exceptions;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;
using TUnit.Assertions;
using TUnit.Core;

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
        Assert.That(async () => await RunModule<ModuleReferencingSelf>())
            .Throws.TypeOf<ModuleFailedException>();
        Assert.That(exception!.InnerException).Is.TypeOf<ModuleReferencingSelfException>();
    }
}