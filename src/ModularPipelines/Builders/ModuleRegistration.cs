using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ModularPipelines.Caching;
using ModularPipelines.Engine;
using ModularPipelines.Extensions;
using ModularPipelines.Interfaces;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Options;
using ModularPipelines.Requirements;
using ModularPipelines.Validation;

namespace ModularPipelines;

/// <summary>
/// Represents a module registration and provides fluent access to the pipeline builder.
/// </summary>
/// <typeparam name="TModule">The registered module type.</typeparam>
public sealed class ModuleRegistration<TModule>
    where TModule : class, IModule
{
    internal ModuleRegistration(PipelineBuilder builder)
    {
        Builder = builder;
    }

    /// <summary>
    /// Gets the pipeline builder associated with this registration.
    /// </summary>
    public PipelineBuilder Builder { get; }

    /// <summary>
    /// Gets the service collection for registering application services.
    /// </summary>
    public IServiceCollection Services => Builder.Services;

    /// <summary>
    /// Gets the configuration manager for adding and reading configuration.
    /// </summary>
    public ConfigurationManager Configuration => Builder.Configuration;

    /// <summary>
    /// Gets the current immutable pipeline options snapshot.
    /// </summary>
    public PipelineOptions Options => Builder.Options;

    /// <summary>
    /// Gets the host environment information.
    /// </summary>
    public IHostEnvironment Environment => Builder.Environment;

    /// <summary>
    /// Adds another module to the pipeline.
    /// </summary>
    /// <typeparam name="TNextModule">The type of module to add.</typeparam>
    /// <returns>The new module registration.</returns>
#pragma warning disable MPG0013 // The concrete type is supplied by the caller of this forwarding method.
    public ModuleRegistration<TNextModule> AddModule<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TNextModule>()
        where TNextModule : class, IModule
        => Builder.AddModule<TNextModule>();
#pragma warning restore MPG0013

    /// <summary>
    /// Adds another pre-created module instance to the pipeline.
    /// </summary>
    /// <param name="module">The module instance to add.</param>
    /// <typeparam name="TNextModule">The type of module to add.</typeparam>
    /// <returns>The new module registration.</returns>
#pragma warning disable MPG0013 // The concrete type is supplied by the caller of this forwarding method.
    public ModuleRegistration<TNextModule> AddModule<TNextModule>(TNextModule module)
        where TNextModule : class, IModule
        => Builder.AddModule(module);
#pragma warning restore MPG0013

    /// <summary>
    /// Adds another module to the pipeline using a factory method.
    /// </summary>
    /// <param name="factory">A factory method for creating the module.</param>
    /// <typeparam name="TNextModule">The type of module to add.</typeparam>
    /// <returns>The new module registration.</returns>
#pragma warning disable MPG0013 // The concrete type is supplied by the caller of this forwarding method.
    public ModuleRegistration<TNextModule> AddModule<TNextModule>(Func<IServiceProvider, TNextModule> factory)
        where TNextModule : class, IModule
        => Builder.AddModule(factory);
#pragma warning restore MPG0013

    /// <summary>
    /// Adds multiple module types to the pipeline.
    /// </summary>
    /// <param name="moduleTypes">The module types to add.</param>
    /// <returns>The pipeline builder.</returns>
    [RequiresUnreferencedCode(
        "Runtime type module registration relies on reflection. Use AddModule<TModule>() for trim-safe registration.")]
    [RequiresDynamicCode(
        "Runtime type module registration may require runtime code generation. Use AddModule<TModule>() for Native AOT.")]
    public PipelineBuilder AddModules(params Type[] moduleTypes) => Builder.AddModules(moduleTypes);

    /// <summary>
    /// Adds all modules from the assembly containing the specified type.
    /// </summary>
    /// <typeparam name="T">Any type from the assembly to scan.</typeparam>
    /// <returns>The pipeline builder.</returns>
    [RequiresUnreferencedCode("Module discovery scans all types in an assembly.")]
    public PipelineBuilder AddModulesFromAssemblyContainingType<T>()
        => Builder.AddModulesFromAssemblyContainingType<T>();

    /// <summary>
    /// Adds all modules from an assembly.
    /// </summary>
    /// <param name="assembly">The assembly to scan.</param>
    /// <returns>The pipeline builder.</returns>
    [RequiresUnreferencedCode("Module discovery scans all types in an assembly.")]
    public PipelineBuilder AddModulesFromAssembly(Assembly assembly)
        => Builder.AddModulesFromAssembly(assembly);

    /// <summary>
    /// Adds a requirement to the pipeline.
    /// </summary>
    /// <typeparam name="TRequirement">The type of requirement to add.</typeparam>
    /// <returns>The pipeline builder.</returns>
    public PipelineBuilder AddRequirement<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TRequirement>()
        where TRequirement : class, IPipelineRequirement
        => Builder.AddRequirement<TRequirement>();

    /// <summary>
    /// Adds a requirement to the pipeline using a factory method.
    /// </summary>
    /// <param name="factory">A factory method for creating the requirement.</param>
    /// <typeparam name="TRequirement">The type of requirement to add.</typeparam>
    /// <returns>The pipeline builder.</returns>
    public PipelineBuilder AddRequirement<TRequirement>(Func<IServiceProvider, TRequirement> factory)
        where TRequirement : class, IPipelineRequirement
        => Builder.AddRequirement(factory);

    /// <summary>
    /// Adds a requirement instance to the pipeline.
    /// </summary>
    /// <param name="requirement">The requirement instance to add.</param>
    /// <returns>The pipeline builder.</returns>
    public PipelineBuilder AddRequirement(IPipelineRequirement requirement)
        => Builder.AddRequirement(requirement);

    /// <summary>
    /// Adds global hooks to run before or after all modules execute.
    /// </summary>
    /// <typeparam name="TGlobalSetup">The hook type.</typeparam>
    /// <returns>The pipeline builder.</returns>
    public PipelineBuilder AddPipelineGlobalHooks<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TGlobalSetup>()
        where TGlobalSetup : class, IPipelineGlobalHooks
        => Builder.AddPipelineGlobalHooks<TGlobalSetup>();

    /// <summary>
    /// Adds a global receiver for module lifecycle events.
    /// </summary>
    /// <typeparam name="TReceiver">The receiver type.</typeparam>
    /// <returns>The pipeline builder.</returns>
    public PipelineBuilder AddModuleEventReceiver<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TReceiver>()
        where TReceiver : class, IModuleEventReceiver
        => Builder.AddModuleEventReceiver<TReceiver>();

    /// <summary>
    /// Configures services for the pipeline.
    /// </summary>
    /// <param name="configureServices">The service configuration action.</param>
    /// <returns>The pipeline builder.</returns>
    public PipelineBuilder ConfigureServices(Action<IServiceCollection> configureServices)
        => Builder.ConfigureServices(configureServices);

    /// <summary>
    /// Adds a custom result repository.
    /// </summary>
    /// <typeparam name="TRepository">The repository type.</typeparam>
    /// <returns>The pipeline builder.</returns>
    [RequiresUnreferencedCode("Result history resolves module result types through reflection.")]
    [RequiresDynamicCode("Result history creates generic delegates for runtime module result types.")]
    public PipelineBuilder AddResultsRepository<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TRepository>()
        where TRepository : class, IModuleResultRepository
        => Builder.AddResultsRepository<TRepository>();

    /// <summary>
    /// Enables fingerprint-based incremental module caching.
    /// </summary>
    /// <param name="configure">Optional cache configuration.</param>
    /// <typeparam name="TStore">The cache storage backend.</typeparam>
    /// <returns>The pipeline builder.</returns>
    public PipelineBuilder AddModuleCache<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TStore>(
        Action<ModuleCacheOptions>? configure = null)
        where TStore : class, IModuleCacheStore
        => Builder.AddModuleCache<TStore>(configure);

    /// <summary>
    /// Configures pipeline options.
    /// </summary>
    /// <param name="configureOptions">The options transformation.</param>
    /// <returns>The pipeline builder.</returns>
    public PipelineBuilder ConfigurePipelineOptions(Func<PipelineOptions, PipelineOptions> configureOptions)
        => Builder.ConfigurePipelineOptions(configureOptions);

    /// <summary>
    /// Configures pipeline options with builder context.
    /// </summary>
    /// <param name="configureOptions">The options transformation.</param>
    /// <returns>The pipeline builder.</returns>
    public PipelineBuilder ConfigurePipelineOptions(
        Func<PipelineBuilder, PipelineOptions, PipelineOptions> configureOptions)
        => Builder.ConfigurePipelineOptions(configureOptions);

    /// <summary>
    /// Adds a custom module estimated time provider.
    /// </summary>
    /// <typeparam name="TProvider">The provider type.</typeparam>
    /// <returns>The pipeline builder.</returns>
    public PipelineBuilder AddModuleEstimatedTimeProvider<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TProvider>()
        where TProvider : class, IModuleEstimatedTimeProvider
        => Builder.AddModuleEstimatedTimeProvider<TProvider>();

    /// <summary>
    /// Adds a pipeline file writer.
    /// </summary>
    /// <typeparam name="TWriter">The writer type.</typeparam>
    /// <returns>The pipeline builder.</returns>
    public PipelineBuilder AddPipelineFileWriter<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TWriter>()
        where TWriter : class, IBuildSystemPipelineFileWriter
        => Builder.AddPipelineFileWriter<TWriter>();

    /// <summary>
    /// Adds a singleton service implementation.
    /// </summary>
    /// <typeparam name="TService">The service type.</typeparam>
    /// <typeparam name="TImplementation">The implementation type.</typeparam>
    /// <returns>The pipeline builder.</returns>
    public PipelineBuilder AddSingleton<
        TService,
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TImplementation>()
        where TService : class
        where TImplementation : class, TService
        => Builder.AddSingleton<TService, TImplementation>();

    /// <summary>
    /// Adds a singleton service instance.
    /// </summary>
    /// <param name="implementationInstance">The service instance.</param>
    /// <typeparam name="TService">The service type.</typeparam>
    /// <returns>The pipeline builder.</returns>
    public PipelineBuilder AddSingleton<TService>(TService implementationInstance)
        where TService : class
        => Builder.AddSingleton(implementationInstance);

    /// <summary>
    /// Configures options for the pipeline.
    /// </summary>
    /// <param name="configureOptions">The configuration action.</param>
    /// <typeparam name="TOptions">The options type.</typeparam>
    /// <returns>The pipeline builder.</returns>
    public PipelineBuilder Configure<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] TOptions>(
        Action<TOptions> configureOptions)
        where TOptions : class
        => Builder.Configure(configureOptions);

    /// <summary>
    /// Builds and executes the pipeline.
    /// </summary>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    /// <returns>A summary of the pipeline execution results.</returns>
    public Task<PipelineSummary> ExecutePipelineAsync(CancellationToken cancellationToken = default)
        => Builder.ExecutePipelineAsync(cancellationToken);

    /// <summary>
    /// Sets the minimum pipeline log level.
    /// </summary>
    /// <param name="logLevel">The minimum log level.</param>
    /// <returns>The pipeline builder.</returns>
    public PipelineBuilder SetLogLevel(LogLevel logLevel) => Builder.SetLogLevel(logLevel);

    /// <summary>
    /// Runs only modules in the specified categories.
    /// </summary>
    /// <param name="categories">The categories to run.</param>
    /// <returns>The pipeline builder.</returns>
    public PipelineBuilder RunCategories(params string[] categories) => Builder.RunCategories(categories);

    /// <summary>
    /// Ignores modules in the specified categories.
    /// </summary>
    /// <param name="categories">The categories to ignore.</param>
    /// <returns>The pipeline builder.</returns>
    public PipelineBuilder IgnoreCategories(params string[] categories) => Builder.IgnoreCategories(categories);

    /// <summary>
    /// Builds and validates the pipeline.
    /// </summary>
    /// <returns>The pipeline.</returns>
    public Task<IPipeline> BuildAsync() => Builder.BuildAsync();

    /// <summary>
    /// Validates the pipeline configuration.
    /// </summary>
    /// <returns>The validation result.</returns>
    public Task<ValidationResult> ValidateAsync() => Builder.ValidateAsync();

    /// <summary>
    /// Converts a module registration back to its pipeline builder.
    /// </summary>
    /// <param name="registration">The module registration.</param>
    public static implicit operator PipelineBuilder(ModuleRegistration<TModule> registration)
    {
        ArgumentNullException.ThrowIfNull(registration);
        return registration.Builder;
    }
}
