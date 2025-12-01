using System.Collections.Concurrent;
using Microsoft.Extensions.Time.Testing;
using ModularPipelines.Context;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests;

public class NotInParallelTestsWithConstraintKeys : TestBase
{
    private static readonly ConcurrentBag<string> _executingModules = new();
    private static readonly ConcurrentBag<string> _violations = new();

    [ModularPipelines.Attributes.NotInParallel("A")]
    public class ModuleWithAConstraintKey1 : IModule<string>
    {
        public async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            var moduleName = GetType().Name;

            _executingModules.Add(moduleName);
            await Task.Delay(50, cancellationToken);

            if (_executingModules.Contains("ModuleWithAConstraintKey2"))
            {
                _violations.Add($"{moduleName} started while ModuleWithAConstraintKey2 was executing");
            }

            await Task.Delay(50, cancellationToken);

            _executingModules.TryTake(out _);
            return moduleName;
        }
    }

    [ModularPipelines.Attributes.NotInParallel("A")]
    public class ModuleWithAConstraintKey2 : IModule<string>
    {
        public async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            var moduleName = GetType().Name;

            _executingModules.Add(moduleName);
            await Task.Delay(50, cancellationToken);

            if (_executingModules.Contains("ModuleWithAConstraintKey1"))
            {
                _violations.Add($"{moduleName} started while ModuleWithAConstraintKey1 was executing");
            }

            await Task.Delay(50, cancellationToken);

            _executingModules.TryTake(out _);
            return moduleName;
        }
    }

    [ModularPipelines.Attributes.NotInParallel("B")]
    public class ModuleWithBConstraintKey1 : IModule<string>
    {
        public async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            var moduleName = GetType().Name;

            _executingModules.Add(moduleName);
            await Task.Delay(50, cancellationToken);

            if (_executingModules.Contains("ModuleWithBConstraintKey2"))
            {
                _violations.Add($"{moduleName} started while ModuleWithBConstraintKey2 was executing");
            }

            await Task.Delay(50, cancellationToken);

            _executingModules.TryTake(out _);
            return moduleName;
        }
    }

    [ModularPipelines.Attributes.NotInParallel("B")]
    public class ModuleWithBConstraintKey2 : IModule<string>
    {
        public async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            var moduleName = GetType().Name;

            _executingModules.Add(moduleName);
            await Task.Delay(50, cancellationToken);

            if (_executingModules.Contains("ModuleWithBConstraintKey1"))
            {
                _violations.Add($"{moduleName} started while ModuleWithBConstraintKey1 was executing");
            }

            await Task.Delay(50, cancellationToken);

            _executingModules.TryTake(out _);
            return moduleName;
        }
    }

    [Test]
    public async Task NotInParallel_If_Same_ConstraintKey()
    {
        _executingModules.Clear();
        _violations.Clear();

        await TestPipelineHostBuilder.Create()
            .AddModule<ModuleWithAConstraintKey1>()
            .AddModule<ModuleWithAConstraintKey2>()
            .AddModule<ModuleWithBConstraintKey1>()
            .AddModule<ModuleWithBConstraintKey2>()
            .ExecutePipelineAsync();

        await Assert.That(_violations).IsEmpty();
    }
}