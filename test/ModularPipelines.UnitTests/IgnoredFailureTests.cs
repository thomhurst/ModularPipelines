using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Context;
using ModularPipelines.Engine;
using ModularPipelines.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Modules.Behaviors;
using ModularPipelines.TestHelpers;
using EngineCancellationToken = ModularPipelines.Engine.EngineCancellationToken;

namespace ModularPipelines.UnitTests;

public class IgnoredFailureTests : TestBase
{
    private class IgnoredFailureModule : IModule<CommandResult>, IIgnoreFailures
    {
        public Task<bool> ShouldIgnoreFailures(IPipelineContext context, Exception exception)
        {
            return Task.FromResult(true);
        }

        public async Task<CommandResult?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            throw new Exception();
        }
    }

    [Test]
    public async Task Has_Not_Thrown_Or_Cancelled_Pipeline()
    {
        var host = await TestPipelineHostBuilder.Create()
            .AddModule<IgnoredFailureModule>()
            .BuildHostAsync();

        await host.ExecutePipelineAsync();

        var resultRegistry = host.RootServices.GetRequiredService<IModuleResultRegistry>();
        var moduleResult = resultRegistry.GetResult<CommandResult>(typeof(IgnoredFailureModule))!;

        var engineCancellationToken = host.RootServices.GetRequiredService<EngineCancellationToken>();

        using (Assert.Multiple())
        {
            await Assert.That(moduleResult.ModuleResultType).IsEqualTo(ModuleResultType.Failure);
            await Assert.That(moduleResult.Exception).IsNotNull();
            await Assert.That(engineCancellationToken.IsCancellationRequested).IsFalse();
        }
    }
}
