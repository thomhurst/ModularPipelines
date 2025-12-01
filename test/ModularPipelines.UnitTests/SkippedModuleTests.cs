using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Context;
using ModularPipelines.Engine;
using ModularPipelines.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Modules.Behaviors;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests;

public class SkippedModuleTests : TestBase
{
    private class SkippedModule : IModule<CommandResult>, ISkippable
    {
        public Task<SkipDecision> ShouldSkip(IPipelineContext context)
        {
            return Task.FromResult(SkipDecision.Skip("Testing purposes"));
        }

        public async Task<CommandResult?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            throw new Exception();
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
            await Assert.That(moduleResult.Exception).IsNull();
        }
    }
}
