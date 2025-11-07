using System.Collections.Concurrent;
using Microsoft.Extensions.Time.Testing;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests;

public class NotInParallelTests
{
    private static readonly TimeSpan ModuleDelay = TimeSpan.FromMilliseconds(100);
    private static readonly ConcurrentBag<string> _executingModules = new();
    private static readonly ConcurrentBag<string> _violations = new();

    [ModularPipelines.Attributes.NotInParallel]
    public class Module1 : Module<string>
    {
        protected override async Task<string?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            var moduleName = GetType().Name;

            _executingModules.Add(moduleName);
            await Task.Delay(50, cancellationToken);

            if (_executingModules.Contains("Module2"))
            {
                _violations.Add($"{moduleName} started while Module2 was executing");
            }

            await Task.Delay(50, cancellationToken);

            _executingModules.TryTake(out _);
            return moduleName;
        }
    }

    [ModularPipelines.Attributes.NotInParallel]
    public class Module2 : Module<string>
    {
        protected override async Task<string?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            var moduleName = GetType().Name;

            _executingModules.Add(moduleName);
            await Task.Delay(50, cancellationToken);

            if (_executingModules.Contains("Module1"))
            {
                _violations.Add($"{moduleName} started while Module1 was executing");
            }

            await Task.Delay(50, cancellationToken);

            _executingModules.TryTake(out _);
            return moduleName;
        }
    }

    [ModularPipelines.Attributes.NotInParallel]
    [ModularPipelines.Attributes.DependsOn<ParallelDependency>]
    public class NotParallelModuleWithParallelDependency : Module<string>
    {
        protected override async Task<string?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            var moduleName = GetType().Name;

            _executingModules.Add(moduleName);
            await Task.Delay(100, cancellationToken);
            _executingModules.TryTake(out _);

            return moduleName;
        }
    }

    public class ParallelDependency : Module<string>
    {
        protected override async Task<string?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await Task.Delay(ModuleDelay, cancellationToken);
            return GetType().Name;
        }
    }

    [ModularPipelines.Attributes.NotInParallel]
    [ModularPipelines.Attributes.DependsOn<NotParallelModuleWithParallelDependency>]
    public class NotParallelModuleWithNonParallelDependency : Module<string>
    {
        protected override async Task<string?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            var moduleName = GetType().Name;

            _executingModules.Add(moduleName);
            await Task.Delay(50, cancellationToken);

            if (_executingModules.Contains("NotParallelModuleWithParallelDependency"))
            {
                _violations.Add($"{moduleName} started while NotParallelModuleWithParallelDependency was executing");
            }

            await Task.Delay(50, cancellationToken);

            _executingModules.TryTake(out _);
            return moduleName;
        }
    }

    [Test]
    public async Task NotInParallel()
    {
        _executingModules.Clear();
        _violations.Clear();

        await TestPipelineHostBuilder.Create()
            .AddModule<Module1>()
            .AddModule<Module2>()
            .ExecutePipelineAsync();

        await Assert.That(_violations).IsEmpty();
    }

    [Test]
    public async Task NotInParallel_With_ParallelDependency()
    {
        _executingModules.Clear();
        _violations.Clear();

        await TestPipelineHostBuilder.Create()
            .AddModule<NotParallelModuleWithParallelDependency>()
            .AddModule<ParallelDependency>()
            .ExecutePipelineAsync();

        await Assert.That(_violations).IsEmpty();
    }

    [Test]
    public async Task NotInParallel_With_NonParallelDependency()
    {
        _executingModules.Clear();
        _violations.Clear();

        await TestPipelineHostBuilder.Create()
            .AddModule<NotParallelModuleWithParallelDependency>()
            .AddModule<ParallelDependency>()
            .AddModule<NotParallelModuleWithNonParallelDependency>()
            .ExecutePipelineAsync();

        await Assert.That(_violations).IsEmpty();
    }
}