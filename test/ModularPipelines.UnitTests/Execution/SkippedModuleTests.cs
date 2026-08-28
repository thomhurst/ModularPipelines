using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Configuration;
using ModularPipelines.Context;
using ModularPipelines.Engine;
using ModularPipelines.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests.Execution;

public class SkippedModuleTests : TestBase
{
    private class SkippedModule : Module<CommandResult>
    {
        protected override void Configure(ModuleConfigurationBuilder module) => module
            .WithSkipWhen(_ => SkipDecision.Skip("Testing purposes"));

        protected internal override async Task<CommandResult> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            throw new Exception("Should not reach here");
        }
    }

    [Test]
    public async Task Skipped_Result_Is_As_Expected()
    {
        var host = await TestPipelineBuilder.Create()
            .AddModule<SkippedModule>()
            .BuildAsync();

        await host.RunAsync();

        var resultRegistry = host.Services.GetRequiredService<IModuleResultRegistry>();
        var moduleResult = resultRegistry.GetResult(typeof(SkippedModule))!;

        using (Assert.Multiple())
        {
            await Assert.That(moduleResult.SkipDecisionOrDefault).IsNotNull();
            await Assert.That(moduleResult.ExceptionOrDefault).IsNull();
        }
    }
}
