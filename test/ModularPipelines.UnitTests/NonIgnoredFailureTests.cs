using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Context;
using ModularPipelines.Engine;
using ModularPipelines.Exceptions;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;
using TUnit.Assertions.Extensions;
using EngineCancellationToken = ModularPipelines.Engine.EngineCancellationToken;

namespace ModularPipelines.UnitTests;

public class NonIgnoredFailureTests : TestBase
{
    private class NonIgnoredFailureModule : Module<CommandResult>
    {
        protected override async Task<CommandResult?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
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

        await Task.Delay(TimeSpan.FromSeconds(2));
        await Assert.That(engineCancellationToken.IsCancellationRequested).Is.True();
    }
}