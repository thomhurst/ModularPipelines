using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ModularPipelines.Caching;
using ModularPipelines.Engine;
using ModularPipelines.Enums;
using ModularPipelines.Events;
using ModularPipelines.Extensions;
using ModularPipelines.Interfaces;
using ModularPipelines.Modules;
using ModularPipelines.Options;
using ModularPipelines.Reporting;
using ModularPipelines.Requirements;
using ModularPipelines.Secrets;
using ModularPipelines.Validation;

namespace ModularPipelines;

/// <summary>
/// Convenience extension methods for PipelineBuilder that delegate to Services.
/// </summary>
/// <remarks>
/// <para>Registration methods follow two rules, stated on each method:</para>
/// <list type="bullet">
/// <item><b>Single-instance services</b> (one active implementation, such as an execution backend or a result
/// repository): the call <b>replaces</b> every earlier registration of the service, so the last call wins.</item>
/// <item><b>Multi-instance services</b> (all registrations are used, such as validators, enrichers, requirements,
/// and event handlers): a type registration is <b>added once</b> per implementation type, and an instance
/// registration is added once per instance, so repeated calls have no effect.</item>
/// </list>
/// </remarks>
public static class PipelineBuilderExtensions
{
    /// <summary>
    /// Adds a Module to the pipeline.
    /// </summary>
    /// <param name="builder">The pipeline builder.</param>
    /// <typeparam name="TModule">The type of Module to add.</typeparam>
    /// <returns>The same builder instance for chaining.</returns>
    public static PipelineBuilder AddModule<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TModule>(
        this PipelineBuilder builder)
        where TModule : class, IModule
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.Services.AddModule<TModule>();
        return builder;
    }

    /// <summary>
    /// Adds a pre-created Module instance to the pipeline.
    /// </summary>
    /// <param name="builder">The pipeline builder.</param>
    /// <param name="module">The module instance to add.</param>
    /// <typeparam name="TModule">The type of Module to add.</typeparam>
    /// <returns>The same builder instance for chaining.</returns>
    public static PipelineBuilder AddModule<TModule>(this PipelineBuilder builder, TModule module)
        where TModule : class, IModule
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(module);
        builder.Services.AddModule(module);
        return builder;
    }

    /// <summary>
    /// Adds a Module to the pipeline using a factory method.
    /// </summary>
    /// <param name="builder">The pipeline builder.</param>
    /// <param name="factory">A factory method for creating the module.</param>
    /// <typeparam name="TModule">The type of Module to add.</typeparam>
    /// <returns>The same builder instance for chaining.</returns>
    public static PipelineBuilder AddModule<TModule>(this PipelineBuilder builder, Func<IServiceProvider, TModule> factory)
        where TModule : class, IModule
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(factory);
        builder.Services.AddModule(factory);
        return builder;
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
        ArgumentNullException.ThrowIfNull(builder);
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
    /// <exception cref="InvalidOperationException">
    /// Thrown when the assembly contains a concrete <see cref="IModule"/> implementation that
    /// does not derive from <see cref="Module{T}"/> or <see cref="SyncModule{T}"/>.
    /// </exception>
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
    /// <exception cref="InvalidOperationException">
    /// Thrown when the assembly contains a concrete <see cref="IModule"/> implementation that
    /// does not derive from <see cref="Module{T}"/> or <see cref="SyncModule{T}"/>.
    /// </exception>
    [RequiresUnreferencedCode("Module discovery scans all types in an assembly.")]
    public static PipelineBuilder AddModulesFromAssembly(this PipelineBuilder builder, Assembly assembly)
    {
        ArgumentNullException.ThrowIfNull(builder);
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
    /// <remarks>Multi-instance service: adding the same requirement type again has no effect.</remarks>
    public static PipelineBuilder AddRequirement<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TRequirement>(
        this PipelineBuilder builder)
        where TRequirement : class, IPipelineRequirement
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IPipelineRequirement, TRequirement>());
        return builder;
    }

    /// <summary>
    /// Adds a requirement using a factory.
    /// </summary>
    /// <typeparam name="TRequirement">The requirement type.</typeparam>
    /// <param name="builder">The pipeline builder.</param>
    /// <param name="factory">The requirement factory.</param>
    /// <returns>The same builder instance for chaining.</returns>
    /// <remarks>
    /// Multi-instance service. Factories cannot be compared, so every call adds a requirement.
    /// </remarks>
    public static PipelineBuilder AddRequirement<TRequirement>(
        this PipelineBuilder builder,
        Func<IServiceProvider, TRequirement> factory)
        where TRequirement : class, IPipelineRequirement
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(factory);
        builder.Services.AddRequirement(factory);
        return builder;
    }

    /// <summary>
    /// Adds a handler for pipeline lifecycle events.
    /// </summary>
    /// <param name="builder">The pipeline builder.</param>
    /// <typeparam name="THandler">The handler type.</typeparam>
    /// <returns>The same builder instance for chaining.</returns>
    /// <remarks>Multi-instance service: adding the same handler type again has no effect.</remarks>
    public static PipelineBuilder AddPipelineEventHandler<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] THandler>(
        this PipelineBuilder builder)
        where THandler : class, IPipelineEventHandler
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.Services.AddPipelineEventHandler<THandler>();
        return builder;
    }

    /// <summary>
    /// Adds a global handler for module lifecycle events.
    /// </summary>
    /// <param name="builder">The pipeline builder.</param>
    /// <typeparam name="THandler">The handler type.</typeparam>
    /// <returns>The same builder instance for chaining.</returns>
    /// <remarks>Multi-instance service: adding the same handler type again has no effect.</remarks>
    public static PipelineBuilder AddModuleEventHandler<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] THandler>(
        this PipelineBuilder builder)
        where THandler : class, IModuleEventHandler
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.Services.AddModuleEventHandler<THandler>();
        return builder;
    }

    /// <summary>
    /// Replaces the built-in execution backend.
    /// </summary>
    /// <typeparam name="TBackend">The execution backend implementation.</typeparam>
    /// <param name="builder">The pipeline builder.</param>
    /// <returns>The same builder instance for chaining.</returns>
    /// <remarks>Single-instance service: replaces any earlier execution backend registration.</remarks>
    public static PipelineBuilder AddExecutionBackend<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TBackend>(
        this PipelineBuilder builder)
        where TBackend : class, IExecutionBackend
    {
        ArgumentNullException.ThrowIfNull(builder);
        ReplaceSingleton<IExecutionBackend, TBackend>(builder.Services);
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

        builder.ConfigureOptions(options => options with
        {
            Secrets = options.Secrets with
            {
                MaskedConfigurationSections = options.Secrets.MaskedConfigurationSections
                    .Append(sectionPath)
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToArray(),
            },
        });

        return builder;
    }

    /// <summary>
    /// Builds and runs the pipeline.
    /// </summary>
    /// <param name="builder">The pipeline builder.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    /// <returns>A summary of the pipeline run results.</returns>
    public static async Task<Models.PipelineSummary> RunAsync(this PipelineBuilder builder, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(builder);
        var pipeline = await builder.BuildAsync().ConfigureAwait(false);
        await using var pipelineLifetime = pipeline.ConfigureAwait(false);
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
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentException.ThrowIfNullOrWhiteSpace(path);
        var pipeline = await builder.BuildForDependencyGraphExportAsync().ConfigureAwait(false);
        await using var pipelineLifetime = pipeline.ConfigureAwait(false);
        await pipeline.ExportDependencyGraphAsync(format, path, cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Adds a custom result repository for storing and retrieving module results.
    /// </summary>
    /// <param name="builder">The pipeline builder.</param>
    /// <typeparam name="TRepository">The type of result repository to add.</typeparam>
    /// <returns>The same builder instance for chaining.</returns>
    /// <remarks>Single-instance service: replaces the default repository and any earlier registration.</remarks>
    [RequiresUnreferencedCode(
        "Result history resolves module result types through reflection.")]
    [RequiresDynamicCode(
        "Result history creates generic delegates for runtime module result types.")]
    public static PipelineBuilder AddResultsRepository<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TRepository>(this PipelineBuilder builder)
        where TRepository : class, IModuleResultRepository
    {
        ArgumentNullException.ThrowIfNull(builder);
        ReplaceSingleton<IModuleResultRepository, TRepository>(builder.Services);
        return builder;
    }

    /// <summary>
    /// Enables fingerprint-based incremental module caching with the specified storage backend.
    /// Modules opt in through <see cref="Attributes.CacheInputsAttribute"/>,
    /// <see cref="ModuleConfigurationBuilder.WithCacheKeyPart"/>, or
    /// <see cref="ModuleConfigurationBuilder.WithCacheEnvironmentVariable"/>.
    /// </summary>
    /// <typeparam name="TStore">The cache storage backend.</typeparam>
    /// <param name="builder">The pipeline builder.</param>
    /// <param name="configure">
    /// Optional cache configuration that returns updated options, for example
    /// <c>options =&gt; options with { MaxInputFiles = 10_000 }</c>. Configurations from repeated calls apply
    /// in call order.
    /// </param>
    /// <returns>The same builder instance for chaining.</returns>
    /// <remarks>Single-instance service: the store replaces any earlier module cache store.</remarks>
    public static PipelineBuilder AddModuleCache<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TStore>(
        this PipelineBuilder builder,
        Func<ModuleCacheOptions, ModuleCacheOptions>? configure = null)
        where TStore : class, IModuleCacheStore
    {
        ArgumentNullException.ThrowIfNull(builder);

        if (configure is not null)
        {
            ModuleCacheOptionsConfiguration.Register(builder.Services, configure);
        }

        builder.Services.TryAddSingleton<TStore>();
        builder.Services.RemoveAll<IModuleCacheStore>();
        builder.Services.AddSingleton<IModuleCacheStore>(
            serviceProvider => serviceProvider.GetRequiredService<TStore>());
        builder.Services.TryAddSingleton<ModuleCacheFileHasher>();
        builder.Services.TryAddSingleton<ModuleCacheResultRepository>();
        builder.Services.TryAddSingleton<IModuleCacheResultRepository>(
            serviceProvider => serviceProvider.GetRequiredService<ModuleCacheResultRepository>());
        return builder;
    }

    /// <summary>
    /// Writes a schema-versioned JSON report when the pipeline finishes.
    /// </summary>
    /// <param name="builder">The pipeline builder.</param>
    /// <param name="path">The report path.</param>
    /// <returns>The same builder instance for chaining.</returns>
    public static PipelineBuilder WriteRunReport(this PipelineBuilder builder, string path)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentException.ThrowIfNullOrWhiteSpace(path);
        return builder.ConfigureOptions(options => options with
        {
            RunReport = options.RunReport with { ReportPath = path },
        });
    }

    /// <summary>
    /// Replaces the default file-system run history store.
    /// </summary>
    /// <typeparam name="TStore">The history store implementation type.</typeparam>
    /// <param name="builder">The pipeline builder.</param>
    /// <returns>The same builder instance for chaining.</returns>
    /// <remarks>Single-instance service: replaces the default store and any earlier registration.</remarks>
    public static PipelineBuilder AddRunHistoryStore<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TStore>(
        this PipelineBuilder builder)
        where TStore : class, IRunHistoryStore
    {
        ArgumentNullException.ThrowIfNull(builder);
        ReplaceSingleton<IRunHistoryStore, TStore>(builder.Services);
        return builder;
    }

    /// <summary>
    /// Adds a run report correlation metadata enricher.
    /// </summary>
    /// <typeparam name="TEnricher">The enricher implementation type.</typeparam>
    /// <param name="builder">The pipeline builder.</param>
    /// <returns>The same builder instance for chaining.</returns>
    /// <remarks>Multi-instance service: adding the same enricher type again has no effect.</remarks>
    public static PipelineBuilder AddRunReportEnricher<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TEnricher>(
        this PipelineBuilder builder)
        where TEnricher : class, IRunReportEnricher
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.Services.TryAddEnumerable(
            ServiceDescriptor.Singleton<IRunReportEnricher, TEnricher>());
        return builder;
    }

    /// <summary>
    /// Adds a custom pipeline validator.
    /// </summary>
    /// <typeparam name="TValidator">The validator implementation type.</typeparam>
    /// <param name="builder">The pipeline builder.</param>
    /// <returns>The same builder instance for chaining.</returns>
    /// <remarks>Multi-instance service: adding the same validator type again has no effect.</remarks>
    public static PipelineBuilder AddValidator<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TValidator>(
        this PipelineBuilder builder)
        where TValidator : class, IPipelineValidator
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.Services.TryAddEnumerable(
            ServiceDescriptor.Singleton<IPipelineValidator, TValidator>());
        return builder;
    }

    /// <summary>
    /// Adds a custom module estimated time provider.
    /// </summary>
    /// <param name="builder">The pipeline builder.</param>
    /// <typeparam name="TProvider">The type of estimated time provider to add.</typeparam>
    /// <returns>The same builder instance for chaining.</returns>
    /// <remarks>Single-instance service: replaces the default provider and any earlier registration.</remarks>
    public static PipelineBuilder AddModuleEstimatedTimeProvider<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TProvider>(this PipelineBuilder builder)
        where TProvider : class, IModuleEstimatedTimeProvider
    {
        ArgumentNullException.ThrowIfNull(builder);
        ReplaceSingleton<IModuleEstimatedTimeProvider, TProvider>(builder.Services);
        return builder;
    }

    /// <summary>
    /// Adds a requirement instance to the pipeline.
    /// </summary>
    /// <param name="builder">The pipeline builder.</param>
    /// <param name="requirement">The requirement instance to add.</param>
    /// <returns>The same builder instance for chaining.</returns>
    /// <remarks>Multi-instance service: adding the same instance again has no effect.</remarks>
    public static PipelineBuilder AddRequirement(this PipelineBuilder builder, IPipelineRequirement requirement)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(requirement);
        AddInstanceOnce(builder.Services, requirement);
        return builder;
    }

    /// <summary>
    /// Adds a command interceptor that wraps every command the pipeline executes.
    /// </summary>
    /// <typeparam name="TInterceptor">The interceptor implementation type.</typeparam>
    /// <param name="builder">The pipeline builder.</param>
    /// <returns>The same builder instance for chaining.</returns>
    /// <remarks>
    /// Interceptors run in registration order: the first registered interceptor is the outermost.
    /// Multi-instance service: adding the same interceptor type again has no effect.
    /// </remarks>
    public static PipelineBuilder AddCommandInterceptor<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TInterceptor>(
        this PipelineBuilder builder)
        where TInterceptor : class, ICommandInterceptor
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.Services.TryAddEnumerable(
            ServiceDescriptor.Singleton<ICommandInterceptor, TInterceptor>());
        return builder;
    }

    /// <summary>
    /// Adds a command interceptor instance that wraps every command the pipeline executes.
    /// </summary>
    /// <param name="builder">The pipeline builder.</param>
    /// <param name="interceptor">The interceptor instance.</param>
    /// <returns>The same builder instance for chaining.</returns>
    /// <remarks>
    /// Interceptors run in registration order: the first registered interceptor is the outermost.
    /// Multi-instance service: adding the same instance again has no effect, while distinct instances of
    /// one type each run.
    /// </remarks>
    public static PipelineBuilder AddCommandInterceptor(
        this PipelineBuilder builder,
        ICommandInterceptor interceptor)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(interceptor);
        AddInstanceOnce(builder.Services, interceptor);
        return builder;
    }

    private static void ReplaceSingleton<
        TService,
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TImplementation>(
        IServiceCollection services)
        where TService : class
        where TImplementation : class, TService
    {
        services.RemoveAll<TService>();
        services.AddSingleton<TService, TImplementation>();
    }

    private static void AddInstanceOnce<TService>(IServiceCollection services, TService instance)
        where TService : class
    {
        if (!services.Any(descriptor => descriptor.ServiceType == typeof(TService)
                                        && !descriptor.IsKeyedService
                                        && ReferenceEquals(descriptor.ImplementationInstance, instance)))
        {
            services.AddSingleton(instance);
        }
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

        ModuleExecutionContract.Validate(moduleType);
    }
}
