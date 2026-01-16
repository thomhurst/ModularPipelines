using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Attributes;
using ModularPipelines.Configuration;
using ModularPipelines.Context;
using ModularPipelines.Engine;
using ModularPipelines.Extensions;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;
using Status = ModularPipelines.Enums.Status;

namespace ModularPipelines.UnitTests.Execution;

[TUnit.Core.NotInParallel(nameof(EngineCancellationTokenTests))]
public class EngineCancellationTokenTests : TestBase
{
    private static readonly TimeSpan WaitForCancellationDelay = TimeSpan.FromMilliseconds(100);

    private class BadModule : ThrowingTestModule<bool>
    {
    }

    [ModularPipelines.Attributes.DependsOn<BadModule>]
    private class Module1 : SimpleTestModule<bool>
    {
        protected override bool Result => true;
    }

    private class LongRunningModule : Module<bool>
    {
        private readonly TaskCompletionSource<bool> _taskCompletionSource = new();

        protected internal override async Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await _taskCompletionSource.Task.WaitAsync(cancellationToken);
            return true;
        }
    }

    private class LongRunningModuleWithoutCancellation : Module<bool>
    {
        private readonly TaskCompletionSource<bool> _taskCompletionSource = new();

        protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
            .WithTimeout(TimeSpan.FromSeconds(10))
            .Build();

        protected internal override async Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await _taskCompletionSource.Task;
            return true;
        }
    }

    [Test]
    public async Task When_Cancel_Engine_Token_With_DependsOn_Then_Modules_Cancel()
    {
        var builder = TestPipelineHostBuilder.Create()
            .AddModule<BadModule>()
            .AddModule<Module1>();

        // This test expects the pipeline to throw when BadModule fails
        builder.Options.ThrowOnPipelineFailure = true;

        var host = await builder.BuildHostAsync();

        var resultRegistry = host.RootServices.GetRequiredService<IModuleResultRegistry>();

        await Assert.That(async () => await host.ExecutePipelineAsync()).ThrowsException();

        // Allow time for all module results to be finalized after pipeline termination
        await Task.Delay(TimeSpan.FromSeconds(1));

        var module1Result = resultRegistry.GetResult(typeof(Module1));
        // Module1 depends on BadModule which failed, so Module1 should be marked as PipelineTerminated
        await Assert.That(module1Result).IsNotNull();
        await Assert.That(module1Result!.ModuleStatus).IsEqualTo(Status.PipelineTerminated);
    }

    [Test]
    public async Task When_Cancel_Engine_Token_Without_DependsOn_Then_Modules_Cancel()
    {
        var builder = TestPipelineHostBuilder.Create()
            .AddModule<BadModule>()
            .AddModule<LongRunningModule>();

        // This test expects the pipeline to throw when BadModule fails
        builder.Options.ThrowOnPipelineFailure = true;

        var host = await builder.BuildHostAsync();

        var resultRegistry = host.RootServices.GetRequiredService<IModuleResultRegistry>();

        var pipelineTask = host.ExecutePipelineAsync();

        await Task.Delay(WaitForCancellationDelay);

        await Assert.That(async () => await pipelineTask).ThrowsException();

        var longRunningModuleResult = resultRegistry.GetResult(typeof(LongRunningModule));
        await Assert.That(longRunningModuleResult).IsNotNull();
        await Assert.That(longRunningModuleResult!.ModuleStatus).IsEqualTo(Status.PipelineTerminated);
        await Assert.That(longRunningModuleResult.ModuleDuration).IsLessThan(TimeSpan.FromSeconds(5));
    }

    [Test]
    public async Task When_Cancel_Engine_Token_Without_DependsOn_Then_Modules_Cancel_Without_Cancellation()
    {
        var builder = TestPipelineHostBuilder.Create()
            .AddModule<BadModule>()
            .AddModule<LongRunningModuleWithoutCancellation>();

        // This test expects the pipeline to throw when BadModule fails
        builder.Options.ThrowOnPipelineFailure = true;

        var host = await builder.BuildHostAsync();

        var resultRegistry = host.RootServices.GetRequiredService<IModuleResultRegistry>();

        var pipelineTask = host.ExecutePipelineAsync();

        await Task.Delay(WaitForCancellationDelay);

        await Assert.That(async () => await pipelineTask).ThrowsException();

        var longRunningModuleResult = resultRegistry.GetResult(typeof(LongRunningModuleWithoutCancellation));
        await Assert.That(longRunningModuleResult).IsNotNull();
        await Assert.That(longRunningModuleResult!.ModuleStatus).IsEqualTo(Status.PipelineTerminated);
    }
}
