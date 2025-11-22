using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Extensions;
using ModularPipelines.Modules;
using ModularPipelines.Modules.Behaviors;
using ModularPipelines.TestHelpers;
using Status = ModularPipelines.Enums.Status;

namespace ModularPipelines.UnitTests;

public class EngineCancellationTokenTests : TestBase
{
    private static readonly TimeSpan WaitForCancellationDelay = TimeSpan.FromMilliseconds(100);

    private class BadModule : Module
    {
        public override async Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            throw new Exception();
        }
    }

    [ModularPipelines.Attributes.DependsOn<BadModule>]
    private class Module1 : Module
    {
        public override async Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            return null;
        }
    }

    private class LongRunningModule : Module
    {
        private readonly TaskCompletionSource<bool> _taskCompletionSource = new();

        public override async Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await _taskCompletionSource.Task.WaitAsync(cancellationToken);
            await Task.CompletedTask;
            return null;
        }
    }

    private class LongRunningModuleWithoutCancellation : Module, IModuleTimeout
    {
        private readonly TaskCompletionSource<bool> _taskCompletionSource = new();

        public TimeSpan GetTimeout() => TimeSpan.FromSeconds(1);

        public override async Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await _taskCompletionSource.Task;
            await Task.CompletedTask;
            return null;
        }
    }

    [Test]
    public async Task When_Cancel_Engine_Token_With_DependsOn_Then_Modules_Cancel()
    {
        var host = await TestPipelineHostBuilder.Create()
            .AddModule<BadModule>()
            .AddModule<Module1>()
            .BuildHostAsync();

        var modules = host.RootServices.GetServices<IModule>();

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

        var modules = host.RootServices.GetServices<IModule>();

        var longRunningModule = modules.GetModule<LongRunningModule>();

        var pipelineTask = host.ExecutePipelineAsync();

        await Task.Delay(WaitForCancellationDelay);

        using (Assert.Multiple())
        {
            await Assert.That(async () => await pipelineTask).ThrowsException();
            await Assert.That(longRunningModule.Status).IsEqualTo(Status.PipelineTerminated);
            // TODO v3.0: Duration property removed from IModule - get from ModuleResult instead
            // await Assert.That(longRunningModule.Duration).IsLessThan(TimeSpan.FromSeconds(2));
        }
    }

    [Test]
    public async Task When_Cancel_Engine_Token_Without_DependsOn_Then_Modules_Cancel_Without_Cancellation()
    {
        var host = await TestPipelineHostBuilder.Create()
            .AddModule<BadModule>()
            .AddModule<LongRunningModuleWithoutCancellation>()
            .BuildHostAsync();

        var modules = host.RootServices.GetServices<IModule>();

        var longRunningModule = modules.GetModule<LongRunningModuleWithoutCancellation>();

        var pipelineTask = host.ExecutePipelineAsync();

        await Task.Delay(WaitForCancellationDelay);

        using (Assert.Multiple())
        {
            await Assert.That(async () => await pipelineTask).ThrowsException();
            await Assert.That(longRunningModule.Status).IsEqualTo(Status.PipelineTerminated);
        }
    }
}
