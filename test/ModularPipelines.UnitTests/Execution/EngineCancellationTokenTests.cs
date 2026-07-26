using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Attributes;
using ModularPipelines.Configuration;
using ModularPipelines.Context;
using ModularPipelines.Engine;
using ModularPipelines.Exceptions;
using ModularPipelines.Extensions;
using ModularPipelines.Modules;
using ModularPipelines.Options;
using ModularPipelines.TestHelpers;
using Status = ModularPipelines.Enums.Status;

namespace ModularPipelines.UnitTests.Execution;

[TUnit.Core.NotInParallel(nameof(EngineCancellationTokenTests))]
public class EngineCancellationTokenTests : TestBase
{
    private static readonly TimeSpan WaitForCancellationDelay = TimeSpan.FromMilliseconds(100);
    private static TaskCompletionSource PeerModuleStarted = new(TaskCreationOptions.RunContinuationsAsynchronously);

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

    private class WaitForAllFailingModule : Module<bool>
    {
        protected internal override async Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await PeerModuleStarted.Task.WaitAsync(cancellationToken);
            throw new InvalidOperationException("Expected test failure");
        }
    }

    private class WaitForAllCompletingModule : Module<bool>
    {
        protected internal override async Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            PeerModuleStarted.TrySetResult();
            await Task.Delay(WaitForCancellationDelay, cancellationToken);
            return true;
        }
    }

    [ModularPipelines.Attributes.DependsOn<WaitForAllCompletingModule>]
    private class WaitForAllPendingModule : Module<bool>
    {
        protected internal override Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            return Task.FromResult(true);
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

        // Results should be registered before the exception is thrown, no delay needed
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

    [Test]
    public async Task WaitForAllModules_Allows_InFlight_And_Pending_Modules_To_Complete()
    {
        PeerModuleStarted = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);

        var builder = TestPipelineHostBuilder.Create()
            .ConfigurePipelineOptions((_, options) => options.ExecutionMode = ExecutionMode.WaitForAllModules)
            .AddModule<WaitForAllFailingModule>()
            .AddModule<WaitForAllCompletingModule>()
            .AddModule<WaitForAllPendingModule>();

        var host = await builder.BuildHostAsync();
        var resultRegistry = host.RootServices.GetRequiredService<IModuleResultRegistry>();

        var pipelineSummary = await host.ExecutePipelineAsync();

        var completingModuleResult = resultRegistry.GetResult(typeof(WaitForAllCompletingModule));
        var pendingModuleResult = resultRegistry.GetResult(typeof(WaitForAllPendingModule));
        await Assert.That(pipelineSummary.Status).IsEqualTo(Status.Failed);
        await Assert.That(completingModuleResult).IsNotNull();
        await Assert.That(completingModuleResult!.ModuleStatus).IsEqualTo(Status.Successful);
        await Assert.That(pendingModuleResult).IsNotNull();
        await Assert.That(pendingModuleResult!.ModuleStatus).IsEqualTo(Status.Successful);
    }

    [Test]
    public async Task WaitForAllModules_Preserves_Original_Failure()
    {
        PeerModuleStarted = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);

        var builder = TestPipelineHostBuilder.Create()
            .ConfigurePipelineOptions((_, options) =>
            {
                options.ExecutionMode = ExecutionMode.WaitForAllModules;
                options.ThrowOnPipelineFailure = true;
            })
            .AddModule<WaitForAllFailingModule>()
            .AddModule<WaitForAllCompletingModule>();

        var exception = await Assert.ThrowsAsync<ModuleFailedException>(
            async () => await builder.ExecutePipelineAsync());

        await Assert.That(exception!.InnerException).IsTypeOf<InvalidOperationException>();
        await Assert.That(exception.InnerException!).HasMessageEqualTo("Expected test failure");
    }
}
