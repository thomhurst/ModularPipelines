using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Configuration;
using ModularPipelines.Context;
using ModularPipelines.Engine;
using ModularPipelines.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests;

public class SkippedModuleTests : TestBase
{
    private class SkippedModule : Module<CommandResult>
    {
        protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
            .WithSkipWhen(() => SkipDecision.Skip("Testing purposes"))
            .Build();

        public override async Task<CommandResult?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            throw new Exception("Should not reach here");
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
