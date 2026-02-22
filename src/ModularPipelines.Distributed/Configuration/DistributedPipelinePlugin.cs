using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModularPipelines.Distributed.Artifacts;
using ModularPipelines.Distributed.Coordination;
using ModularPipelines.Distributed.Master;
using ModularPipelines.Distributed.Serialization;
using ModularPipelines.Distributed.Worker;
using ModularPipelines.Engine;
using ModularPipelines.Plugins;

namespace ModularPipelines.Distributed.Configuration;

internal class DistributedPipelinePlugin : IModularPipelinesPlugin
{
    public string Name => "ModularPipelines.Distributed";

    public int Priority => 100;

    public void ConfigureServices(IServiceCollection services)
    {
        // Check if distributed options are registered
        var sp = services.BuildServiceProvider();
        var options = sp.GetService<IOptions<DistributedOptions>>();

        if (options?.Value.Enabled != true)
        {
            return;
        }

        var distributedOptions = options.Value;

        // Register shared services
        var typeRegistry = new ModuleTypeRegistry();
        services.AddSingleton(typeRegistry);
        services.AddSingleton(new ModuleResultSerializer(typeRegistry));

        // Register coordinator: prefer explicit registration, then factory, then fallback to in-memory
        var hasCoordinator = services.Any(d => d.ServiceType == typeof(IDistributedCoordinator));
        var hasFactory = services.Any(d => d.ServiceType == typeof(IDistributedCoordinatorFactory));
        if (!hasCoordinator && !hasFactory)
        {
            services.AddSingleton<IDistributedCoordinator, InMemoryDistributedCoordinator>();

            // Warn that InMemoryDistributedCoordinator is only suitable for single-process testing
            var tempSp = services.BuildServiceProvider();
            var logger = tempSp.GetService<ILoggerFactory>()?.CreateLogger<DistributedPipelinePlugin>();
            logger?.LogWarning(
                "No IDistributedCoordinator or IDistributedCoordinatorFactory was registered. " +
                "Using InMemoryDistributedCoordinator which is only suitable for single-process testing. " +
                "Register a real coordinator (e.g., Redis, HTTP) for multi-process distributed execution.");
        }
        else if (hasFactory)
        {
            RemoveService<IDistributedCoordinator>(services);
            services.AddSingleton<IDistributedCoordinator>(sp =>
            {
                var factory = sp.GetRequiredService<IDistributedCoordinatorFactory>();
                return factory.CreateAsync(CancellationToken.None).GetAwaiter().GetResult();
            });
        }

        // Register artifact store: prefer explicit registration, then factory, then fallback to in-memory
        var hasArtifactStore = services.Any(d => d.ServiceType == typeof(IDistributedArtifactStore));
        var hasArtifactFactory = services.Any(d => d.ServiceType == typeof(IDistributedArtifactStoreFactory));
        if (!hasArtifactStore && !hasArtifactFactory)
        {
            services.AddSingleton<IDistributedArtifactStore, InMemoryDistributedArtifactStore>();
        }
        else if (hasArtifactFactory)
        {
            RemoveService<IDistributedArtifactStore>(services);
            services.AddSingleton<IDistributedArtifactStore>(sp2 =>
            {
                var factory = sp2.GetRequiredService<IDistributedArtifactStoreFactory>();
                return factory.CreateAsync(CancellationToken.None).GetAwaiter().GetResult();
            });
        }

        // Register artifact options if not already registered
        if (!services.Any(d => d.ServiceType == typeof(IOptions<ArtifactOptions>)))
        {
            services.Configure<ArtifactOptions>(_ => { });
        }

        // Register lifecycle manager and context
        services.AddSingleton<ArtifactLifecycleManager>();
        services.AddSingleton<IArtifactContext>(sp2 =>
            new ArtifactContextImpl(
                sp2.GetRequiredService<IDistributedArtifactStore>(),
                sp2.GetRequiredService<IOptions<ArtifactOptions>>(),
                string.Empty));

        var roleDetector = new RoleDetector(options);
        var role = roleDetector.DetectRole();

        if (role == DistributedRole.Master)
        {
            // Master services
            services.AddSingleton<DistributedWorkPublisher>();
            services.AddSingleton<DistributedResultCollector>();
            services.AddSingleton<DistributedSummaryAggregator>();
            services.AddHostedService<WorkerHealthMonitor>();

            // Replace the default module executor with the distributed one
            RemoveService<IModuleExecutor>(services);
            services.AddSingleton<IModuleExecutor, DistributedModuleExecutor>();
        }
        else
        {
            // Worker services
            services.AddHostedService<WorkerHeartbeatService>();
            services.AddHostedService<WorkerCancellationMonitor>();

            // Replace the default module executor with the worker one
            RemoveService<IModuleExecutor>(services);
            services.AddSingleton<IModuleExecutor, WorkerModuleExecutor>();
        }
    }

    public void ConfigurePipeline(PipelineBuilder pipelineBuilder)
    {
        // Pipeline-level configuration (e.g., matrix expansion) will be added in later phases
    }

    private static void RemoveService<T>(IServiceCollection services)
    {
        var descriptors = services.Where(d => d.ServiceType == typeof(T)).ToList();
        foreach (var descriptor in descriptors)
        {
            services.Remove(descriptor);
        }
    }
}
