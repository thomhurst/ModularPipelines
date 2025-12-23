using System.Collections.Concurrent;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Enums;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests;

[TUnit.Core.NotInParallel(nameof(ExecutionHintTests))]
public class ExecutionHintTests : TestBase
{
    private static readonly ConcurrentBag<string> CpuModulesExecuting = new();
    private static readonly ConcurrentBag<string> CpuViolations = new();
    private static int _maxCpuConcurrency = 0;

    [ExecutionHint(ExecutionType.CpuIntensive)]
    public class CpuIntensiveModule1 : Module<string>
    {
        public override async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
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
                CpuViolations.Add($"{moduleName}: {CpuModulesExecuting.Count} concurrent CPU-intensive modules");
            }

            CpuModulesExecuting.TryTake(out _);
            return moduleName;
        }
    }

    [ExecutionHint(ExecutionType.CpuIntensive)]
    public class CpuIntensiveModule2 : Module<string>
    {
        public override async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
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

    [ExecutionHint(ExecutionType.CpuIntensive)]
    public class CpuIntensiveModule3 : Module<string>
    {
        public override async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
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

    [ExecutionHint(ExecutionType.IoIntensive)]
    public class IoIntensiveModule : Module<string>
    {
        public override async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Delay(10, cancellationToken);
            return "IoIntensive";
        }
    }

    [ExecutionHint(ExecutionType.Default)]
    public class DefaultExecutionTypeModule : Module<string>
    {
        public override Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            return Task.FromResult<string?>("Default");
        }
    }

    public class NoHintModule : Module<string>
    {
        public override Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            return Task.FromResult<string?>("NoHint");
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
        var result = await TestPipelineHostBuilder.Create()
            .AddModule<CpuIntensiveModule1>()
            .ExecutePipelineAsync();

        await Assert.That(result.Status).IsEqualTo(Status.Successful);
    }

    [Test]
    public async Task ModulesWithoutExecutionHint_UseDefaultType()
    {
        var result = await TestPipelineHostBuilder.Create()
            .AddModule<NoHintModule>()
            .ExecutePipelineAsync();

        await Assert.That(result.Status).IsEqualTo(Status.Successful);
    }

    [Test]
    public async Task AllExecutionTypes_ExecuteSuccessfully()
    {
        var result = await TestPipelineHostBuilder.Create()
            .AddModule<CpuIntensiveModule1>()
            .AddModule<IoIntensiveModule>()
            .AddModule<DefaultExecutionTypeModule>()
            .AddModule<NoHintModule>()
            .ExecutePipelineAsync();

        await Assert.That(result.Status).IsEqualTo(Status.Successful);
    }

    [Test]
    public async Task CpuIntensiveModules_AreThrottled()
    {
        // Set max CPU-intensive modules to 2
        var result = await TestPipelineHostBuilder.Create()
            .AddModule<CpuIntensiveModule1>()
            .AddModule<CpuIntensiveModule2>()
            .AddModule<CpuIntensiveModule3>()
            .ConfigurePipelineOptions((_, options) =>
            {
                options.Concurrency.MaxCpuIntensiveModules = 2;
            })
            .ExecutePipelineAsync();

        await Assert.That(result.Status).IsEqualTo(Status.Successful);
        // The max concurrency should not exceed 2
        await Assert.That(_maxCpuConcurrency).IsLessThanOrEqualTo(2);
    }
}
