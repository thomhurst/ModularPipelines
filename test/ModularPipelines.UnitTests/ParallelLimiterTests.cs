using System.Collections.Concurrent;
using Microsoft.Extensions.Time.Testing;
using ModularPipelines.Context;
using ModularPipelines.Interfaces;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests;

public class ParallelLimiterTests
{
    private static readonly TimeSpan ModuleDelay = TimeSpan.FromMilliseconds(100);
    private static readonly ConcurrentBag<string> _executingModules = new();
    private static readonly ConcurrentBag<string> _violations = new();

    [ModularPipelines.Attributes.ParallelLimiter<MyParallelLimit>]
    public class Module1 : Module<string>
    {
        protected override async Task<string?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            var moduleName = GetType().Name;

            _executingModules.Add(moduleName);
            await Task.Delay(50, cancellationToken);

            if (_executingModules.Count > 3)
            {
                _violations.Add($"{moduleName} executing with {_executingModules.Count} total modules (limit is 3)");
            }

            await Task.Delay(50, cancellationToken);

            _executingModules.TryTake(out _);
            return moduleName;
        }
    }

    [ModularPipelines.Attributes.ParallelLimiter<MyParallelLimit>]
    public class Module2 : Module<string>
    {
        protected override async Task<string?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            var moduleName = GetType().Name;

            _executingModules.Add(moduleName);
            await Task.Delay(50, cancellationToken);

            if (_executingModules.Count > 3)
            {
                _violations.Add($"{moduleName} executing with {_executingModules.Count} total modules (limit is 3)");
            }

            await Task.Delay(50, cancellationToken);

            _executingModules.TryTake(out _);
            return moduleName;
        }
    }

    [ModularPipelines.Attributes.ParallelLimiter<MyParallelLimit>]
    public class Module3 : Module<string>
    {
        protected override async Task<string?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            var moduleName = GetType().Name;

            _executingModules.Add(moduleName);
            await Task.Delay(50, cancellationToken);

            if (_executingModules.Count > 3)
            {
                _violations.Add($"{moduleName} executing with {_executingModules.Count} total modules (limit is 3)");
            }

            await Task.Delay(50, cancellationToken);

            _executingModules.TryTake(out _);
            return moduleName;
        }
    }


    [ModularPipelines.Attributes.ParallelLimiter<MyParallelLimit>]
    public class Module4 : Module<string>
    {
        protected override async Task<string?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            var moduleName = GetType().Name;

            _executingModules.Add(moduleName);
            await Task.Delay(50, cancellationToken);

            if (_executingModules.Count > 3)
            {
                _violations.Add($"{moduleName} executing with {_executingModules.Count} total modules (limit is 3)");
            }

            await Task.Delay(50, cancellationToken);

            _executingModules.TryTake(out _);
            return moduleName;
        }
    }

    [ModularPipelines.Attributes.ParallelLimiter<MyParallelLimit>]
    public class Module5 : Module<string>
    {
        protected override async Task<string?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            var moduleName = GetType().Name;

            _executingModules.Add(moduleName);
            await Task.Delay(50, cancellationToken);

            if (_executingModules.Count > 3)
            {
                _violations.Add($"{moduleName} executing with {_executingModules.Count} total modules (limit is 3)");
            }

            await Task.Delay(50, cancellationToken);

            _executingModules.TryTake(out _);
            return moduleName;
        }
    }

    [ModularPipelines.Attributes.ParallelLimiter<MyParallelLimit>]
    public class Module6 : Module<string>
    {
        protected override async Task<string?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            var moduleName = GetType().Name;

            _executingModules.Add(moduleName);
            await Task.Delay(50, cancellationToken);

            if (_executingModules.Count > 3)
            {
                _violations.Add($"{moduleName} executing with {_executingModules.Count} total modules (limit is 3)");
            }

            await Task.Delay(50, cancellationToken);

            _executingModules.TryTake(out _);
            return moduleName;
        }
    }

    [Test]
    public async Task LimitParallel()
    {
        _executingModules.Clear();
        _violations.Clear();

        var timeProvider = new FakeTimeProvider();

        await TestPipelineHostBuilder.Create(new TestHostSettings(), timeProvider)
            .AddModule<Module1>()
            .AddModule<Module2>()
            .AddModule<Module3>()
            .AddModule<Module4>()
            .AddModule<Module5>()
            .AddModule<Module6>()
            .ExecutePipelineAsync();

        await Assert.That(_violations).IsEmpty();
    }

    private record MyParallelLimit : IParallelLimit
    {
        public int Limit => 3;
    }
}