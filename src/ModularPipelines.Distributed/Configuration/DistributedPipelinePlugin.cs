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
        // Resolve DistributedOptions without BuildServiceProvider() by inspecting registrations directly
        var distributedOptions = ResolveOptionsFromDescriptors(services);

        if (distributedOptions is null || !distributedOptions.Enabled)
        {
            return;
        }

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

            // Defer the warning log to resolution time (avoids BuildServiceProvider)
            services.AddSingleton<InMemoryCoordinatorWarning>();
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

        var roleDetector = new RoleDetector(Microsoft.Extensions.Options.Options.Create(distributedOptions));
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

    /// <summary>
    /// Extracts DistributedOptions from the service collection without calling BuildServiceProvider().
    /// The AddDistributedMode extension registers IOptions&lt;DistributedOptions&gt; as a singleton instance.
    /// </summary>
    private static DistributedOptions? ResolveOptionsFromDescriptors(IServiceCollection services)
    {
        // Look for the IOptions<DistributedOptions> singleton instance registration
        var optionsDescriptor = services.FirstOrDefault(d =>
            d.ServiceType == typeof(IOptions<DistributedOptions>) &&
            d.Lifetime == ServiceLifetime.Singleton &&
            d.ImplementationInstance is not null);

        if (optionsDescriptor?.ImplementationInstance is IOptions<DistributedOptions> options)
        {
            return options.Value;
        }

        // Check for IConfigureOptions<DistributedOptions> (from Configure<T>() calls)
        var hasConfigureOptions = services.Any(d =>
            d.ServiceType == typeof(IConfigureOptions<DistributedOptions>) ||
            d.ServiceType == typeof(IPostConfigureOptions<DistributedOptions>));

        if (hasConfigureOptions)
        {
            // Build the options manually by invoking the configure actions
            var opts = new DistributedOptions();
            foreach (var descriptor in services.Where(d =>
                d.ServiceType == typeof(IConfigureOptions<DistributedOptions>) &&
                d.ImplementationInstance is IConfigureOptions<DistributedOptions>))
            {
                ((IConfigureOptions<DistributedOptions>)descriptor.ImplementationInstance!).Configure(opts);
            }

            foreach (var descriptor in services.Where(d =>
                d.ServiceType == typeof(IPostConfigureOptions<DistributedOptions>) &&
                d.ImplementationInstance is IPostConfigureOptions<DistributedOptions>))
            {
                ((IPostConfigureOptions<DistributedOptions>)descriptor.ImplementationInstance!).PostConfigure(string.Empty, opts);
            }

            return opts;
        }

        return null;
    }

    private static void RemoveService<T>(IServiceCollection services)
    {
        var descriptors = services.Where(d => d.ServiceType == typeof(T)).ToList();
        foreach (var descriptor in descriptors)
        {
            services.Remove(descriptor);
        }
    }

    /// <summary>
    /// Singleton that logs a warning when resolved, replacing the BuildServiceProvider() pattern.
    /// </summary>
    private sealed class InMemoryCoordinatorWarning
    {
        public InMemoryCoordinatorWarning(ILogger<DistributedPipelinePlugin> logger)
        {
            logger.LogWarning(
                "No IDistributedCoordinator or IDistributedCoordinatorFactory was registered. " +
                "Using InMemoryDistributedCoordinator which is only suitable for single-process testing. " +
                "Register a real coordinator (e.g., Redis, HTTP) for multi-process distributed execution.");
        }
    }
}
