using System.Reflection;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModularPipelines.Attributes;
using ModularPipelines.DependencyInjection;
using ModularPipelines.Distributed;
using ModularPipelines.Distributed.Artifacts;
using ModularPipelines.Distributed.Configuration;
using ModularPipelines.Distributed.Coordination;
using ModularPipelines.Distributed.Master;
using ModularPipelines.Distributed.Serialization;
using ModularPipelines.Distributed.Worker;
using ModularPipelines.Engine;
using ModularPipelines.Exceptions;
using ModularPipelines.Options;
using ModularPipelines.Plugins;
using ModularPipelines.Validation;

namespace ModularPipelines;

/// <summary>
/// A builder for configuring and creating a pipeline.
/// </summary>
/// <remarks>
/// The caller owns the builder and should dispose it when finished, especially after using
/// <see cref="IHostEnvironment.ContentRootFileProvider"/>.
/// </remarks>
public sealed class PipelineBuilder : IDisposable
{
    private readonly IHostBuilder _hostBuilder;
    private readonly ServiceCollection _services;
    private readonly ConfigurationManager _configuration;
    private readonly PipelineOptions _options;
    private readonly IHostEnvironment _environment;

    internal Type? LastRegisteredModuleType { get; set; }

    internal PipelineBuilder(string[]? args)
        : this(new PipelineBuilderOptions { Args = args })
    {
    }

    internal PipelineBuilder(PipelineBuilderOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        _services = new ServiceCollection();
        _configuration = new ConfigurationManager();
        _options = new PipelineOptions();

        _hostBuilder = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(options.Args);

        // Add default configuration sources
        _configuration.AddEnvironmentVariables();
        if (options.Args != null)
        {
            _configuration.AddCommandLine(options.Args);
        }

        _environment = CreateHostEnvironment(options);
        _hostBuilder.UseEnvironment(_environment.EnvironmentName);
        _hostBuilder.UseContentRoot(_environment.ContentRootPath);

        if (!string.IsNullOrEmpty(_environment.ApplicationName))
        {
            _hostBuilder.ConfigureHostConfiguration(configuration =>
                configuration.AddInMemoryCollection(new Dictionary<string, string?>
                {
                    [HostDefaults.ApplicationKey] = _environment.ApplicationName,
                }));
        }
    }

    /// <summary>
    /// Gets the service collection for registering application services.
    /// </summary>
    public IServiceCollection Services => _services;

    /// <summary>
    /// Gets the configuration manager for adding and reading configuration.
    /// </summary>
    public ConfigurationManager Configuration => _configuration;

    /// <summary>
    /// Gets the pipeline options for configuring execution behavior.
    /// </summary>
    public PipelineOptions Options => _options;

    /// <summary>
    /// Gets the host environment information.
    /// </summary>
    public IHostEnvironment Environment => _environment;

    /// <summary>
    /// Releases resources owned by the cached host environment.
    /// </summary>
    public void Dispose()
    {
        (_environment.ContentRootFileProvider as IDisposable)?.Dispose();
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Sets the minimum log level for the pipeline.
    /// </summary>
    /// <param name="logLevel">The minimum log level.</param>
    /// <returns>The same builder instance for chaining.</returns>
    public PipelineBuilder SetLogLevel(LogLevel logLevel)
    {
        _services.Configure<LoggerFilterOptions>(options => options.MinLevel = logLevel);
        return this;
    }

    /// <summary>
    /// Configures modules with the specified categories to be run exclusively.
    /// </summary>
    /// <param name="categories">The categories to run.</param>
    /// <returns>The same builder instance for chaining.</returns>
    public PipelineBuilder RunCategories(params string[] categories)
    {
        _options.RunOnlyCategories ??= new List<string>();
        foreach (var category in categories)
        {
            _options.RunOnlyCategories.Add(category);
        }

        return this;
    }

    /// <summary>
    /// Configures modules with the specified categories to be ignored.
    /// </summary>
    /// <param name="categories">The categories to ignore.</param>
    /// <returns>The same builder instance for chaining.</returns>
    public PipelineBuilder IgnoreCategories(params string[] categories)
    {
        _options.IgnoreCategories ??= new List<string>();
        foreach (var category in categories)
        {
            _options.IgnoreCategories.Add(category);
        }

        return this;
    }

    /// <summary>
    /// Builds the pipeline and validates configuration.
    /// </summary>
    /// <returns>A validated pipeline ready for execution.</returns>
    /// <exception cref="PipelineValidationException">Thrown when validation fails.</exception>
    public async Task<IPipeline> BuildAsync()
    {
        var (pipeline, validationResult, validationException) =
            await BuildAndValidatePipelineAsync().ConfigureAwait(false);

        if (validationResult.HasErrors)
        {
            if (pipeline != null)
            {
                await pipeline.DisposeAsync().ConfigureAwait(false);
            }

            throw new PipelineValidationException(validationResult, validationException);
        }

        return pipeline!;
    }

    /// <summary>
    /// Validates the pipeline configuration without executing it.
    /// </summary>
    /// <returns>A validation result containing any errors found.</returns>
    public async Task<ValidationResult> ValidateAsync()
    {
        var (pipeline, validationResult, _) =
            await BuildAndValidatePipelineAsync().ConfigureAwait(false);

        try
        {
            return validationResult;
        }
        finally
        {
            if (pipeline != null)
            {
                await pipeline.DisposeAsync().ConfigureAwait(false);
            }
        }
    }

    private async Task<(IPipeline? Pipeline, ValidationResult ValidationResult, Exception? ValidationException)>
        BuildAndValidatePipelineAsync()
    {
        IPipeline? pipeline = null;

        try
        {
            pipeline = await BuildPipelineAsync().ConfigureAwait(false);
            var validationResult = await ValidatePipelineAsync(pipeline.Services).ConfigureAwait(false);
            return (pipeline, validationResult, null);
        }
        catch (PipelineException ex) when (ex.Message.Contains("No modules"))
        {
            return (
                pipeline,
                ValidationResult.WithError(new ValidationError(
                    ValidationErrorCategory.ModuleConfiguration,
                    "No modules are registered. A pipeline must have at least one module.")),
                ex);
        }
        catch (Exception ex) when (ex is ModuleNotRegisteredException
            or ModuleReferencingSelfException
            or DependencyCollisionException)
        {
            return (pipeline, CreateDependencyValidationResult(ex), ex);
        }
        catch
        {
            if (pipeline != null)
            {
                await pipeline.DisposeAsync().ConfigureAwait(false);
            }

            throw;
        }
    }

    private static ValidationResult CreateDependencyValidationResult(Exception exception)
    {
        return ValidationResult.WithError(new ValidationError(
            ValidationErrorCategory.Dependency,
            exception.Message));
    }

    private static Task<ValidationResult> ValidatePipelineAsync(IServiceProvider services)
    {
        var validationService = services.GetService<IPipelineValidationService>();
        return validationService?.ValidateAsync(services)
               ?? Task.FromResult(ValidationResult.Success());
    }

    private static IHostEnvironment CreateHostEnvironment(PipelineBuilderOptions options)
    {
        var hostConfiguration = new ConfigurationManager();
        hostConfiguration.AddEnvironmentVariables(prefix: "DOTNET_");
        if (options.Args is not null)
        {
            hostConfiguration.AddCommandLine(options.Args);
        }

        var environmentName = FirstNonEmpty(
            Environments.Production,
            options.EnvironmentName,
            hostConfiguration[HostDefaults.EnvironmentKey],
            System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"));
        var contentRootPath = Path.GetFullPath(FirstNonEmpty(
            Directory.GetCurrentDirectory(),
            options.ContentRootPath,
            hostConfiguration[HostDefaults.ContentRootKey]));
        var applicationName = FirstNonEmpty(
            string.Empty,
            options.ApplicationName,
            hostConfiguration[HostDefaults.ApplicationKey],
            Assembly.GetEntryAssembly()?.GetName().Name);

        return new PipelineHostEnvironment
        {
            ApplicationName = applicationName,
            EnvironmentName = environmentName,
            ContentRootPath = contentRootPath,
            ContentRootFileProvider = new PhysicalFileProvider(contentRootPath),
        };
    }

    private static string FirstNonEmpty(string fallback, params string?[] candidates)
    {
        return candidates.FirstOrDefault(static value => !string.IsNullOrEmpty(value)) ?? fallback;
    }

    private async Task<IPipeline> BuildPipelineAsync()
    {
        LoadModularPipelineAssembliesIfNotLoadedYet();

        // Apply plugin configuration to the builder (modules, hooks, options)
        PluginIntegration.ApplyPluginConfiguration(this);

        // Configure the host with our collected configuration
        _hostBuilder.ConfigureAppConfiguration((_, config) =>
        {
            config.AddConfiguration(_configuration);
        });

        // Configure services: core first, then user services, then plugins (so plugins can inspect user config)
        _hostBuilder.ConfigureServices((_, services) =>
        {
            DependencyInjectionSetup.Initialize(services);

            // Add user-registered services before plugins so plugins can inspect user configuration
            foreach (var descriptor in _services)
            {
                services.Add(descriptor);
            }

            // Apply plugin services after user services
            PluginIntegration.ApplyPluginServices(services);

            // Activate distributed mode if configured (replaces executor based on role)
            ActivateDistributedModeIfConfigured(services);

            // Configure pipeline options
            services.Configure<PipelineOptions>(opts =>
            {
                opts.ExecutionMode = _options.ExecutionMode;
                opts.DefaultModuleTimeout = _options.DefaultModuleTimeout;
                opts.RunOnlyCategories = _options.RunOnlyCategories;
                opts.IgnoreCategories = _options.IgnoreCategories;
                opts.ShowProgressInConsole = _options.ShowProgressInConsole;
                opts.PrintResults = _options.PrintResults;
                opts.PrintLogo = _options.PrintLogo;
                opts.PrintDependencyChains = _options.PrintDependencyChains;
                opts.LoadModularPipelineAssemblies = _options.LoadModularPipelineAssemblies;
                opts.DefaultRetryCount = _options.DefaultRetryCount;
                opts.DefaultLoggingOptions = _options.DefaultLoggingOptions;
                opts.DefaultHttpLoggingOptions = _options.DefaultHttpLoggingOptions;
                opts.DefaultHttpTimeout = _options.DefaultHttpTimeout;
                opts.DefaultHttpResilienceOptions = _options.DefaultHttpResilienceOptions;
                opts.Concurrency = _options.Concurrency;
                opts.ConsoleWidth = _options.ConsoleWidth;
                opts.DefaultExecutionOptions = _options.DefaultExecutionOptions;
                opts.ThrowOnPipelineFailure = _options.ThrowOnPipelineFailure;
                opts.ModuleOutputFlushInterval = _options.ModuleOutputFlushInterval;
                opts.ModuleOutputFlushThreshold = _options.ModuleOutputFlushThreshold;
            });

            // Auto-register any missing required dependencies
            ModuleAutoRegistrar.AutoRegisterMissingDependencies(services);

            foreach (var contextAttribute in AppDomain.CurrentDomain.GetAssemblies()
                         .SelectMany(static assembly => assembly.GetCustomAttributes<ModularPipelinesContextAttribute>())
                         .OrderBy(static attribute => attribute.ContextType.AssemblyQualifiedName, StringComparer.Ordinal))
            {
                contextAttribute.Register(services);
            }
        });

        return await PipelineImpl.CreateAsync(_hostBuilder).ConfigureAwait(false);
    }

    private void LoadModularPipelineAssembliesIfNotLoadedYet()
    {
        var coreVersion = typeof(PipelineBuilder).Assembly.GetName().Version;
        LoadReferencedModularPipelineAssemblies(coreVersion);

        if (!_options.LoadModularPipelineAssemblies)
        {
            return;
        }

        var currentAssemblies = AppDomain.CurrentDomain.GetAssemblies();

        var unloadedModularPipelineAssemblies = GetDlls()
            .Select(Path.GetFileNameWithoutExtension)
            .Except(currentAssemblies.Select(x => x.GetName().Name))
            .OfType<string>()
            .ToList();

        foreach (var modularPipelineAssembly in unloadedModularPipelineAssemblies)
        {
            LoadAndInitializeAssembly(new AssemblyName(modularPipelineAssembly), coreVersion);
        }
    }

    private static void LoadReferencedModularPipelineAssemblies(Version? coreVersion)
    {
        ReferencedAssemblyTraversal.LoadModularPipelinesAssemblies(
            AppDomain.CurrentDomain.GetAssemblies(),
            assemblyName => LoadAndInitializeAssembly(assemblyName, coreVersion));
    }

    private static Assembly LoadAndInitializeAssembly(AssemblyName assemblyName, Version? coreVersion)
    {
        var assembly = Assembly.Load(assemblyName);
        PluginVersionValidator.Validate(assembly, coreVersion);
        RuntimeHelpers.RunModuleConstructor(assembly.ManifestModule.ModuleHandle);
        return assembly;
    }

    private static IEnumerable<string> GetDlls()
    {
        var baseDirectoryDlls = Directory.EnumerateFiles(AppDomain.CurrentDomain.BaseDirectory, "*ModularPipeline*.dll", SearchOption.TopDirectoryOnly);

        if (string.IsNullOrEmpty(AppDomain.CurrentDomain.DynamicDirectory))
        {
            return baseDirectoryDlls;
        }

        return baseDirectoryDlls
            .Concat(Directory.EnumerateFiles(AppDomain.CurrentDomain.DynamicDirectory, "*ModularPipeline*.dll", SearchOption.TopDirectoryOnly))
            .Distinct();
    }

    /// <summary>
    /// Activates distributed execution mode if configured with TotalInstances > 1.
    /// Replaces the default <see cref="IModuleExecutor"/> with a role-specific implementation.
    /// </summary>
    private static void ActivateDistributedModeIfConfigured(IServiceCollection services)
    {
        var options = ResolveDistributedOptions(services);
        if (options is null || !options.Enabled || options.TotalInstances <= 1)
        {
            return;
        }

        var roleDetector = new RoleDetector(Microsoft.Extensions.Options.Options.Create(options));
        var role = roleDetector.DetectRole();

        // Replace coordinator if factory registered — deferred so workers don't block
        // during DI build waiting for the master to advertise its URL
        var hasFactory = services.Any(d => d.ServiceType == typeof(IDistributedCoordinatorFactory));
        if (hasFactory)
        {
            RemoveService<IDistributedCoordinator>(services);
            services.AddSingleton<IDistributedCoordinator>(sp =>
            {
                var factory = sp.GetRequiredService<IDistributedCoordinatorFactory>();
                return new DeferredCoordinator(factory);
            });
        }

        // Replace artifact store if factory registered — deferred for same reason
        var hasArtifactFactory = services.Any(d => d.ServiceType == typeof(IDistributedArtifactStoreFactory));
        if (hasArtifactFactory)
        {
            RemoveService<IDistributedArtifactStore>(services);
            services.AddSingleton<IDistributedArtifactStore>(sp =>
            {
                var factory = sp.GetRequiredService<IDistributedArtifactStoreFactory>();
                return new DeferredArtifactStore(factory);
            });
        }

        if (role == DistributedRole.Master)
        {
            services.AddSingleton<DistributedWorkPublisher>();
            services.AddSingleton<DistributedResultCollector>();
            RemoveService<IModuleExecutor>(services);
            services.AddSingleton<IModuleExecutor, DistributedModuleExecutor>();
        }
        else
        {
            RemoveService<IModuleExecutor>(services);
            services.AddSingleton<IModuleExecutor, WorkerModuleExecutor>();
        }
    }

    /// <summary>
    /// Extracts DistributedOptions from the service collection without calling BuildServiceProvider().
    /// </summary>
    private static DistributedOptions? ResolveDistributedOptions(IServiceCollection services)
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
            var opts = new DistributedOptions();
            foreach (var descriptor in services.Where(d =>
                d.ServiceType == typeof(IConfigureOptions<DistributedOptions>) &&
                d.ImplementationInstance is IConfigureOptions<DistributedOptions>))
            {
                ((IConfigureOptions<DistributedOptions>) descriptor.ImplementationInstance!).Configure(opts);
            }

            foreach (var descriptor in services.Where(d =>
                d.ServiceType == typeof(IPostConfigureOptions<DistributedOptions>) &&
                d.ImplementationInstance is IPostConfigureOptions<DistributedOptions>))
            {
                ((IPostConfigureOptions<DistributedOptions>) descriptor.ImplementationInstance!).PostConfigure(string.Empty, opts);
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

    private sealed class PipelineHostEnvironment : IHostEnvironment
    {
        public string EnvironmentName { get; set; } = string.Empty;

        public string ApplicationName { get; set; } = string.Empty;

        public string ContentRootPath { get; set; } = string.Empty;

        public IFileProvider ContentRootFileProvider { get; set; } = null!;
    }

    /// <summary>
    /// Defers <see cref="IDistributedCoordinatorFactory.CreateAsync"/> to first use so that
    /// workers don't block during DI build waiting for the master to advertise its URL.
    /// </summary>
    private sealed class DeferredCoordinator(IDistributedCoordinatorFactory factory) : IDistributedCoordinator
    {
        private readonly SemaphoreSlim _lock = new(1, 1);
        private volatile IDistributedCoordinator? _inner;

        public async Task EnqueueModuleAsync(ModuleAssignment a, CancellationToken ct) => await (await GetAsync(ct)).EnqueueModuleAsync(a, ct);

        public async Task<ModuleAssignment?> DequeueModuleAsync(IReadOnlySet<string> c, CancellationToken ct) => await (await GetAsync(ct)).DequeueModuleAsync(c, ct);

        public async Task PublishResultAsync(SerializedModuleResult r, CancellationToken ct) => await (await GetAsync(ct)).PublishResultAsync(r, ct);

        public async Task<SerializedModuleResult> WaitForResultAsync(string m, CancellationToken ct) => await (await GetAsync(ct)).WaitForResultAsync(m, ct);

        public async Task RegisterWorkerAsync(WorkerRegistration r, CancellationToken ct) => await (await GetAsync(ct)).RegisterWorkerAsync(r, ct);

        public async Task<IReadOnlyList<WorkerRegistration>> GetRegisteredWorkersAsync(CancellationToken ct) => await (await GetAsync(ct)).GetRegisteredWorkersAsync(ct);

        public async Task SignalCompletionAsync(CancellationToken ct) => await (await GetAsync(ct)).SignalCompletionAsync(ct);

        private async ValueTask<IDistributedCoordinator> GetAsync(CancellationToken ct)
        {
            if (_inner is not null)
            {
                return _inner;
            }

            await _lock.WaitAsync(ct);
            try
            {
                return _inner ??= await factory.CreateAsync(ct);
            }
            finally
            {
                _lock.Release();
            }
        }
    }

    /// <summary>
    /// Defers <see cref="IDistributedArtifactStoreFactory.CreateAsync"/> to first use.
    /// </summary>
    private sealed class DeferredArtifactStore(IDistributedArtifactStoreFactory factory) : IDistributedArtifactStore
    {
        private readonly SemaphoreSlim _lock = new(1, 1);
        private volatile IDistributedArtifactStore? _inner;

        public async Task<ArtifactReference> UploadAsync(ArtifactDescriptor d, Stream s, CancellationToken ct) => await (await GetAsync(ct)).UploadAsync(d, s, ct);

        public async Task<Stream> DownloadAsync(ArtifactReference r, CancellationToken ct) => await (await GetAsync(ct)).DownloadAsync(r, ct);

        public async Task<IReadOnlyList<ArtifactReference>> ListArtifactsAsync(string m, CancellationToken ct) => await (await GetAsync(ct)).ListArtifactsAsync(m, ct);

        public async Task DeleteAsync(ArtifactReference r, CancellationToken ct) => await (await GetAsync(ct)).DeleteAsync(r, ct);

        private async ValueTask<IDistributedArtifactStore> GetAsync(CancellationToken ct)
        {
            if (_inner is not null)
            {
                return _inner;
            }

            await _lock.WaitAsync(ct);
            try
            {
                return _inner ??= await factory.CreateAsync(ct);
            }
            finally
            {
                _lock.Release();
            }
        }
    }
}
