using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;
using TUnit.Assertions.AssertConditions.Throws;

namespace ModularPipelines.UnitTests;

public class SkippedModuleTests : TestBase
{
    private class SkippedModule : Module<CommandResult>
    {
        protected internal override Task<SkipDecision> ShouldSkip(IPipelineContext context)
        {
            return Task.FromResult(SkipDecision.Skip("Testing purposes"));
        }

        protected override async Task<CommandResult?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            throw new Exception();
        }
    }

    [Test]
    public async Task Skipped_Result_Is_As_Expected()
    {
        var module = await RunModule<SkippedModule>();

        var moduleResult = await module;
        
        using (Assert.Multiple())
        {
            await Assert.That(moduleResult.ModuleResultType).IsEqualTo(ModuleResultType.Skipped);
            await Assert.That(moduleResult.Exception).IsNull();
            await Assert.That(() => moduleResult.Value).ThrowsException();
        }
    }
}