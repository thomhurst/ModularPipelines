using System.Collections.Concurrent;
using Microsoft.Extensions.Time.Testing;
using ModularPipelines.Context;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests;

public class NotInParallelTestsWithMultipleConstraintKeys : TestBase
{
    private static readonly ConcurrentBag<string> _executingModules = new();
    private static readonly ConcurrentBag<string> _violations = new();
    private static TaskCompletionSource _allModulesStarted = new();
    private static int _startedCount;

    [ModularPipelines.Attributes.NotInParallel("A")]
    public class Module1 : Module<string>
    {
        protected override async Task<string?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            var moduleName = GetType().Name;

            if (_executingModules.Contains("Module2"))
            {
                _violations.Add($"{moduleName} started while Module2 was executing (both have 'A')");
            }

            _executingModules.Add(moduleName);

            if (Interlocked.Increment(ref _startedCount) == 4)
            {
                _allModulesStarted.SetResult();
            }

            await _allModulesStarted.Task;
            await Task.Yield();

            _executingModules.TryTake(out _);
            return moduleName;
        }
    }

    [ModularPipelines.Attributes.NotInParallel("A", "B")]
    public class Module2 : Module<string>
    {
        protected override async Task<string?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            var moduleName = GetType().Name;

            if (_executingModules.Contains("Module1"))
            {
                _violations.Add($"{moduleName} started while Module1 was executing (both have 'A')");
            }

            if (_executingModules.Contains("Module3"))
            {
                _violations.Add($"{moduleName} started while Module3 was executing (both have 'B')");
            }

            _executingModules.Add(moduleName);

            if (Interlocked.Increment(ref _startedCount) == 4)
            {
                _allModulesStarted.SetResult();
            }

            await _allModulesStarted.Task;
            await Task.Yield();

            _executingModules.TryTake(out _);
            return moduleName;
        }
    }

    [ModularPipelines.Attributes.NotInParallel("B", "C")]
    public class Module3 : Module<string>
    {
        protected override async Task<string?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            var moduleName = GetType().Name;

            if (_executingModules.Contains("Module2"))
            {
                _violations.Add($"{moduleName} started while Module2 was executing (both have 'B')");
            }

            _executingModules.Add(moduleName);

            if (Interlocked.Increment(ref _startedCount) == 4)
            {
                _allModulesStarted.SetResult();
            }

            await _allModulesStarted.Task;
            await Task.Yield();

            _executingModules.TryTake(out _);
            return moduleName;
        }
    }

    [ModularPipelines.Attributes.NotInParallel("D")]
    public class Module4 : Module<string>
    {
        protected override async Task<string?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            var moduleName = GetType().Name;
            _executingModules.Add(moduleName);

            if (Interlocked.Increment(ref _startedCount) == 4)
            {
                _allModulesStarted.SetResult();
            }

            await _allModulesStarted.Task;
            await Task.Yield();

            _executingModules.TryTake(out _);
            return moduleName;
        }
    }

    [Test]
    public async Task NotInParallel_If_Any_Modules_Executing_With_Any_Of_Same_ConstraintKey()
    {
        _executingModules.Clear();
        _violations.Clear();
        _allModulesStarted = new TaskCompletionSource();
        _startedCount = 0;

        await TestPipelineHostBuilder.Create()
            .AddModule<Module1>()
            .AddModule<Module2>()
            .AddModule<Module3>()
            .AddModule<Module4>()
            .ExecutePipelineAsync();

        await Assert.That(_violations).IsEmpty();
    }
}