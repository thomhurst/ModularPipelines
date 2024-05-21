using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Context;
using ModularPipelines.Engine;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;
using TUnit.Assertions.Extensions;
using EngineCancellationToken = ModularPipelines.Engine.EngineCancellationToken;

namespace ModularPipelines.UnitTests;

public class IgnoredFailureTests : TestBase
{
    private class IgnoredFailureModule : Module<CommandResult>
    {
        protected internal override Task<bool> ShouldIgnoreFailures(IPipelineContext context, Exception exception)
        {
            return Task.FromResult(true);
        }

        protected override async Task<CommandResult?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            throw new Exception();
        }
    }

    [Test]
    public async Task Has_Not_Thrown_Or_Cancelled_Pipeline()
    {
        var module = await RunModule<IgnoredFailureModule>();

        var serviceProvider = module.Context.Get<IServiceProvider>()!;
        var engineCancellationToken = serviceProvider.GetRequiredService<EngineCancellationToken>();

        var moduleResult = await module;
        
        await Assert.Multiple(() =>
        {
            Assert.That(moduleResult.ModuleResultType).Is.EqualTo(ModuleResultType.Failure);
            Assert.That(moduleResult.Exception).Is.Not.Null();
            Assert.That(engineCancellationToken.IsCancellationRequested).Is.False();
        });
    }
}