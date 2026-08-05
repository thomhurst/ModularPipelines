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
using ModularPipelines.PipelineCli;
using ModularPipelines.Plugins;
using ModularPipelines.Validation;

namespace ModularPipelines;

/// <summary>
/// A builder for configuring and creating a pipeline.
/// </summary>
/// <remarks>
/// Resources created while configuring the builder are transferred to the built pipeline.
/// </remarks>
public sealed class PipelineBuilder : IDisposable
{
    private readonly IHostBuilder _hostBuilder;
    private readonly ServiceCollection _services;
    private readonly ConfigurationManager _configuration;
    private readonly PipelineHostEnvironment _environment;
    private readonly PipelineBuilderResources _resources;
    private readonly PipelineCommandLineOptions _commandLineOptions;
    private PipelineOptions _options;

    internal PipelineBuilder(PipelineBuilderOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        _commandLineOptions = options.EnableCommandLineOptions
            ? PipelineCommandLineParser.Parse(options.Args)
            : PipelineCommandLineOptions.Empty with
            {
                HostArguments = options.Args?.ToArray() ?? [],
            };
        var args = _commandLineOptions.HostArguments.ToArray();
        _services = new ServiceCollection();
        _configuration = new ConfigurationManager();
        _options = new PipelineOptions
        {
            DryRun = _commandLineOptions.Command == PipelineCommand.DryRun,
            DisableModuleCache = _commandLineOptions.DisableModuleCache,
            TargetModules = NullIfEmpty(_commandLineOptions.TargetModules),
            SkippedModules = NullIfEmpty(_commandLineOptions.SkippedModules),
            RunOnlyCategories = NullIfEmpty(_commandLineOptions.RunOnlyCategories),
            IgnoreCategories = NullIfEmpty(_commandLineOptions.IgnoreCategories),
        };

        _hostBuilder = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args);

        // Add default configuration sources
        _configuration.AddEnvironmentVariables();
        if (args.Length > 0)
        {
            _configuration.AddCommandLine(args);
        }

        _environment = CreateHostEnvironment(options, args);
        _resources = _environment.Resources;
        _configuration.SetBasePath(_environment.WorkingDirectory);
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
    /// Gets the current immutable pipeline options snapshot.
    /// </summary>
    /// <remarks>
    /// Use <see cref="PipelineBuilderExtensions.ConfigurePipelineOptions(PipelineBuilder, Func{PipelineOptions, PipelineOptions})"/>
    /// to replace this snapshot.
    /// </remarks>
    public PipelineOptions Options => _options;

    internal void SetOptions(PipelineOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        _options = options;
    }

    /// <summary>
    /// Gets the host environment information.
    /// </summary>
    public IHostEnvironment Environment => _environment;

    /// <summary>
    /// Gets the default working directory for commands and relative file paths.
    /// </summary>
    public string WorkingDirectory => _environment.WorkingDirectory;

    /// <summary>
    /// Retained for source compatibility. Resources are owned by the built pipeline.
    /// </summary>
    public void Dispose()
    {
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
    /// Replaces the categories whose modules should be run exclusively.
    /// </summary>
    /// <param name="categories">The complete set of categories to run.</param>
    /// <returns>The same builder instance for chaining.</returns>
    /// <remarks>Calling this method again replaces the previous category filter.</remarks>
    public PipelineBuilder RunOnlyCategories(params string[] categories)
    {
        ArgumentNullException.ThrowIfNull(categories);
        _options = _options with
        {
            RunOnlyCategories = NullIfEmpty(categories),
        };

        return this;
    }

    /// <summary>
    /// Replaces the categories whose modules should be ignored.
    /// </summary>
    /// <param name="categories">The complete set of categories to ignore.</param>
    /// <returns>The same builder instance for chaining.</returns>
    /// <remarks>Calling this method again replaces the previous category filter.</remarks>
    public PipelineBuilder IgnoreCategories(params string[] categories)
    {
        ArgumentNullException.ThrowIfNull(categories);
        _options = _options with
        {
            IgnoreCategories = NullIfEmpty(categories),
        };

        return this;
    }

    /// <summary>
    /// Builds the pipeline and validates configuration.
    /// </summary>
    /// <returns>A validated pipeline ready for execution.</returns>
    /// <exception cref="PipelineValidationException">Thrown when validation fails.</exception>
    public async Task<IPipeline> BuildAsync()
    {
        var validatePipeline = _commandLineOptions.Command != PipelineCommand.Help;
        var (pipeline, validationResult, validationException) =
            await BuildAndValidatePipelineAsync(validatePipeline).ConfigureAwait(false);

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
            await BuildAndValidatePipelineAsync(validatePipeline: true).ConfigureAwait(false);

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
        BuildAndValidatePipelineAsync(bool validatePipeline)
    {
        IPipeline? pipeline = null;

        try
        {
            pipeline = await BuildPipelineAsync(initializePipeline: validatePipeline).ConfigureAwait(false);
            var validationResult = validatePipeline
                ? await ValidatePipelineAsync(pipeline.Services).ConfigureAwait(false)
                : ValidationResult.Success();
            return (pipeline, validationResult, null);
        }
        catch (NoModulesRegisteredException ex)
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

    private static PipelineHostEnvironment CreateHostEnvironment(
        PipelineBuilderOptions options,
        IReadOnlyList<string> hostArguments)
    {
        var hostConfiguration = new ConfigurationManager();
        hostConfiguration.AddEnvironmentVariables(prefix: "DOTNET_");
        if (hostArguments.Count > 0)
        {
            hostConfiguration.AddCommandLine([.. hostArguments]);
        }

        var environmentName = FirstNonEmpty(
            Environments.Production,
            options.EnvironmentName,
            hostConfiguration[HostDefaults.EnvironmentKey],
            System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"));
        var workingDirectory = Path.GetFullPath(FirstNonEmpty(
            Directory.GetCurrentDirectory(),
            options.WorkingDirectory,
            options.ContentRootPath,
            hostConfiguration[HostDefaults.ContentRootKey]));
        var contentRootPath = Path.GetFullPath(FirstNonEmpty(
            workingDirectory,
            options.ContentRootPath,
            hostConfiguration[HostDefaults.ContentRootKey]));
        var applicationName = FirstNonEmpty(
            string.Empty,
            options.ApplicationName,
            hostConfiguration[HostDefaults.ApplicationKey],
            Assembly.GetEntryAssembly()?.GetName().Name);

        var resources = new PipelineBuilderResources(contentRootPath);
        return new PipelineHostEnvironment(resources)
        {
            ApplicationName = applicationName,
            EnvironmentName = environmentName,
            ContentRootPath = contentRootPath,
            WorkingDirectory = workingDirectory,
        };
    }

    private static string FirstNonEmpty(string fallback, params string?[] candidates)
    {
        return candidates.FirstOrDefault(static value => !string.IsNullOrEmpty(value)) ?? fallback;
    }

    private static IReadOnlyList<string>? NullIfEmpty(IReadOnlyList<string> values) =>
        values.Count == 0 ? null : values;

    private async Task<IPipeline> BuildPipelineAsync(bool initializePipeline)
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

            services
                .AddSingleton(_commandLineOptions)
                .AddSingleton(new PipelineWorkingDirectory(_environment.WorkingDirectory))
                .AddSingleton(_options)
                .AddTransient<IOptionsFactory<PipelineOptions>, PipelineOptionsFactory>();

            // Auto-register any missing required dependencies
            ModuleAutoRegistrar.AutoRegisterMissingDependencies(services);

            foreach (var contextAttribute in AppDomain.CurrentDomain.GetAssemblies()
                         .SelectMany(static assembly => assembly.GetCustomAttributes<ModularPipelinesContextAttribute>())
                         .OrderBy(static attribute => attribute.ContextType.AssemblyQualifiedName, StringComparer.Ordinal))
            {
                contextAttribute.Register(services);
            }
        });

        return await PipelineImpl.CreateAsync(_hostBuilder, _resources, initializePipeline).ConfigureAwait(false);
    }

    private void LoadModularPipelineAssembliesIfNotLoadedYet()
    {
        if (!RuntimeFeature.IsDynamicCodeSupported)
        {
            return;
        }

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

    private sealed class PipelineHostEnvironment(PipelineBuilderResources resources) : IHostEnvironment
    {
        internal PipelineBuilderResources Resources { get; } = resources;

        public string EnvironmentName { get; set; } = string.Empty;

        public string ApplicationName { get; set; } = string.Empty;

        public string ContentRootPath { get; set; } = string.Empty;

        internal string WorkingDirectory { get; init; } = string.Empty;

        public IFileProvider ContentRootFileProvider
        {
            get => Resources.ContentRootFileProvider;
            set => Resources.ContentRootFileProvider = value;
        }
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
