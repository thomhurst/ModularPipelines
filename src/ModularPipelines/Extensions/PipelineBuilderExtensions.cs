using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ModularPipelines.Caching;
using ModularPipelines.Engine;
using ModularPipelines.Enums;
using ModularPipelines.Interfaces;
using ModularPipelines.Modules;
using ModularPipelines.Options;
using ModularPipelines.Requirements;

namespace ModularPipelines.Extensions;

/// <summary>
/// Convenience extension methods for PipelineBuilder that delegate to Services.
/// </summary>
public static class PipelineBuilderExtensions
{
    /// <summary>
    /// Adds a Module to the pipeline.
    /// </summary>
    /// <param name="builder">The pipeline builder.</param>
    /// <typeparam name="TModule">The type of Module to add.</typeparam>
    /// <returns>A typed registration handle for configuring module metadata.</returns>
    public static ModuleRegistration<TModule> AddModule<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TModule>(
        this PipelineBuilder builder)
        where TModule : class, IModule
    {
        builder.Services.AddModule<TModule>();
        return new ModuleRegistration<TModule>(builder);
    }

    /// <summary>
    /// Adds a pre-created Module instance to the pipeline.
    /// </summary>
    /// <param name="builder">The pipeline builder.</param>
    /// <param name="module">The module instance to add.</param>
    /// <typeparam name="TModule">The type of Module to add.</typeparam>
    /// <returns>A typed registration handle for configuring module metadata.</returns>
    public static ModuleRegistration<TModule> AddModule<TModule>(this PipelineBuilder builder, TModule module)
        where TModule : class, IModule
    {
        builder.Services.AddModule(module);
        return new ModuleRegistration<TModule>(builder);
    }

    /// <summary>
    /// Adds a Module to the pipeline using a factory method.
    /// </summary>
    /// <param name="builder">The pipeline builder.</param>
    /// <param name="factory">A factory method for creating the module.</param>
    /// <typeparam name="TModule">The type of Module to add.</typeparam>
    /// <returns>A typed registration handle for configuring module metadata.</returns>
    public static ModuleRegistration<TModule> AddModule<TModule>(this PipelineBuilder builder, Func<IServiceProvider, TModule> factory)
        where TModule : class, IModule
    {
        builder.Services.AddModule(factory);
        return new ModuleRegistration<TModule>(builder);
    }

    /// <summary>
    /// Adds multiple module types to the pipeline.
    /// </summary>
    /// <param name="builder">The pipeline builder.</param>
    /// <param name="moduleTypes">The module types to add.</param>
    /// <returns>The same builder instance for chaining.</returns>
    [RequiresUnreferencedCode(
        "Runtime type module registration relies on reflection. Use AddModule<TModule>() for trim-safe registration.")]
    [RequiresDynamicCode(
        "Runtime type module registration may require runtime code generation. Use AddModule<TModule>() for Native AOT.")]
    public static PipelineBuilder AddModules(this PipelineBuilder builder, params Type[] moduleTypes)
    {
        ArgumentNullException.ThrowIfNull(moduleTypes);

        foreach (var moduleType in moduleTypes)
        {
            ValidateModuleType(moduleType);
            builder.Services.AddModule(moduleType);
        }

        return builder;
    }

    /// <summary>
    /// Adds all modules from the assembly containing the specified type.
    /// </summary>
    /// <typeparam name="T">Any type from the assembly to scan.</typeparam>
    /// <param name="builder">The pipeline builder.</param>
    /// <returns>The same builder instance for chaining.</returns>
    [RequiresUnreferencedCode("Module discovery scans all types in an assembly.")]
    public static PipelineBuilder AddModulesFromAssemblyContainingType<T>(this PipelineBuilder builder)
    {
        return builder.AddModulesFromAssembly(typeof(T).Assembly);
    }

    /// <summary>
    /// Adds all modules from an assembly.
    /// </summary>
    /// <param name="builder">The pipeline builder.</param>
    /// <param name="assembly">The assembly to scan.</param>
    /// <returns>The same builder instance for chaining.</returns>
    [RequiresUnreferencedCode("Module discovery scans all types in an assembly.")]
    public static PipelineBuilder AddModulesFromAssembly(this PipelineBuilder builder, Assembly assembly)
    {
        ArgumentNullException.ThrowIfNull(assembly);
        builder.Services.AddModulesFromAssembly(assembly);
        return builder;
    }

    /// <summary>
    /// Adds a requirement to the pipeline.
    /// </summary>
    /// <param name="builder">The pipeline builder.</param>
    /// <typeparam name="TRequirement">The type of requirement to add.</typeparam>
    /// <returns>The same builder instance for chaining.</returns>
    public static PipelineBuilder AddRequirement<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TRequirement>(
        this PipelineBuilder builder)
        where TRequirement : class, IPipelineRequirement
    {
        builder.Services.AddRequirement<TRequirement>();
        return builder;
    }

    /// <summary>
    /// Adds a requirement using a factory.
    /// </summary>
    /// <typeparam name="TRequirement">The requirement type.</typeparam>
    /// <param name="builder">The pipeline builder.</param>
    /// <param name="factory">The requirement factory.</param>
    /// <returns>The same builder instance for chaining.</returns>
    public static PipelineBuilder AddRequirement<TRequirement>(
        this PipelineBuilder builder,
        Func<IServiceProvider, TRequirement> factory)
        where TRequirement : class, IPipelineRequirement
    {
        builder.Services.AddRequirement(factory);
        return builder;
    }

    /// <summary>
    /// Adds global hooks to run before or after all the modules have executed.
    /// </summary>
    /// <param name="builder">The pipeline builder.</param>
    /// <typeparam name="TGlobalSetup">The type of hook class.</typeparam>
    /// <returns>The same builder instance for chaining.</returns>
    public static PipelineBuilder AddPipelineGlobalHooks<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TGlobalSetup>(
        this PipelineBuilder builder)
        where TGlobalSetup : class, IPipelineGlobalHooks
    {
        builder.Services.AddPipelineGlobalHooks<TGlobalSetup>();
        return builder;
    }

    /// <summary>
    /// Adds a global receiver for module lifecycle events.
    /// </summary>
    /// <param name="builder">The pipeline builder.</param>
    /// <typeparam name="TReceiver">The receiver type.</typeparam>
    /// <returns>The same builder instance for chaining.</returns>
    public static PipelineBuilder AddModuleEventReceiver<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TReceiver>(
        this PipelineBuilder builder)
        where TReceiver : class, IModuleEventReceiver
    {
        builder.Services.AddModuleEventReceiver<TReceiver>();
        return builder;
    }

    /// <summary>
    /// Configures services for the pipeline.
    /// </summary>
    /// <param name="builder">The pipeline builder.</param>
    /// <param name="configureServices">Action to configure services.</param>
    /// <returns>The same builder instance for chaining.</returns>
    public static PipelineBuilder ConfigureServices(this PipelineBuilder builder, Action<IServiceCollection> configureServices)
    {
        configureServices(builder.Services);
        return builder;
    }

    /// <summary>
    /// Registers every leaf value beneath a configuration section as a secret during pipeline startup.
    /// </summary>
    /// <param name="builder">The pipeline builder.</param>
    /// <param name="sectionPath">The configuration section path, such as <c>Secrets</c>.</param>
    /// <returns>The same builder instance for chaining.</returns>
    public static PipelineBuilder MaskConfigurationSection(this PipelineBuilder builder, string sectionPath)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentException.ThrowIfNullOrWhiteSpace(sectionPath);

        builder.Services.Configure<SecretMaskingOptions>(options =>
        {
            options.MaskedConfigurationSections = options.MaskedConfigurationSections
                .Append(sectionPath)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToArray();
        });

        return builder;
    }

    /// <summary>
    /// Builds the pipeline and executes it.
    /// </summary>
    /// <param name="builder">The pipeline builder.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    /// <returns>A summary of the pipeline execution results.</returns>
    public static async Task<Models.PipelineSummary> ExecutePipelineAsync(this PipelineBuilder builder, CancellationToken cancellationToken = default)
    {
        await using var pipeline = await builder.BuildAsync().ConfigureAwait(false);
        return await pipeline.RunAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Builds the pipeline and exports its resolved dependency graph without executing modules.
    /// </summary>
    /// <param name="builder">The pipeline builder.</param>
    /// <param name="format">The output format.</param>
    /// <param name="path">The destination file.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    public static async Task ExportDependencyGraphAsync(
        this PipelineBuilder builder,
        DependencyGraphFormat format,
        string path,
        CancellationToken cancellationToken = default)
    {
        await using var pipeline = await builder.BuildAsync().ConfigureAwait(false);
        await pipeline.ExportDependencyGraphAsync(format, path, cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Adds a custom result repository for storing and retrieving module results.
    /// </summary>
    /// <param name="builder">The pipeline builder.</param>
    /// <typeparam name="TRepository">The type of result repository to add.</typeparam>
    /// <returns>The same builder instance for chaining.</returns>
    [RequiresUnreferencedCode(
        "Result history resolves module result types through reflection.")]
    [RequiresDynamicCode(
        "Result history creates generic delegates for runtime module result types.")]
    public static PipelineBuilder AddResultsRepository<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TRepository>(this PipelineBuilder builder)
        where TRepository : class, IModuleResultRepository
    {
        builder.Services.AddSingleton<IModuleResultRepository, TRepository>();
        return builder;
    }

    /// <summary>
    /// Enables fingerprint-based incremental module caching with the specified storage backend.
    /// Modules opt in through <see cref="Attributes.CacheInputsAttribute"/>,
    /// <see cref="Configuration.ModuleConfigurationBuilder.WithCacheKeyPart"/>, or
    /// <see cref="Configuration.ModuleConfigurationBuilder.WithCacheEnvironmentVariable"/>.
    /// </summary>
    /// <typeparam name="TStore">The cache storage backend.</typeparam>
    /// <param name="builder">The pipeline builder.</param>
    /// <param name="configure">Optional cache configuration.</param>
    /// <returns>The same builder instance for chaining.</returns>
    public static PipelineBuilder AddModuleCache<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TStore>(
        this PipelineBuilder builder,
        Action<ModuleCacheOptions>? configure = null)
        where TStore : class, IModuleCacheStore
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.Services.AddOptions<ModuleCacheOptions>();
        if (configure is not null)
        {
            builder.Services.Configure(configure);
        }

        builder.Services.TryAddSingleton<TStore>();
        builder.Services.Replace(ServiceDescriptor.Singleton<IModuleCacheStore>(
            serviceProvider => serviceProvider.GetRequiredService<TStore>()));
        builder.Services.TryAddSingleton<ModuleCacheFileHasher>();
        builder.Services.TryAddSingleton<ModuleCacheResultRepository>();
        builder.Services.TryAddSingleton<IModuleCacheResultRepository>(
            serviceProvider => serviceProvider.GetRequiredService<ModuleCacheResultRepository>());
        return builder;
    }

    /// <summary>
    /// Configures pipeline options.
    /// </summary>
    /// <param name="builder">The pipeline builder.</param>
    /// <param name="configureOptions">
    /// Function that returns the configured immutable options while preserving prior settings,
    /// typically with <see langword="with"/>.
    /// </param>
    /// <returns>The same builder instance for chaining.</returns>
    public static PipelineBuilder ConfigurePipelineOptions(
        this PipelineBuilder builder,
        Func<PipelineOptions, PipelineOptions> configureOptions)
    {
        ArgumentNullException.ThrowIfNull(configureOptions);
        builder.SetOptions(configureOptions(builder.Options));
        return builder;
    }

    /// <summary>
    /// Configures pipeline options with builder context.
    /// </summary>
    /// <param name="builder">The pipeline builder.</param>
    /// <param name="configureOptions">
    /// Function that receives the builder and returns the configured immutable options while preserving prior settings,
    /// typically with <see langword="with"/>.
    /// </param>
    /// <returns>The same builder instance for chaining.</returns>
    public static PipelineBuilder ConfigurePipelineOptions(
        this PipelineBuilder builder,
        Func<PipelineBuilder, PipelineOptions, PipelineOptions> configureOptions)
    {
        ArgumentNullException.ThrowIfNull(configureOptions);
        builder.SetOptions(configureOptions(builder, builder.Options));
        return builder;
    }

    /// <summary>
    /// Adds a custom module estimated time provider.
    /// </summary>
    /// <param name="builder">The pipeline builder.</param>
    /// <typeparam name="TProvider">The type of estimated time provider to add.</typeparam>
    /// <returns>The same builder instance for chaining.</returns>
    public static PipelineBuilder AddModuleEstimatedTimeProvider<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TProvider>(this PipelineBuilder builder)
        where TProvider : class, IModuleEstimatedTimeProvider
    {
        builder.Services.AddSingleton<IModuleEstimatedTimeProvider, TProvider>();
        return builder;
    }

    /// <summary>
    /// Adds a pipeline file writer for generating pipeline configuration files.
    /// </summary>
    /// <param name="builder">The pipeline builder.</param>
    /// <typeparam name="TWriter">The type of pipeline file writer to add.</typeparam>
    /// <returns>The same builder instance for chaining.</returns>
    public static PipelineBuilder AddPipelineFileWriter<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TWriter>(
        this PipelineBuilder builder)
        where TWriter : class, IBuildSystemPipelineFileWriter
    {
        builder.Services.AddSingleton<IBuildSystemPipelineFileWriter, TWriter>();
        return builder;
    }

    /// <summary>
    /// Adds a requirement instance to the pipeline.
    /// </summary>
    /// <param name="builder">The pipeline builder.</param>
    /// <param name="requirement">The requirement instance to add.</param>
    /// <returns>The same builder instance for chaining.</returns>
    public static PipelineBuilder AddRequirement(this PipelineBuilder builder, IPipelineRequirement requirement)
    {
        builder.Services.AddSingleton(requirement);
        return builder;
    }

    /// <summary>
    /// Adds a singleton service to the pipeline.
    /// </summary>
    /// <typeparam name="TService">The service type.</typeparam>
    /// <typeparam name="TImplementation">The implementation type.</typeparam>
    /// <param name="builder">The pipeline builder.</param>
    /// <returns>The same builder instance for chaining.</returns>
    public static PipelineBuilder AddSingleton<
        TService,
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TImplementation>(
        this PipelineBuilder builder)
        where TService : class
        where TImplementation : class, TService
    {
        builder.Services.AddSingleton<TService, TImplementation>();
        return builder;
    }

    /// <summary>
    /// Adds a singleton service instance to the pipeline.
    /// </summary>
    /// <typeparam name="TService">The service type.</typeparam>
    /// <param name="builder">The pipeline builder.</param>
    /// <param name="implementationInstance">The service instance.</param>
    /// <returns>The same builder instance for chaining.</returns>
    public static PipelineBuilder AddSingleton<TService>(
        this PipelineBuilder builder,
        TService implementationInstance)
        where TService : class
    {
        builder.Services.AddSingleton(implementationInstance);
        return builder;
    }

    /// <summary>
    /// Configures options for the pipeline.
    /// </summary>
    /// <typeparam name="TOptions">The options type.</typeparam>
    /// <param name="builder">The pipeline builder.</param>
    /// <param name="configureOptions">The configuration action.</param>
    /// <returns>The same builder instance for chaining.</returns>
    public static PipelineBuilder Configure<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] TOptions>(
        this PipelineBuilder builder,
        Action<TOptions> configureOptions)
        where TOptions : class
    {
        builder.Services.Configure(configureOptions);
        return builder;
    }

    private static void ValidateModuleType(Type moduleType)
    {
        ArgumentNullException.ThrowIfNull(moduleType);

        if (!typeof(IModule).IsAssignableFrom(moduleType)
            || !moduleType.IsClass
            || moduleType.IsAbstract
            || moduleType.IsGenericTypeDefinition)
        {
            throw new ArgumentException(
                $"Type '{moduleType.FullName}' must be a concrete, closed module type.",
                nameof(moduleType));
        }
    }
}
