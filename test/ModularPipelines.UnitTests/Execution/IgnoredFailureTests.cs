using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Configuration;
using ModularPipelines.Context;
using ModularPipelines.Engine;
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
        protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
            .WithIgnoreFailures()
            .Build();

        protected internal override async Task<CommandResult?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
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
            await Assert.That(moduleResult.ExceptionOrDefault).IsNotNull();
            await Assert.That(engineCancellationToken.IsCancellationRequested).IsFalse();
        }
    }
}
