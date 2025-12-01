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

    [ModularPipelines.Attributes.NotInParallel("A")]
    public class Module1 : IModule<string>
    {
        public async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            var moduleName = GetType().Name;

            _executingModules.Add(moduleName);
            await Task.Delay(50, cancellationToken);

            if (_executingModules.Contains("Module2"))
            {
                _violations.Add($"{moduleName} started while Module2 was executing (both have 'A')");
            }

            await Task.Delay(50, cancellationToken);

            _executingModules.TryTake(out _);
            return moduleName;
        }
    }

    [ModularPipelines.Attributes.NotInParallel("A", "B")]
    public class Module2 : IModule<string>
    {
        public async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            var moduleName = GetType().Name;

            _executingModules.Add(moduleName);
            await Task.Delay(50, cancellationToken);

            if (_executingModules.Contains("Module1"))
            {
                _violations.Add($"{moduleName} started while Module1 was executing (both have 'A')");
            }

            if (_executingModules.Contains("Module3"))
            {
                _violations.Add($"{moduleName} started while Module3 was executing (both have 'B')");
            }

            await Task.Delay(50, cancellationToken);

            _executingModules.TryTake(out _);
            return moduleName;
        }
    }

    [ModularPipelines.Attributes.NotInParallel("B", "C")]
    public class Module3 : IModule<string>
    {
        public async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            var moduleName = GetType().Name;

            _executingModules.Add(moduleName);
            await Task.Delay(50, cancellationToken);

            if (_executingModules.Contains("Module2"))
            {
                _violations.Add($"{moduleName} started while Module2 was executing (both have 'B')");
            }

            await Task.Delay(50, cancellationToken);

            _executingModules.TryTake(out _);
            return moduleName;
        }
    }

    [ModularPipelines.Attributes.NotInParallel("D")]
    public class Module4 : IModule<string>
    {
        public async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            var moduleName = GetType().Name;
            _executingModules.Add(moduleName);

            await Task.Delay(100, cancellationToken);

            _executingModules.TryTake(out _);
            return moduleName;
        }
    }

    [Test]
    public async Task NotInParallel_If_Any_Modules_Executing_With_Any_Of_Same_ConstraintKey()
    {
        _executingModules.Clear();
        _violations.Clear();

        await TestPipelineHostBuilder.Create()
            .AddModule<Module1>()
            .AddModule<Module2>()
            .AddModule<Module3>()
            .AddModule<Module4>()
            .ExecutePipelineAsync();

        await Assert.That(_violations).IsEmpty();
    }
}