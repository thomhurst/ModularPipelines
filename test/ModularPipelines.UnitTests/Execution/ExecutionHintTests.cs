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
    private static readonly ConcurrentBag<string> CpuModulesExecuting = new();
    private static readonly ConcurrentBag<string> CpuViolations = new();
    private static int _maxCpuConcurrency = 0;

    [ExecutionHint(ExecutionHint.CpuBound)]
    public class CpuBoundModule1 : Module<string>
    {
        protected internal override async Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            var moduleName = GetType().Name;
            CpuModulesExecuting.Add(moduleName);

            var currentCount = CpuModulesExecuting.Count;
            if (currentCount > _maxCpuConcurrency)
            {
                Interlocked.Exchange(ref _maxCpuConcurrency, currentCount);
            }

            await Task.Delay(50, cancellationToken);

            // Record violation if more than allowed
            if (CpuModulesExecuting.Count > 2)
            {
                CpuViolations.Add($"{moduleName}: {CpuModulesExecuting.Count} concurrent CPU-bound modules");
            }

            CpuModulesExecuting.TryTake(out _);
            return moduleName;
        }
    }

    [ExecutionHint(ExecutionHint.CpuBound)]
    public class CpuBoundModule2 : Module<string>
    {
        protected internal override async Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            var moduleName = GetType().Name;
            CpuModulesExecuting.Add(moduleName);

            var currentCount = CpuModulesExecuting.Count;
            if (currentCount > _maxCpuConcurrency)
            {
                Interlocked.Exchange(ref _maxCpuConcurrency, currentCount);
            }

            await Task.Delay(50, cancellationToken);

            CpuModulesExecuting.TryTake(out _);
            return moduleName;
        }
    }

    [ExecutionHint(ExecutionHint.CpuBound)]
    public class CpuBoundModule3 : Module<string>
    {
        protected internal override async Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            var moduleName = GetType().Name;
            CpuModulesExecuting.Add(moduleName);

            var currentCount = CpuModulesExecuting.Count;
            if (currentCount > _maxCpuConcurrency)
            {
                Interlocked.Exchange(ref _maxCpuConcurrency, currentCount);
            }

            await Task.Delay(50, cancellationToken);

            CpuModulesExecuting.TryTake(out _);
            return moduleName;
        }
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
        while (CpuModulesExecuting.TryTake(out _)) { }
        while (CpuViolations.TryTake(out _)) { }
        _maxCpuConcurrency = 0;
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
        // Set max CPU-bound modules to 2
        var result = await TestPipelineBuilder.Create()
            .AddModule<CpuBoundModule1>()
            .AddModule<CpuBoundModule2>()
            .AddModule<CpuBoundModule3>()
            .ConfigureOptions(options => options with
            {
                Concurrency = options.Concurrency with { MaxCpuIntensiveModules = 2 },
            })
            .RunAsync();

        await Assert.That(result.Status).IsEqualTo(ModuleStatus.Succeeded);
        // The max concurrency should not exceed 2
        await Assert.That(_maxCpuConcurrency).IsLessThanOrEqualTo(2);
    }
}
