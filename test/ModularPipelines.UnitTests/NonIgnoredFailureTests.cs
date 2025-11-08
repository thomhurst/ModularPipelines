using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Context;
using ModularPipelines.Exceptions;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;
using EngineCancellationToken = ModularPipelines.Engine.EngineCancellationToken;

namespace ModularPipelines.UnitTests;

public class NonIgnoredFailureTests : TestBase
{
    private class NonIgnoredFailureModule : ModuleNew<CommandResult>
    {
        public override async Task<CommandResult?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            throw new Exception();
        }
    }

    [Test]
    public async Task Has_Thrown_And_Cancelled_Pipeline()
    {
        var exception = await Assert.ThrowsAsync<ModuleFailedException>(async () => await RunModule<NonIgnoredFailureModule>());

        var serviceProvider = exception.Module.Context.Get<IServiceProvider>()!;
        var engineCancellationToken = serviceProvider.GetRequiredService<EngineCancellationToken>();

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