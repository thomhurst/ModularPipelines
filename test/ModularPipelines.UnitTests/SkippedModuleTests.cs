using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Context;
using ModularPipelines.Engine;
using ModularPipelines.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Modules.Behaviors;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests;

public class SkippedModuleTests : TestBase
{
    private class SkippedModule : ThrowingTestModule<CommandResult>, ISkippable
    {
        public Task<SkipDecision> ShouldSkip(IPipelineContext context)
        {
            return Task.FromResult(SkipDecision.Skip("Testing purposes"));
        }
    }

    [Test]
    public async Task Skipped_Result_Is_As_Expected()
    {
        var host = await TestPipelineHostBuilder.Create()
            .AddModule<SkippedModule>()
            .BuildHostAsync();

        await host.ExecutePipelineAsync();

        var resultRegistry = host.RootServices.GetRequiredService<IModuleResultRegistry>();
        var moduleResult = resultRegistry.GetResult(typeof(SkippedModule))!;

        using (Assert.Multiple())
        {
            await Assert.That(moduleResult.ModuleResultType).IsEqualTo(ModuleResultType.Skipped);
            await Assert.That(moduleResult.ExceptionOrDefault).IsNull();
        }
    }
}
