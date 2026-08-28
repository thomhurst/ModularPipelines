using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Context;
using ModularPipelines.Engine;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;
using ModularPipelines.Enums;

namespace ModularPipelines.UnitTests.Engine;

public class SafeEstimatedTimeProviderTests
{
    [Test]
    public async Task When_EstimatedTimeProvider_Succeeds_Then_No_Error()
    {
        var host = await TestPipelineBuilder.Create()
            .AddModule<DummyModule>()
            .AddModuleEstimatedTimeProvider<SuccessfulTimeProvider>()
            .BuildAsync();

        await host.RunAsync();

        var resultRegistry = host.Services.GetRequiredService<IModuleResultRegistry>();
        var result = resultRegistry.GetResult(typeof(DummyModule))!;
        await Assert.That(result.Status).IsEqualTo(ModuleStatus.Succeeded);
    }

    [Test]
    public async Task When_EstimatedTimeProvider_Fails_Receiving_Time_Then_Still_No_Error()
    {
        var host = await TestPipelineBuilder.Create()
            .AddModule<DummyModule>()
            .AddModuleEstimatedTimeProvider<FailingTimeProvider>()
            .BuildAsync();

        await host.RunAsync();

        var resultRegistry = host.Services.GetRequiredService<IModuleResultRegistry>();
        var result = resultRegistry.GetResult(typeof(DummyModule))!;
        await Assert.That(result.Status).IsEqualTo(ModuleStatus.Succeeded);
    }

    [Test]
    public async Task When_EstimatedTimeProvider_Fails_Saving_Time_Then_Still_No_Error()
    {
        var host = await TestPipelineBuilder.Create()
            .AddModule<DummyModule>()
            .AddModuleEstimatedTimeProvider<FailingTimeProvider2>()
            .BuildAsync();

        await host.RunAsync();

        var resultRegistry = host.Services.GetRequiredService<IModuleResultRegistry>();
        var result = resultRegistry.GetResult(typeof(DummyModule))!;
        await Assert.That(result.Status).IsEqualTo(ModuleStatus.Succeeded);
    }

    [Test]
    public async Task Pipeline_Termination_Does_Not_Save_Module_Time()
    {
        TrackingTimeProvider.SaveCount = 0;
        CancellableModule.Reset();
        var host = await TestPipelineBuilder.Create()
            .AddModule<CancellableModule>()
            .AddModuleEstimatedTimeProvider<TrackingTimeProvider>()
            .BuildAsync();
        using var cancellationTokenSource = new CancellationTokenSource();
        var runTask = host.RunAsync(cancellationTokenSource.Token);
        await CancellableModule.Entered.Task.WaitAsync(TimeSpan.FromSeconds(5));
        cancellationTokenSource.Cancel();

        try
        {
            await runTask;
        }
        catch (OperationCanceledException)
        {
        }

        await Assert.That(TrackingTimeProvider.SaveCount).IsEqualTo(0);
    }

    private class DummyModule : Module<bool>
    {
        protected internal override Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            return Task.FromResult(true);
        }
    }

    private class CancellableModule : Module<bool>
    {
        public static TaskCompletionSource Entered { get; private set; } = CreateSignal();

        public static void Reset() => Entered = CreateSignal();

        protected internal override async Task<bool> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            Entered.TrySetResult();
            await Task.Delay(Timeout.InfiniteTimeSpan, cancellationToken);
            return true;
        }

        private static TaskCompletionSource CreateSignal() =>
            new(TaskCreationOptions.RunContinuationsAsynchronously);
    }

    private class TrackingTimeProvider : IModuleEstimatedTimeProvider
    {
        public static int SaveCount
        {
            get => Volatile.Read(ref _saveCount);
            set => Interlocked.Exchange(ref _saveCount, value);
        }

        public Task<TimeSpan> GetModuleEstimatedTimeAsync(Type moduleType) =>
            Task.FromResult(TimeSpan.FromMinutes(1));

        public Task SaveModuleTimeAsync(Type moduleType, TimeSpan duration)
        {
            Interlocked.Increment(ref _saveCount);
            return Task.CompletedTask;
        }

        public Task<IEnumerable<SubModuleEstimation>> GetSubModuleEstimatedTimesAsync(Type moduleType) =>
            Task.FromResult<IEnumerable<SubModuleEstimation>>([]);

        public Task SaveSubModuleTimeAsync(Type moduleType, SubModuleEstimation subModuleEstimation) =>
            Task.CompletedTask;

        private static int _saveCount;
    }

    private class SuccessfulTimeProvider : IModuleEstimatedTimeProvider
    {
        public Task<TimeSpan> GetModuleEstimatedTimeAsync(Type moduleType)
        {
            return Task.FromResult(TimeSpan.FromMinutes(1));
        }

        public Task SaveModuleTimeAsync(Type moduleType, TimeSpan duration)
        {
            return Task.CompletedTask;
        }

        public Task<IEnumerable<SubModuleEstimation>> GetSubModuleEstimatedTimesAsync(Type moduleType)
        {
            return Task.FromResult<IEnumerable<SubModuleEstimation>>(new List<SubModuleEstimation>());
        }

        public Task SaveSubModuleTimeAsync(Type moduleType, SubModuleEstimation subModuleEstimation)
        {
            return Task.CompletedTask;
        }
    }

    private class FailingTimeProvider : IModuleEstimatedTimeProvider
    {
        public Task<TimeSpan> GetModuleEstimatedTimeAsync(Type moduleType)
        {
            throw new Exception();
        }

        public Task SaveModuleTimeAsync(Type moduleType, TimeSpan duration)
        {
            throw new Exception();
        }

        public Task<IEnumerable<SubModuleEstimation>> GetSubModuleEstimatedTimesAsync(Type moduleType)
        {
            throw new Exception();
        }

        public Task SaveSubModuleTimeAsync(Type moduleType, SubModuleEstimation subModuleEstimation)
        {
            throw new Exception();
        }
    }

    private class FailingTimeProvider2 : IModuleEstimatedTimeProvider
    {
        public Task<TimeSpan> GetModuleEstimatedTimeAsync(Type moduleType)
        {
            return Task.FromResult(TimeSpan.FromMinutes(2));
        }

        public Task SaveModuleTimeAsync(Type moduleType, TimeSpan duration)
        {
            throw new Exception();
        }

        public Task<IEnumerable<SubModuleEstimation>> GetSubModuleEstimatedTimesAsync(Type moduleType)
        {
            return Task.FromResult<IEnumerable<SubModuleEstimation>>(new List<SubModuleEstimation>());
        }

        public Task SaveSubModuleTimeAsync(Type moduleType, SubModuleEstimation subModuleEstimation)
        {
            throw new Exception();
        }
    }
}
