using System.Collections.Concurrent;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Enums;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests.Execution;

[TUnit.Core.NotInParallel(nameof(ExecutionHintTests))]
public class ExecutionHintTests : TestBase
{
    private static readonly ConcurrentBag<string> CpuViolations = new();
    private static int _cpuConcurrency;
    private static int _maxCpuConcurrency;
    private static TaskCompletionSource? _twoCpuModulesStarted;

    private static async Task<string> ExecuteCpuBoundModuleAsync(string moduleName, CancellationToken cancellationToken)
    {
        var currentCount = Interlocked.Increment(ref _cpuConcurrency);
        try
        {
            var maximum = Volatile.Read(ref _maxCpuConcurrency);
            while (currentCount > maximum)
            {
                var observed = Interlocked.CompareExchange(ref _maxCpuConcurrency, currentCount, maximum);
                if (observed == maximum)
                {
                    break;
                }

                maximum = observed;
            }

            if (currentCount > 2)
            {
                CpuViolations.Add($"{moduleName}: {currentCount} concurrent CPU-bound modules");
            }

            if (currentCount >= 2)
            {
                _twoCpuModulesStarted?.TrySetResult();
            }

            if (_twoCpuModulesStarted is { } twoCpuModulesStarted)
            {
                // Make overlap explicit so serial execution fails even on a busy runner.
                await twoCpuModulesStarted.Task.WaitAsync(TimeSpan.FromSeconds(10), cancellationToken);
            }

            await Task.Delay(50, cancellationToken);
            return moduleName;
        }
        finally
        {
            Interlocked.Decrement(ref _cpuConcurrency);
        }
    }

    [ExecutionHint(ExecutionHint.CpuBound)]
    public class CpuBoundModule1 : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
            => ExecuteCpuBoundModuleAsync(GetType().Name, cancellationToken);
    }

    [ExecutionHint(ExecutionHint.CpuBound)]
    public class CpuBoundModule2 : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
            => ExecuteCpuBoundModuleAsync(GetType().Name, cancellationToken);
    }

    [ExecutionHint(ExecutionHint.CpuBound)]
    public class CpuBoundModule3 : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
            => ExecuteCpuBoundModuleAsync(GetType().Name, cancellationToken);
    }

    [ExecutionHint(ExecutionHint.IoBound)]
    public class IoBoundModule : Module<string>
    {
        protected internal override async Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Delay(10, cancellationToken);
            return "IoBound";
        }
    }

    [ExecutionHint(ExecutionHint.Default)]
    public class DefaultExecutionHintModule : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            return Task.FromResult<string>("Default");
        }
    }

    public class NoHintModule : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            return Task.FromResult<string>("NoHint");
        }
    }

    [Before(Test)]
    public void ClearState()
    {
        _cpuConcurrency = 0;
        while (CpuViolations.TryTake(out _)) { }
        _maxCpuConcurrency = 0;
        _twoCpuModulesStarted = null;
    }

    [Test]
    public async Task ExecutionHintAttribute_CanBeAppliedToModule()
    {
        var result = await TestPipelineBuilder.Create()
            .AddModule<CpuBoundModule1>()
            .RunAsync();

        await Assert.That(result.Status).IsEqualTo(ModuleStatus.Succeeded);
    }

    [Test]
    public async Task ModulesWithoutExecutionHint_UseDefaultHint()
    {
        var result = await TestPipelineBuilder.Create()
            .AddModule<NoHintModule>()
            .RunAsync();

        await Assert.That(result.Status).IsEqualTo(ModuleStatus.Succeeded);
    }

    [Test]
    public async Task AllExecutionHints_ExecuteSuccessfully()
    {
        var result = await TestPipelineBuilder.Create()
            .AddModule<CpuBoundModule1>()
            .AddModule<IoBoundModule>()
            .AddModule<DefaultExecutionHintModule>()
            .AddModule<NoHintModule>()
            .RunAsync();

        await Assert.That(result.Status).IsEqualTo(ModuleStatus.Succeeded);
    }

    [Test]
    public async Task CpuBoundModules_AreThrottled()
    {
        _twoCpuModulesStarted = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var result = await TestPipelineBuilder.Create()
            .AddModule<CpuBoundModule1>()
            .AddModule<CpuBoundModule2>()
            .AddModule<CpuBoundModule3>()
            .ConfigureOptions(options => options with
            {
                Concurrency = options.Concurrency with { MaxParallelism = 3, MaxCpuIntensiveModules = 2 },
            })
            .RunAsync();

        await Assert.That(result.Status).IsEqualTo(ModuleStatus.Succeeded);
        await Assert.That(CpuViolations).IsEmpty();
        await Assert.That(_maxCpuConcurrency).IsGreaterThan(1);
        await Assert.That(_maxCpuConcurrency).IsLessThanOrEqualTo(2);
    }
}
