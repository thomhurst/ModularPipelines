using Initialization.Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.DependencyInjection;

namespace ModularPipelines.UnitTests.Engine;

public class PipelineServiceInitializerTests
{
    public interface IFactoryService;

    private sealed class InitializationLog
    {
        public List<string> Entries { get; } = [];
    }

    private sealed class TypeInitializer(InitializationLog log) : IInitializer
    {
        public int Order => 1;

        public Task InitializeAsync()
        {
            lock (log)
            {
                log.Entries.Add(nameof(TypeInitializer));
            }

            return Task.CompletedTask;
        }
    }

    private sealed class EarlyInstanceInitializer(InitializationLog log) : IInitializer
    {
        public int Order => 0;

        public Task InitializeAsync()
        {
            lock (log)
            {
                log.Entries.Add(nameof(EarlyInstanceInitializer));
            }

            return Task.CompletedTask;
        }
    }

    private sealed class FactoryInitializer(InitializationLog log) : IFactoryService, IInitializer
    {
        public int Order => 2;

        public Task InitializeAsync()
        {
            lock (log)
            {
                log.Entries.Add(nameof(FactoryInitializer));
            }

            return Task.CompletedTask;
        }
    }

    private sealed class ScopedInitializer : IInitializer
    {
        public Task InitializeAsync() => Task.CompletedTask;
    }

    private sealed class NamedInitializer(InitializationLog log, string name) : IInitializer
    {
        public Task InitializeAsync()
        {
            lock (log)
            {
                log.Entries.Add(name);
            }

            return Task.CompletedTask;
        }
    }

    private abstract class InitializerBase(InitializationLog log) : IInitializer
    {
        public Task InitializeAsync()
        {
            lock (log)
            {
                log.Entries.Add(GetType().Name);
            }

            return Task.CompletedTask;
        }
    }

    private sealed class InheritedFactoryInitializer(InitializationLog log) : InitializerBase(log), IFactoryService;

    private sealed class PendingInitializer(Task completion) : IInitializer
    {
        public Task InitializeAsync() => completion;
    }

    private sealed class SynchronouslyThrowingInitializer : IInitializer
    {
        public Task InitializeAsync() => throw new InvalidOperationException("sync failure");
    }

    [Test]
    public async Task InitializesInstanceTypeAndFactoryRegistrationsInOrder()
    {
        var log = new InitializationLog();
        var services = CreateServices();
        services.AddSingleton(log);
        services.AddSingleton(new EarlyInstanceInitializer(log));
        services.AddSingleton<TypeInitializer>();
        services.AddSingleton<IFactoryService>(serviceProvider =>
            new FactoryInitializer(serviceProvider.GetRequiredService<InitializationLog>()));

        await using var serviceProvider = services.BuildServiceProvider();
        await PipelineServiceInitializer.InitializeAsync(serviceProvider);

        await Assert.That(string.Join(",", log.Entries)).IsEqualTo(
            $"{nameof(EarlyInstanceInitializer)},{nameof(TypeInitializer)},{nameof(FactoryInitializer)}");
    }

    [Test]
    public async Task DoesNotResolveFactoriesThatCannotProduceInitializers()
    {
        var resolved = false;
        var services = CreateServices();
        services.AddSingleton<InitializationLog>(_ =>
        {
            resolved = true;
            return new InitializationLog();
        });

        await using var serviceProvider = services.BuildServiceProvider();
        await PipelineServiceInitializer.InitializeAsync(serviceProvider);

        await Assert.That(resolved).IsFalse();
    }

    [Test]
    public async Task RejectsNonSingletonInitializers()
    {
        var services = CreateServices();
        services.AddScoped<ScopedInitializer>();

        await using var serviceProvider = services.BuildServiceProvider();

        var exception = await Assert.That(() => PipelineServiceInitializer.InitializeAsync(serviceProvider))
            .Throws<InvalidOperationException>();

        await Assert.That(exception!.Message).Contains("only supported for Singletons");
    }

    [Test]
    public async Task InitializesEachRegistrationOfASharedServiceTypeOnce()
    {
        var log = new InitializationLog();
        var services = CreateServices();
        services.AddSingleton<IInitializer>(new NamedInitializer(log, "first"));
        services.AddSingleton<IInitializer>(_ => new NamedInitializer(log, "second"));
        services.AddSingleton<IInitializer>(_ => new NamedInitializer(log, "third"));

        await using var serviceProvider = services.BuildServiceProvider();
        await PipelineServiceInitializer.InitializeAsync(serviceProvider);

        await Assert.That(string.Join(",", log.Entries.Order(StringComparer.Ordinal)))
            .IsEqualTo("first,second,third");
    }

    [Test]
    public async Task InitializesForwardedSingletonOnce()
    {
        var log = new InitializationLog();
        var services = CreateServices();
        services.AddSingleton(log);
        services.AddSingleton<TypeInitializer>();
        services.AddSingleton<IInitializer>(serviceProvider => serviceProvider.GetRequiredService<TypeInitializer>());

        await using var serviceProvider = services.BuildServiceProvider();
        await PipelineServiceInitializer.InitializeAsync(serviceProvider);

        await Assert.That(log.Entries).HasSingleItem();
    }

    [Test]
    public async Task InitializesFactoryWhoseInitializerIsInheritedFromABaseClass()
    {
        var log = new InitializationLog();
        var services = CreateServices();
        services.AddSingleton<IFactoryService>(_ => new InheritedFactoryInitializer(log));

        await using var serviceProvider = services.BuildServiceProvider();
        await PipelineServiceInitializer.InitializeAsync(serviceProvider);

        await Assert.That(string.Join(",", log.Entries)).IsEqualTo(nameof(InheritedFactoryInitializer));
    }

    [Test]
    public async Task ScansLoadedTypesOnceForSeveralFactoryRegistrations()
    {
        var log = new InitializationLog();
        var scans = 0;
        var services = CreateServices();
        services.AddSingleton<IFactoryService>(_ => new FactoryInitializer(log));
        services.AddSingleton<IFactoryService>(_ => new InheritedFactoryInitializer(log));
        services.AddSingleton<IDisposable>(_ => new CancellationTokenSource());

        await using var serviceProvider = services.BuildServiceProvider();
        await PipelineServiceInitializer.InitializeAsync(serviceProvider, () =>
        {
            scans++;
            return [typeof(FactoryInitializer), typeof(InheritedFactoryInitializer)];
        });

        using (Assert.Multiple())
        {
            await Assert.That(scans).IsEqualTo(1);
            await Assert.That(log.Entries.Count).IsEqualTo(2);
        }
    }

    [Test]
    public async Task AwaitsStartedInitializersWhenAnotherThrowsSynchronously()
    {
        var pending = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var services = CreateServices();
        services.AddSingleton<IInitializer>(new PendingInitializer(pending.Task));
        services.AddSingleton<IInitializer>(new SynchronouslyThrowingInitializer());

        await using var serviceProvider = services.BuildServiceProvider();
        var initialization = PipelineServiceInitializer.InitializeAsync(serviceProvider);

        await Assert.That(initialization.IsCompleted).IsFalse();

        pending.SetResult();
        var exception = await Assert.That(() => initialization).Throws<InvalidOperationException>();
        await Assert.That(exception!.Message).IsEqualTo("sync failure");
    }

    private static ServiceCollection CreateServices()
    {
        var services = new ServiceCollection();
        services.AddSingleton<IPipelineServiceContainerWrapper>(new PipelineServiceContainerWrapper(services));
        return services;
    }
}
