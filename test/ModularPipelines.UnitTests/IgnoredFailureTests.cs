using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Modules.Behaviors;
using ModularPipelines.TestHelpers;
using EngineCancellationToken = ModularPipelines.Engine.EngineCancellationToken;

namespace ModularPipelines.UnitTests;

public class IgnoredFailureTests : TestBase
{
    private class IgnoredFailureModule : Module<CommandResult>, IModuleErrorHandling
    {
        public Task<bool> ShouldIgnoreFailuresAsync(IPipelineContext context, Exception exception)
        {
            return Task.FromResult(true);
        }

        public override async Task<CommandResult?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            throw new Exception();
        }
    }

    [Test]
    public async Task Has_Not_Thrown_Or_Cancelled_Pipeline()
    {
        EngineCancellationToken? engineCancellationToken = null;

        var pipelineSummary = await TestPipelineHostBuilder.Create()
            .ConfigureServices((_, collection) =>
            {
                collection.AddSingleton<Action<IServiceProvider>>(sp =>
                    engineCancellationToken = sp.GetRequiredService<EngineCancellationToken>());
            })
            .AddModule<IgnoredFailureModule>()
            .ExecutePipelineAsync();

        var module = pipelineSummary.GetModule<IgnoredFailureModule>();
        var moduleResult = (ModuleResult<CommandResult>)await module.GetModuleResult();

        using (Assert.Multiple())
        {
            await Assert.That(moduleResult.ModuleResultType).IsEqualTo(ModuleResultType.Failure);
            await Assert.That(moduleResult.Exception).IsNotNull();
            await Assert.That(engineCancellationToken.IsCancellationRequested).IsFalse();
        }
    }
}