using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;
using MEL.Spectre;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Caching;
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
public sealed class PipelineBuilder
{
    private readonly IHostBuilder _hostBuilder;
    private readonly ServiceCollection _services;
    private readonly ServiceCollection _loggingServices;
    private readonly ConfigurationManager _configuration;
    private readonly PipelineHostEnvironment _environment;
    private readonly PipelineBuilderResources _resources;
    private readonly PipelineCommandLineOptions _commandLineOptions;
    private readonly PipelineBuilderSettings _settings;
    private readonly ServiceDescriptor[] _defaultLoggingServiceDescriptors;
    private readonly ServiceDescriptor[] _defaultLoggingProviderDescriptors;
    private readonly HashSet<Type> _defaultLoggingProviderTypes;
    private PipelineOptions _options;

    internal PipelineBuilder(
        PipelineBuilderSettings settings,
        string? projectInferenceSourcePath = null)
    {
        _settings = settings;

        _commandLineOptions = settings.EnableCommandLineOptions
            ? PipelineCommandLineParser.Parse(settings.Args)
            : PipelineCommandLineOptions.Empty with
            {
                HostArguments = settings.Args?.ToArray() ?? [],
            };
        var args = _commandLineOptions.HostArguments.ToArray();
        _services = new ServiceCollection();
        _loggingServices = new ServiceCollection();
        DependencyInjectionSetup.RegisterDefaultLogging(_loggingServices);
        _defaultLoggingServiceDescriptors = _loggingServices.ToArray();
        _defaultLoggingProviderDescriptors = _defaultLoggingServiceDescriptors
            .Where(static descriptor => descriptor.ServiceType == typeof(ILoggerProvider))
            .ToArray();
        _defaultLoggingProviderTypes = _defaultLoggingProviderDescriptors
            .Select(static descriptor => descriptor.ImplementationType)
            .OfType<Type>()
            .ToHashSet();
        Logging = new PipelineLoggingBuilder(_loggingServices);
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

        _environment = CreateHostEnvironment(settings, args, projectInferenceSourcePath);
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
    /// Gets the logging builder for configuring pipeline logging.
    /// </summary>
    public ILoggingBuilder Logging { get; }

    /// <summary>
    /// Gets the configuration manager for adding and reading configuration.
    /// </summary>
    public ConfigurationManager Configuration => _configuration;

    /// <summary>
    /// Gets the current immutable pipeline options snapshot.
    /// </summary>
    public PipelineOptions Options => _options;

    /// <summary>
    /// Replaces the current pipeline options snapshot.
    /// </summary>
    /// <param name="configureOptions">A function that returns the configured options.</param>
    /// <returns>The same builder instance for chaining.</returns>
    public PipelineBuilder ConfigureOptions(Func<PipelineOptions, PipelineOptions> configureOptions)
    {
        ArgumentNullException.ThrowIfNull(configureOptions);
        _options = configureOptions(_options)
            ?? throw new InvalidOperationException("The pipeline options configuration returned null.");
        return this;
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
    /// Builds the pipeline and validates configuration.
    /// </summary>
    /// <returns>A validated pipeline ready for execution.</returns>
    /// <exception cref="PipelineValidationException">Thrown when validation fails.</exception>
    public async Task<IPipeline> BuildAsync()
    {
        if (_commandLineOptions.Command == PipelineCommand.ExportGraph)
        {
            return await BuildForDependencyGraphExportAsync().ConfigureAwait(false);
        }

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

    internal Task<IPipeline> BuildForDependencyGraphExportAsync() =>
        BuildPipelineAsync(initializePipeline: false);

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
            or ModuleSelfDependencyException
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
        PipelineBuilderSettings settings,
        IReadOnlyList<string> hostArguments,
        string? projectInferenceSourcePath)
    {
        var hostConfiguration = new ConfigurationManager();
        hostConfiguration.AddEnvironmentVariables(prefix: "DOTNET_");
        if (hostArguments.Count > 0)
        {
            hostConfiguration.AddCommandLine([.. hostArguments]);
        }

        var environmentName = FirstNonEmpty(
            Environments.Production,
            settings.EnvironmentName,
            hostConfiguration[HostDefaults.EnvironmentKey],
            System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"));
        var configuredWorkingDirectory = FirstNonEmpty(
            string.Empty,
            settings.WorkingDirectory,
            settings.ContentRootPath,
            hostConfiguration[HostDefaults.ContentRootKey]);
        var workingDirectory = Path.GetFullPath(
            !string.IsNullOrEmpty(configuredWorkingDirectory)
                ? configuredWorkingDirectory
                : FirstNonEmpty(
                    Directory.GetCurrentDirectory(),
                    projectInferenceSourcePath is null
                        ? null
                        : PipelineDirectory.TryFindPipelineProject(projectInferenceSourcePath)));
        var contentRootPath = Path.GetFullPath(FirstNonEmpty(
            workingDirectory,
            settings.ContentRootPath,
            hostConfiguration[HostDefaults.ContentRootKey]));
        var applicationName = FirstNonEmpty(
            string.Empty,
            settings.ApplicationName,
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
        LoadModularPipelinesAssembliesIfNotLoadedYet();

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
            services.AddSingleton(new PipelineWorkingDirectory(_environment.WorkingDirectory));
            DependencyInjectionSetup.Initialize(services);
            services.Configure<ModuleCacheOptions>(options =>
                options.WorkingDirectory = _environment.WorkingDirectory);

            foreach (var defaultProvider in _defaultLoggingProviderDescriptors
                         .Where(provider => !_loggingServices.Contains(provider)))
            {
                foreach (var descriptor in services
                             .Where(descriptor => IsMatchingLoggingProvider(
                                 descriptor,
                                 defaultProvider))
                             .ToArray())
                {
                    services.Remove(descriptor);
                }
            }

            // Add user-registered services before plugins so plugins can inspect user configuration
            foreach (var descriptor in _services)
            {
                services.Add(descriptor);
            }

            foreach (var descriptor in _loggingServices
                         .Where(descriptor => !_defaultLoggingServiceDescriptors.Contains(descriptor)))
            {
                services.Add(descriptor);
            }

            // Apply plugin services after user services
            PluginIntegration.ApplyPluginServices(services);

            if (!HasDefaultLoggingProvider(services) || !UsesDefaultLoggerFactory(services))
            {
                services.Replace(ServiceDescriptor.Singleton<
                    ISpectreConsoleLoggerControl,
                    Console.NoopSpectreConsoleLoggerControl>());
            }

            // Activate distributed mode if configured (replaces executor based on role)
            ActivateDistributedModeIfConfigured(services);

            services
                .AddSingleton(_commandLineOptions)
                .AddSingleton(_options)
                .AddSingleton(provider => new FixedOptions<PipelineOptions>(
                    _options,
                    ClonePipelineOptions,
                    provider.GetServices<IConfigureOptions<PipelineOptions>>(),
                    provider.GetServices<IPostConfigureOptions<PipelineOptions>>(),
                    provider.GetServices<IValidateOptions<PipelineOptions>>()))
                .AddSingleton<IOptions<PipelineOptions>>(provider =>
                    provider.GetRequiredService<FixedOptions<PipelineOptions>>())
                .AddSingleton<IOptionsSnapshot<PipelineOptions>>(provider =>
                    provider.GetRequiredService<FixedOptions<PipelineOptions>>())
                .AddSingleton<IOptionsMonitor<PipelineOptions>>(provider =>
                    provider.GetRequiredService<FixedOptions<PipelineOptions>>())
                .AddSingleton<IOptions<SecretMaskingOptions>>(provider =>
                    Microsoft.Extensions.Options.Options.Create(
                        provider.GetRequiredService<IOptions<PipelineOptions>>().Value.Secrets));

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

    private void LoadModularPipelinesAssembliesIfNotLoadedYet()
    {
        if (!RuntimeFeature.IsDynamicCodeSupported)
        {
            return;
        }

        var coreVersion = typeof(PipelineBuilder).Assembly.GetName().Version;
        LoadReferencedModularPipelinesAssemblies(coreVersion);

        if (!_settings.LoadModularPipelinesAssemblies)
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

    private static void LoadReferencedModularPipelinesAssemblies(Version? coreVersion)
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

    private sealed class PipelineLoggingBuilder(IServiceCollection services) : ILoggingBuilder
    {
        public IServiceCollection Services { get; } = services;
    }

    private bool HasDefaultLoggingProvider(IServiceCollection services)
        => services.Any(IsDefaultLoggingProvider);

    private static bool UsesDefaultLoggerFactory(IServiceCollection services)
    {
        var descriptor = services.LastOrDefault(static service =>
            service.ServiceType == typeof(ILoggerFactory));
        return GetImplementationType(descriptor) == typeof(LoggerFactory);
    }

    private static bool IsMatchingLoggingProvider(
        ServiceDescriptor candidate,
        ServiceDescriptor expected) =>
        candidate.ServiceType == typeof(ILoggerProvider)
        && GetImplementationType(candidate) == GetImplementationType(expected);

    private bool IsDefaultLoggingProvider(ServiceDescriptor descriptor)
    {
        if (descriptor.ServiceType != typeof(ILoggerProvider))
        {
            return false;
        }

        var implementationType = GetImplementationType(descriptor);
        return implementationType is not null
               && _defaultLoggingProviderTypes.Contains(implementationType);
    }

    private static Type? GetImplementationType(ServiceDescriptor? descriptor) =>
        descriptor?.ImplementationType ?? descriptor?.ImplementationInstance?.GetType();

    private static PipelineOptions ClonePipelineOptions(PipelineOptions options) => options with
    {
        RunReport = options.RunReport with { },
        Console = options.Console with { },
        Http = options.Http with
        {
            Logging = options.Http.Logging is null
                ? null
                : options.Http.Logging with { },
            Resilience = options.Http.Resilience is null
                ? null
                : options.Http.Resilience with { },
        },
        Commands = options.Commands with
        {
            Logging = options.Commands.Logging is null
                ? null
                : options.Commands.Logging with { },
        },
        Secrets = options.Secrets with { },
        Concurrency = options.Concurrency with { },
    };

    private sealed class FixedOptions<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] T>
        : IOptions<T>, IOptionsSnapshot<T>, IOptionsMonitor<T>
        where T : class
    {
        private readonly T _initialValue;
        private readonly Func<T, T> _clone;
        private readonly IConfigureOptions<T>[] _configurations;
        private readonly IPostConfigureOptions<T>[] _postConfigurations;
        private readonly IValidateOptions<T>[] _validators;
        private readonly ConcurrentDictionary<string, Lazy<T>> _namedValues = new(StringComparer.Ordinal);
        private readonly Lazy<T> _value;

        public FixedOptions(
            T value,
            Func<T, T> clone,
            IEnumerable<IConfigureOptions<T>> configurations,
            IEnumerable<IPostConfigureOptions<T>> postConfigurations,
            IEnumerable<IValidateOptions<T>> validators)
        {
            _initialValue = value;
            _clone = clone;
            _configurations = configurations.ToArray();
            _postConfigurations = postConfigurations.ToArray();
            _validators = validators.ToArray();
            _value = new Lazy<T>(() => ConfigureAndValidate(
                Microsoft.Extensions.Options.Options.DefaultName,
                _clone(value),
                _configurations,
                _postConfigurations,
                _validators));
        }

        public T Value => _value.Value;

        public T CurrentValue => Value;

        public T Get(string? name)
        {
            name ??= Microsoft.Extensions.Options.Options.DefaultName;
            if (name == Microsoft.Extensions.Options.Options.DefaultName)
            {
                return Value;
            }

            return _namedValues.GetOrAdd(
                name,
                optionName => new Lazy<T>(() => ConfigureAndValidate(
                    optionName,
                    _clone(_initialValue),
                    _configurations,
                    _postConfigurations,
                    _validators))).Value;
        }

        public IDisposable? OnChange(Action<T, string?> listener) => NoopDisposable.Instance;

        private static T ConfigureAndValidate(
            string name,
            T value,
            IEnumerable<IConfigureOptions<T>> configurations,
            IEnumerable<IPostConfigureOptions<T>> postConfigurations,
            IEnumerable<IValidateOptions<T>> validators)
        {
            foreach (var configuration in configurations)
            {
                if (configuration is IConfigureNamedOptions<T> namedConfiguration)
                {
                    namedConfiguration.Configure(name, value);
                }
                else if (name == Microsoft.Extensions.Options.Options.DefaultName)
                {
                    configuration.Configure(value);
                }
            }

            foreach (var postConfiguration in postConfigurations)
            {
                postConfiguration.PostConfigure(name, value);
            }

            var failures = validators
                .Select(validator => validator.Validate(name, value))
                .Where(static result => result.Failed)
                .SelectMany(static result => result.Failures ?? [])
                .ToArray();

            return failures.Length == 0
                ? value
                : throw new OptionsValidationException(
                    name,
                    typeof(T),
                    failures);
        }
    }

    private sealed class NoopDisposable : IDisposable
    {
        public static NoopDisposable Instance { get; } = new();

        public void Dispose()
        {
        }
    }
}
