using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Engine;
using ModularPipelines.Extensions;
using ModularPipelines.Modules;
using ModularPipelines.Modules.Behaviors;
using ModularPipelines.TestHelpers;
using Status = ModularPipelines.Enums.Status;

namespace ModularPipelines.UnitTests;

public class EngineCancellationTokenTests : TestBase
{
    private static readonly TimeSpan WaitForCancellationDelay = TimeSpan.FromMilliseconds(100);

    private class BadModule : Module<IDictionary<string, object>?>
    {
        public override async Task<IDictionary<string, object>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            throw new Exception();
        }
    }

    [ModularPipelines.Attributes.DependsOn<BadModule>]
    private class Module1 : Module<IDictionary<string, object>?>
    {
        public override Task<IDictionary<string, object>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            return Task.FromResult<IDictionary<string, object>?>(null);
        }
    }

    private class LongRunningModule : Module<IDictionary<string, object>?>
    {
        private readonly TaskCompletionSource<bool> _taskCompletionSource = new();

        public override async Task<IDictionary<string, object>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await _taskCompletionSource.Task.WaitAsync(cancellationToken);
            return null;
        }
    }

    private class LongRunningModuleWithoutCancellation : Module<IDictionary<string, object>?>, ITimeoutable
    {
        private readonly TaskCompletionSource<bool> _taskCompletionSource = new();

        public TimeSpan Timeout => TimeSpan.FromSeconds(1);

        public override async Task<IDictionary<string, object>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await _taskCompletionSource.Task;
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

        var resultRegistry = host.RootServices.GetRequiredService<IModuleResultRegistry>();

        using (Assert.Multiple())
        {
            await Assert.That(async () => await host.ExecutePipelineAsync()).ThrowsException();
            var module1Result = resultRegistry.GetResult(typeof(Module1))!;
            await Assert.That(module1Result.ModuleStatus).IsEqualTo(Status.PipelineTerminated);
        }
    }

    [Test]
    public async Task When_Cancel_Engine_Token_Without_DependsOn_Then_Modules_Cancel()
    {
        var host = await TestPipelineHostBuilder.Create()
            .AddModule<BadModule>()
            .AddModule<LongRunningModule>()
            .BuildHostAsync();

        var resultRegistry = host.RootServices.GetRequiredService<IModuleResultRegistry>();

        var pipelineTask = host.ExecutePipelineAsync();

        await Task.Delay(WaitForCancellationDelay);

        using (Assert.Multiple())
        {
            await Assert.That(async () => await pipelineTask).ThrowsException();
            var longRunningModuleResult = resultRegistry.GetResult(typeof(LongRunningModule))!;
            await Assert.That(longRunningModuleResult.ModuleStatus).IsEqualTo(Status.PipelineTerminated);
            await Assert.That(longRunningModuleResult.ModuleDuration).IsLessThan(TimeSpan.FromSeconds(2));
        }
    }

    [Test]
    public async Task When_Cancel_Engine_Token_Without_DependsOn_Then_Modules_Cancel_Without_Cancellation()
    {
        var host = await TestPipelineHostBuilder.Create()
            .AddModule<BadModule>()
            .AddModule<LongRunningModuleWithoutCancellation>()
            .BuildHostAsync();

        var resultRegistry = host.RootServices.GetRequiredService<IModuleResultRegistry>();

        var pipelineTask = host.ExecutePipelineAsync();

        await Task.Delay(WaitForCancellationDelay);

        using (Assert.Multiple())
        {
            await Assert.That(async () => await pipelineTask).ThrowsException();
            var longRunningModuleResult = resultRegistry.GetResult(typeof(LongRunningModuleWithoutCancellation))!;
            await Assert.That(longRunningModuleResult.ModuleStatus).IsEqualTo(Status.PipelineTerminated);
        }
    }
}
