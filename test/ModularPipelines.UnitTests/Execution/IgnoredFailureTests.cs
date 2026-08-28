using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Configuration;
using ModularPipelines.Context;
using ModularPipelines.Engine;
using ModularPipelines.Enums;
using ModularPipelines.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;
using EngineCancellationToken = ModularPipelines.Engine.EngineCancellationToken;

namespace ModularPipelines.UnitTests.Execution;

public class IgnoredFailureTests : TestBase
{
    private class IgnoredFailureModule : Module<CommandResult>
    {
        protected override void Configure(ModuleConfigurationBuilder module) => module
            .WithIgnoreFailures();

        protected internal override async Task<CommandResult> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            throw new Exception();
        }
    }

    [Test]
    public async Task Has_Not_Thrown_Or_Cancelled_Pipeline()
    {
        var host = await TestPipelineBuilder.Create()
            .AddModule<IgnoredFailureModule>()
            .BuildAsync();

        var pipelineSummary = await host.RunAsync();

        var resultRegistry = host.Services.GetRequiredService<IModuleResultRegistry>();
        var moduleResult = resultRegistry.GetResult<CommandResult>(typeof(IgnoredFailureModule))!;

        var engineCancellationToken = host.Services.GetRequiredService<EngineCancellationToken>();

        using (Assert.Multiple())
        {
            await Assert.That(moduleResult.Status).IsEqualTo(ModuleStatus.FailureIgnored);
            await Assert.That(moduleResult.ExceptionOrDefault).IsNotNull();
            await Assert.That(engineCancellationToken.IsCancellationRequested).IsFalse();
            await Assert.That(pipelineSummary.Results).Count().IsEqualTo(1);
            await Assert.That(pipelineSummary.Status).IsEqualTo(ModuleStatus.Succeeded);
        }
    }
}
