using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using ModularPipelines.Distributed.Abstractions;
using ModularPipelines.Distributed.Caching;
using ModularPipelines.Distributed.Communication;
using ModularPipelines.Distributed.Engine;
using ModularPipelines.Distributed.Options;
using ModularPipelines.Distributed.Registry;
using ModularPipelines.Distributed.Serialization;
using ModularPipelines.Distributed.Services;
using ModularPipelines.Host;

namespace ModularPipelines.Distributed.Extensions;

/// <summary>
/// Extension methods for configuring distributed execution in ModularPipelines.
/// </summary>
public static class PipelineHostBuilderExtensions
{
    /// <summary>
    /// Adds distributed execution support to the pipeline.
    /// </summary>
    /// <param name="builder">The pipeline host builder.</param>
    /// <param name="configureOptions">Optional action to configure distributed pipeline options.</param>
    /// <returns>The pipeline host builder for chaining.</returns>
    public static PipelineHostBuilder AddDistributedExecution(
        this PipelineHostBuilder builder,
        Action<DistributedPipelineOptions>? configureOptions = null)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ConfigureServices((context, services) =>
        {
            // Register options
            if (configureOptions != null)
            {
                services.Configure(configureOptions);
            }

            // Validate options on startup
            services.AddSingleton<IValidateOptions<DistributedPipelineOptions>, DistributedPipelineOptionsValidator>();

            // Register core serialization services
            services.AddSingleton<ModuleSerializer>();
            services.AddSingleton<ContextSerializer>();

            // Register default implementations (can be overridden)
            services.AddSingleton<IResultCache, MemoryResultCache>();
            services.AddSingleton<INodeRegistry, HttpNodeRegistry>();
            services.AddSingleton<IDistributedScheduler, DistributedScheduler>();

            // Register HTTP client for remote communication
            services.AddHttpClient("ModularPipelines.Distributed")
                .ConfigureHttpClient(client =>
                {
                    client.DefaultRequestHeaders.Add("User-Agent", "ModularPipelines.Distributed/1.0");
                });

            services.AddSingleton<IRemoteCommunicator, HttpRemoteCommunicator>();

            // Register distributed executor
            services.AddSingleton<DistributedModuleExecutor>();

            // Register execution node factory
            services.AddSingleton<IExecutionNodeFactory, ExecutionNodeFactory>();

            // Register worker execution handler (will be used if in worker mode)
            services.AddSingleton<WorkerModuleExecutionHandler>();
        });

        return builder;
    }

    /// <summary>
    /// Configures the pipeline for orchestrator mode.
    /// </summary>
    /// <param name="builder">The pipeline host builder.</param>
    /// <param name="port">The port to listen on for worker connections.</param>
    /// <returns>The pipeline host builder for chaining.</returns>
    public static PipelineHostBuilder AsOrchestrator(
        this PipelineHostBuilder builder,
        int port = 8080)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ConfigureServices((context, services) =>
        {
            services.Configure<DistributedPipelineOptions>(options =>
            {
                options.Mode = DistributedExecutionMode.Orchestrator;
                options.OrchestratorPort = port;
            });

            // Register background service for stale worker cleanup
            services.AddHostedService<StaleWorkerCleanupService>();

            // Register orchestrator HTTP API service
            services.AddHostedService<OrchestratorApiService>();
        });

        return builder;
    }

    /// <summary>
    /// Configures the pipeline for worker mode.
    /// </summary>
    /// <param name="builder">The pipeline host builder.</param>
    /// <param name="orchestratorUrl">The URL of the orchestrator.</param>
    /// <param name="configureCapabilities">Action to configure worker capabilities.</param>
    /// <returns>The pipeline host builder for chaining.</returns>
    public static PipelineHostBuilder AsWorker(
        this PipelineHostBuilder builder,
        string orchestratorUrl,
        Action<WorkerCapabilities>? configureCapabilities = null)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentException.ThrowIfNullOrWhiteSpace(orchestratorUrl);

        builder.ConfigureServices((context, services) =>
        {
            var capabilities = new WorkerCapabilities();
            configureCapabilities?.Invoke(capabilities);

            services.Configure<DistributedPipelineOptions>(options =>
            {
                options.Mode = DistributedExecutionMode.Worker;
                options.OrchestratorUrl = orchestratorUrl;
                options.WorkerCapabilities = capabilities;
            });

            // Register background service for sending heartbeats
            services.AddHostedService<WorkerHeartbeatService>();

            // Register worker HTTP API service
            services.AddHostedService<WorkerApiService>();
        });

        return builder;
    }

    /// <summary>
    /// Runs the pipeline as a worker node, listening for execution requests from the orchestrator.
    /// This method blocks until the application is shut down.
    /// </summary>
    /// <param name="builder">The pipeline host builder.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A task that represents the worker's lifetime.</returns>
    public static async Task RunWorkerAsync(
        this PipelineHostBuilder builder,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(builder);

        // Build the host (uses internal BuildHostAsync method)
        await using var host = await builder.BuildHostAsync();

        // Run the host (this blocks until shutdown)
        await host.RunAsync(cancellationToken);
    }
}

/// <summary>
/// Validates distributed pipeline options.
/// </summary>
internal sealed class DistributedPipelineOptionsValidator : IValidateOptions<DistributedPipelineOptions>
{
    public ValidateOptionsResult Validate(string? name, DistributedPipelineOptions options)
    {
        if (options.Mode == DistributedExecutionMode.Worker)
        {
            if (string.IsNullOrWhiteSpace(options.OrchestratorUrl))
            {
                return ValidateOptionsResult.Fail(
                    "OrchestratorUrl must be specified when Mode is Worker");
            }

            if (options.WorkerCapabilities == null)
            {
                return ValidateOptionsResult.Fail(
                    "WorkerCapabilities must be specified when Mode is Worker");
            }

            if (options.WorkerPort <= 0 || options.WorkerPort > 65535)
            {
                return ValidateOptionsResult.Fail(
                    "WorkerPort must be between 1 and 65535");
            }
        }

        if (options.Mode == DistributedExecutionMode.Orchestrator)
        {
            if (options.OrchestratorPort <= 0 || options.OrchestratorPort > 65535)
            {
                return ValidateOptionsResult.Fail(
                    "OrchestratorPort must be between 1 and 65535");
            }
        }

        if (options.WorkerHeartbeatTimeout <= TimeSpan.Zero)
        {
            return ValidateOptionsResult.Fail(
                "WorkerHeartbeatTimeout must be greater than zero");
        }

        if (options.WorkerHeartbeatInterval <= TimeSpan.Zero)
        {
            return ValidateOptionsResult.Fail(
                "WorkerHeartbeatInterval must be greater than zero");
        }

        if (options.RemoteExecutionTimeout <= TimeSpan.Zero)
        {
            return ValidateOptionsResult.Fail(
                "RemoteExecutionTimeout must be greater than zero");
        }

        if (options.MaxRetryAttempts < 0)
        {
            return ValidateOptionsResult.Fail(
                "MaxRetryAttempts cannot be negative");
        }

        if (options.MaxPayloadSize <= 0)
        {
            return ValidateOptionsResult.Fail(
                "MaxPayloadSize must be greater than zero");
        }

        return ValidateOptionsResult.Success;
    }
}

/// <summary>
/// Factory for creating execution nodes.
/// </summary>
internal interface IExecutionNodeFactory
{
    /// <summary>
    /// Creates a local execution node.
    /// </summary>
    LocalExecutionNode CreateLocalNode();

    /// <summary>
    /// Creates a remote execution node for the specified worker.
    /// </summary>
    RemoteExecutionNode CreateRemoteNode(WorkerNode worker);

    /// <summary>
    /// Creates execution nodes for all available workers plus a local node.
    /// </summary>
    Task<IReadOnlyList<IExecutionNode>> CreateAllNodesAsync(CancellationToken cancellationToken = default);
}

/// <summary>
/// Default implementation of execution node factory.
/// </summary>
internal sealed class ExecutionNodeFactory : IExecutionNodeFactory
{
    private readonly IServiceProvider _serviceProvider;
    private readonly INodeRegistry _nodeRegistry;
    private readonly IOptions<DistributedPipelineOptions> _options;

    public ExecutionNodeFactory(
        IServiceProvider serviceProvider,
        INodeRegistry nodeRegistry,
        IOptions<DistributedPipelineOptions> options)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        _nodeRegistry = nodeRegistry ?? throw new ArgumentNullException(nameof(nodeRegistry));
        _options = options ?? throw new ArgumentNullException(nameof(options));
    }

    public LocalExecutionNode CreateLocalNode()
    {
        return ActivatorUtilities.CreateInstance<LocalExecutionNode>(_serviceProvider);
    }

    public RemoteExecutionNode CreateRemoteNode(WorkerNode worker)
    {
        return ActivatorUtilities.CreateInstance<RemoteExecutionNode>(_serviceProvider, worker);
    }

    public async Task<IReadOnlyList<IExecutionNode>> CreateAllNodesAsync(
        CancellationToken cancellationToken = default)
    {
        var nodes = new List<IExecutionNode>();

        // Add local node if not in Worker-only mode
        if (_options.Value.Mode != DistributedExecutionMode.Worker)
        {
            nodes.Add(CreateLocalNode());
        }

        // Add remote nodes for all available workers
        if (_options.Value.Mode == DistributedExecutionMode.Orchestrator)
        {
            var workers = await _nodeRegistry.GetAvailableWorkersAsync(cancellationToken);

            foreach (var worker in workers)
            {
                nodes.Add(CreateRemoteNode(worker));
            }
        }

        return nodes;
    }
}
