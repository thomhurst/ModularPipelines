using System.Reflection;
using Initialization.Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.DependencyInjection;

namespace ModularPipelines.UnitTests.Engine;

public class PipelineServiceInitializerTests
{
    public interface IFactoryService;

    public interface IOtherFactoryService;

    public interface ILateFactoryService;

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

    private sealed class LateLoadSignal
    {
        public bool Loaded { get; set; }
    }

    private sealed class LoadingInitializer : IInitializer
    {
        public LoadingInitializer(LateLoadSignal signal)
        {
            signal.Loaded = true;
        }

        public Task InitializeAsync() => Task.CompletedTask;
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

    private sealed class InheritedFactoryInitializer(InitializationLog log) : InitializerBase(log), IFactoryService, IOtherFactoryService;

    private sealed class LateFactoryInitializer(InitializationLog log) : InitializerBase(log), ILateFactoryService;

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
    public async Task ReadsEachLoadedAssemblyOnceForSeveralFactoryRegistrations()
    {
        var log = new InitializationLog();
        var dynamicAssemblyName = $"InitializerScan_{Guid.NewGuid():N}";
        var dynamicAssembly = System.Reflection.Emit.AssemblyBuilder.DefineDynamicAssembly(
            new AssemblyName(dynamicAssemblyName),
            System.Reflection.Emit.AssemblyBuilderAccess.RunAndCollect);
        dynamicAssembly.DefineDynamicModule(dynamicAssemblyName);
        var reads = new Dictionary<Assembly, int>();
        var loadedInitializerTypes = new PipelineServiceInitializer.LoadedInitializerTypes(assembly =>
        {
            lock (reads)
            {
                reads[assembly] = reads.GetValueOrDefault(assembly) + 1;
            }

            return assembly == typeof(PipelineServiceInitializerTests).Assembly
                ? [typeof(FactoryInitializer), typeof(InheritedFactoryInitializer)]
                : [];
        });
        var services = CreateServices();
        services.AddSingleton<IFactoryService>(_ => new FactoryInitializer(log));
        services.AddSingleton<IOtherFactoryService>(_ => new InheritedFactoryInitializer(log));
        services.AddSingleton<IDisposable>(_ => new CancellationTokenSource());
        services.AddSingleton<IAsyncDisposable>(_ => new MemoryStream());

        await using var serviceProvider = services.BuildServiceProvider();
        await PipelineServiceInitializer.InitializeAsync(serviceProvider, loadedInitializerTypes);

        using (Assert.Multiple())
        {
            await Assert.That(reads.All(read => read.Value == 1)).IsTrue();
            await Assert.That(reads.Keys.Any(assembly => assembly.IsDynamic
                                                         && assembly.GetName().Name == dynamicAssemblyName)).IsTrue();
            await Assert.That(reads.ContainsKey(typeof(PipelineServiceInitializerTests).Assembly)).IsTrue();
            await Assert.That(log.Entries.Count).IsEqualTo(2);
        }
    }

    [Test]
    public async Task ChecksFactoriesAfterResolvingInstanceAndTypeInitializers()
    {
        var log = new InitializationLog();
        var signal = new LateLoadSignal();
        var loadedInitializerTypes = new PipelineServiceInitializer.LoadedInitializerTypes(
            _ => signal.Loaded ? [typeof(FactoryInitializer)] : []);
        var services = CreateServices();
        services.AddSingleton<IFactoryService>(_ => new FactoryInitializer(log));
        services.AddSingleton(signal);
        services.AddSingleton<LoadingInitializer>();

        await using var serviceProvider = services.BuildServiceProvider();
        await PipelineServiceInitializer.InitializeAsync(serviceProvider, loadedInitializerTypes);

        await Assert.That(string.Join(",", log.Entries)).IsEqualTo(nameof(FactoryInitializer));
    }

    [Test]
    public async Task RereadsDynamicAssembliesAfterResolvingAFactory()
    {
        var log = new InitializationLog();
        var emitted = false;
        var dynamicAssemblyName = $"InitializerEmit_{Guid.NewGuid():N}";
        var dynamicAssembly = System.Reflection.Emit.AssemblyBuilder.DefineDynamicAssembly(
            new AssemblyName(dynamicAssemblyName),
            System.Reflection.Emit.AssemblyBuilderAccess.RunAndCollect);
        dynamicAssembly.DefineDynamicModule(dynamicAssemblyName);

        // Stands in for a factory that emits a new initializer type into a dynamic assembly
        // which was already scanned earlier in the same initialization.
        var loadedInitializerTypes = new PipelineServiceInitializer.LoadedInitializerTypes(assembly =>
            assembly == typeof(PipelineServiceInitializerTests).Assembly
                ? [typeof(InheritedFactoryInitializer)]
                : assembly.IsDynamic && assembly.GetName().Name == dynamicAssemblyName && Volatile.Read(ref emitted)
                    ? [typeof(LateFactoryInitializer)]
                    : []);
        var services = CreateServices();

        // Matches nothing, so its check reads (and caches) every assembly before any resolution.
        services.AddSingleton<IDisposable>(_ => new CancellationTokenSource());
        services.AddSingleton<IOtherFactoryService>(_ =>
        {
            Volatile.Write(ref emitted, true);
            return new InheritedFactoryInitializer(log);
        });
        services.AddSingleton<ILateFactoryService>(_ => new LateFactoryInitializer(log));

        await using var serviceProvider = services.BuildServiceProvider();
        await PipelineServiceInitializer.InitializeAsync(serviceProvider, loadedInitializerTypes);

        await Assert.That(string.Join(",", log.Entries.Order(StringComparer.Ordinal)))
            .IsEqualTo($"{nameof(InheritedFactoryInitializer)},{nameof(LateFactoryInitializer)}");
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

        await Assert.That(string.Join(",", log.Entries)).IsEqualTo(nameof(TypeInitializer));
    }

    [Test]
    public async Task AwaitsStartedInitializersWhenAnotherThrowsSynchronously()
    {
        var pending = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var services = CreateServices();
        services.AddSingleton(new PendingInitializer(pending.Task));
        services.AddSingleton(new SynchronouslyThrowingInitializer());

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
