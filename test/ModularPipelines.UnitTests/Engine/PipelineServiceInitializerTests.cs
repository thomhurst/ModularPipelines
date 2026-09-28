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

    private static ServiceCollection CreateServices()
    {
        var services = new ServiceCollection();
        services.AddSingleton<IPipelineServiceContainerWrapper>(new PipelineServiceContainerWrapper(services));
        return services;
    }
}
