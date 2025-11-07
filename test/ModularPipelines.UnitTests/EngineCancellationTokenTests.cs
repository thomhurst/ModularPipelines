using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Extensions;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;
using Status = ModularPipelines.Enums.Status;

namespace ModularPipelines.UnitTests;

public class EngineCancellationTokenTests : TestBase
{
    // Using TaskCompletionSource for instant test execution while properly testing cancellation behavior
    private static readonly TimeSpan WaitForCancellationDelay = TimeSpan.FromMilliseconds(100);

    private class BadModule : Module
    {
        protected override async Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();

            // Exception will cause the engine to cancel the engine token
            throw new Exception();
        }
    }

    [ModularPipelines.Attributes.DependsOn<BadModule>]
    private class Module1 : Module
    {
        protected override Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            return NothingAsync();
        }
    }

    // Using TaskCompletionSource allows us to test cancellation without actual delays
    // The task will wait indefinitely until the cancellation token cancels it
    private class LongRunningModule : Module
    {
        private static readonly TaskCompletionSource<bool> _taskCompletionSource = new();

        protected override async Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            // This will wait indefinitely (or until cancellation) without blocking the thread
            // The BadModule exception will trigger engine cancellation which should cancel this
            await _taskCompletionSource.Task.WaitAsync(cancellationToken);
            return await NothingAsync();
        }
    }

    // This module ignores cancellation to test that the pipeline still terminates
    private class LongRunningModuleWithoutCancellation : Module
    {
        private static readonly TaskCompletionSource<bool> _taskCompletionSource = new();

        protected override async Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            try
            {
                // This will wait indefinitely without honoring cancellation
                // We need a short timeout to ensure the test doesn't hang forever
                await _taskCompletionSource.Task.WaitAsync(TimeSpan.FromSeconds(5));
            }
            catch (TimeoutException)
            {
                // Expected - the task will timeout because it's never completed
            }

            return await NothingAsync();
        }
    }

    [Test]
    public async Task When_Cancel_Engine_Token_With_DependsOn_Then_Modules_Cancel()
    {
        var host = await TestPipelineHostBuilder.Create()
            .AddModule<BadModule>()
            .AddModule<Module1>()
            .BuildHostAsync();

        var modules = host.RootServices.GetServices<ModuleBase>();

        var module1 = modules.GetModule<Module1>();

        using (Assert.Multiple())
        {
            await Assert.That(async () => await host.ExecutePipelineAsync()).ThrowsException();
            await Assert.That(module1.Status).IsEqualTo(Status.PipelineTerminated);
        }
    }

    [Test]
    public async Task When_Cancel_Engine_Token_Without_DependsOn_Then_Modules_Cancel()
    {
        var host = await TestPipelineHostBuilder.Create()
            .AddModule<BadModule>()
            .AddModule<LongRunningModule>()
            .BuildHostAsync();

        var modules = host.RootServices.GetServices<ModuleBase>();

        var longRunningModule = modules.GetModule<LongRunningModule>();

        var pipelineTask = host.ExecutePipelineAsync();

        // Wait briefly to allow the bad module to fail and trigger cancellation
        await Task.Delay(WaitForCancellationDelay);

        using (Assert.Multiple())
        {
            await Assert.That(async () => await pipelineTask).ThrowsException();
            await Assert.That(longRunningModule.Status).IsEqualTo(Status.PipelineTerminated);
            // Module should be cancelled almost immediately (within 2 seconds max)
            await Assert.That(longRunningModule.Duration).IsLessThan(TimeSpan.FromSeconds(2));
        }
    }

    [Test]
    public async Task When_Cancel_Engine_Token_Without_DependsOn_Then_Modules_Cancel_Without_Cancellation()
    {
        var host = await TestPipelineHostBuilder.Create()
            .AddModule<BadModule>()
            .AddModule<LongRunningModuleWithoutCancellation>()
            .BuildHostAsync();

        var modules = host.RootServices.GetServices<ModuleBase>();

        var longRunningModule = modules.GetModule<LongRunningModuleWithoutCancellation>();

        var pipelineTask = host.ExecutePipelineAsync();

        // Wait briefly to allow the bad module to fail and trigger cancellation
        await Task.Delay(WaitForCancellationDelay);

        using (Assert.Multiple())
        {
            await Assert.That(async () => await pipelineTask).ThrowsException();
            await Assert.That(longRunningModule.Status).IsEqualTo(Status.PipelineTerminated);
            // Module ignores cancellation so it will run until timeout (5s max)
            // We just verify the pipeline terminated (which it does due to the BadModule exception)
            await Assert.That(longRunningModule.Duration).IsGreaterThanOrEqualTo(TimeSpan.Zero);
            await Assert.That(longRunningModule.Duration).IsLessThan(TimeSpan.FromSeconds(6));
        }
    }
}
