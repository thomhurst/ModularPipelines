using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Modules.Behaviors;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests;

public class SkippedModuleTests : TestBase
{
    private class SkippedModule : Module<CommandResult>, IModuleSkipLogic
    {
        public Task<SkipDecision> ShouldSkipAsync(IPipelineContext context)
        {
            return Task.FromResult(SkipDecision.Skip("Testing purposes"));
        }

        public override async Task<CommandResult?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            throw new Exception();
        }
    }

    [Test]
    public async Task Skipped_Result_Is_As_Expected()
    {
        var module = await RunModule<SkippedModule>();

        var moduleResult = (ModuleResult<CommandResult>)await module.GetModuleResult();

        using (Assert.Multiple())
        {
            await Assert.That(moduleResult.ModuleResultType).IsEqualTo(ModuleResultType.Skipped);
            await Assert.That(moduleResult.Exception).IsNull();
            await Assert.That(() => moduleResult.Value).ThrowsException();
        }
    }
}