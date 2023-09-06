using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Context;
using ModularPipelines.Engine;
using ModularPipelines.Exceptions;
using ModularPipelines.Models;
using ModularPipelines.Modules;

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
    public void Has_Thrown_And_Cancelled_Pipeline()
    {
        var exception = Assert.ThrowsAsync<ModuleFailedException>(async () => await RunModule<NonIgnoredFailureModule>());

        var serviceProvider = exception!.Module.Context.Get<IServiceProvider>()!;
        var engineCancellationToken = serviceProvider.GetRequiredService<EngineCancellationToken>();
        
        Assert.That(engineCancellationToken.IsCancellationRequested, Is.True);
    }
}