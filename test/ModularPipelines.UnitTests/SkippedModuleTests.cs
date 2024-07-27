using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;

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
        
        await using (Assert.Multiple())
        {
            await Assert.That(moduleResult.ModuleResultType).Is.EqualTo(ModuleResultType.Skipped);
            await Assert.That(moduleResult.Exception).Is.Null();
            await Assert.That(() => moduleResult.Value).Throws.Exception().OfAnyType();
        }
    }
}