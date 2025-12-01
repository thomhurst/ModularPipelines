using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Context;
using ModularPipelines.Engine;
using ModularPipelines.Exceptions;
using ModularPipelines.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;
using EngineCancellationToken = ModularPipelines.Engine.EngineCancellationToken;

namespace ModularPipelines.UnitTests;

public class NonIgnoredFailureTests : TestBase
{
    private class NonIgnoredFailureModule : IModule<CommandResult>
    {
        public async Task<CommandResult?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            throw new Exception();
        }
    }

    [Test]
    public async Task Has_Thrown_And_Cancelled_Pipeline()
    {
        var host = await TestPipelineHostBuilder.Create()
            .AddModule<NonIgnoredFailureModule>()
            .BuildHostAsync();

        await Assert.ThrowsAsync<ModuleFailedException>(async () => await host.ExecutePipelineAsync());

        var engineCancellationToken = host.RootServices.GetRequiredService<EngineCancellationToken>();

        // The cancellation should happen very quickly after module failure
        // Use a small polling loop with a reasonable timeout instead of a fixed 10-second delay
        var timeout = TimeSpan.FromSeconds(2);
        var pollInterval = TimeSpan.FromMilliseconds(10);
        var startTime = DateTimeOffset.UtcNow;

        while (!engineCancellationToken.IsCancellationRequested && DateTimeOffset.UtcNow - startTime < timeout)
        {
            await Task.Delay(pollInterval);
        }

        await Assert.That(engineCancellationToken.IsCancellationRequested).IsTrue();
    }
}
