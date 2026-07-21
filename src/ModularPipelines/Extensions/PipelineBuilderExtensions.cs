using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.DependencyInjection;
using ModularPipelines.Engine;
using ModularPipelines.Interfaces;
using ModularPipelines.Modules;
using ModularPipelines.Options;
using ModularPipelines.Requirements;
using IPipelineRequirementInstance = ModularPipelines.Requirements.IPipelineRequirement;

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
    /// <returns>The same builder instance for chaining.</returns>
    public static PipelineBuilder AddModule<TModule>(this PipelineBuilder builder)
        where TModule : class, IModule
    {
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
        builder.Services.AddModule(factory);
        return builder;
    }

    /// <summary>
    /// Adds multiple modules to the pipeline.
    /// </summary>
    public static PipelineBuilder AddModules<T1, T2>(this PipelineBuilder builder)
        where T1 : class, IModule
        where T2 : class, IModule
    {
        builder.Services.AddModule<T1>().AddModule<T2>();
        return builder;
    }

    /// <summary>
    /// Adds multiple modules to the pipeline.
    /// </summary>
    public static PipelineBuilder AddModules<T1, T2, T3>(this PipelineBuilder builder)
        where T1 : class, IModule
        where T2 : class, IModule
        where T3 : class, IModule
    {
        builder.Services.AddModule<T1>().AddModule<T2>().AddModule<T3>();
        return builder;
    }

    /// <summary>
    /// Adds multiple modules to the pipeline.
    /// </summary>
    public static PipelineBuilder AddModules<T1, T2, T3, T4>(this PipelineBuilder builder)
        where T1 : class, IModule
        where T2 : class, IModule
        where T3 : class, IModule
        where T4 : class, IModule
    {
        builder.Services.AddModule<T1>().AddModule<T2>().AddModule<T3>().AddModule<T4>();
        return builder;
    }

    /// <summary>
    /// Adds multiple modules to the pipeline.
    /// </summary>
    public static PipelineBuilder AddModules<T1, T2, T3, T4, T5>(this PipelineBuilder builder)
        where T1 : class, IModule
        where T2 : class, IModule
        where T3 : class, IModule
        where T4 : class, IModule
        where T5 : class, IModule
    {
        builder.Services.AddModule<T1>().AddModule<T2>().AddModule<T3>().AddModule<T4>().AddModule<T5>();
        return builder;
    }

    /// <summary>
    /// Adds multiple modules to the pipeline.
    /// </summary>
    public static PipelineBuilder AddModules<T1, T2, T3, T4, T5, T6>(this PipelineBuilder builder)
        where T1 : class, IModule
        where T2 : class, IModule
        where T3 : class, IModule
        where T4 : class, IModule
        where T5 : class, IModule
        where T6 : class, IModule
    {
        builder.Services.AddModule<T1>().AddModule<T2>().AddModule<T3>().AddModule<T4>().AddModule<T5>().AddModule<T6>();
        return builder;
    }

    /// <summary>
    /// Adds a requirement to the pipeline.
    /// </summary>
    /// <param name="builder">The pipeline builder.</param>
    /// <typeparam name="TRequirement">The type of requirement to add.</typeparam>
    /// <returns>The same builder instance for chaining.</returns>
    public static PipelineBuilder AddRequirement<TRequirement>(this PipelineBuilder builder)
        where TRequirement : class, IPipelineRequirement
    {
        builder.Services.AddRequirement<TRequirement>();
        return builder;
    }

    /// <summary>
    /// Adds global hooks to run before or after all the modules have executed.
    /// </summary>
    /// <param name="builder">The pipeline builder.</param>
    /// <typeparam name="TGlobalSetup">The type of hook class.</typeparam>
    /// <returns>The same builder instance for chaining.</returns>
    public static PipelineBuilder AddPipelineGlobalHooks<TGlobalSetup>(this PipelineBuilder builder)
        where TGlobalSetup : class, IPipelineGlobalHooks
    {
        builder.Services.AddPipelineGlobalHooks<TGlobalSetup>();
        return builder;
    }

    /// <summary>
    /// Adds module hooks to run before or after each module has executed.
    /// </summary>
    /// <param name="builder">The pipeline builder.</param>
    /// <typeparam name="TModuleHooks">The type of hook class.</typeparam>
    /// <returns>The same builder instance for chaining.</returns>
    public static PipelineBuilder AddPipelineModuleHooks<TModuleHooks>(this PipelineBuilder builder)
        where TModuleHooks : class, IPipelineModuleHooks
    {
        builder.Services.AddPipelineModuleHooks<TModuleHooks>();
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
    /// Configures services for the pipeline with builder context.
    /// </summary>
    /// <param name="builder">The pipeline builder.</param>
    /// <param name="configureServices">Action to configure services, receiving the builder as context.</param>
    /// <returns>The same builder instance for chaining.</returns>
    /// <remarks>This overload is provided for backward compatibility.</remarks>
    public static PipelineBuilder ConfigureServices(this PipelineBuilder builder, Action<PipelineBuilder, IServiceCollection> configureServices)
    {
        configureServices(builder, builder.Services);
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
        var pipeline = builder.Build();
        return await pipeline.RunAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Adds a custom result repository for storing and retrieving module results.
    /// </summary>
    /// <param name="builder">The pipeline builder.</param>
    /// <typeparam name="TRepository">The type of result repository to add.</typeparam>
    /// <returns>The same builder instance for chaining.</returns>
    public static PipelineBuilder AddResultsRepository<TRepository>(this PipelineBuilder builder)
        where TRepository : class, IModuleResultRepository
    {
        builder.Services.AddSingleton<IModuleResultRepository, TRepository>();
        return builder;
    }

    /// <summary>
    /// Configures pipeline options.
    /// </summary>
    /// <param name="builder">The pipeline builder.</param>
    /// <param name="configureOptions">Action to configure pipeline options.</param>
    /// <returns>The same builder instance for chaining.</returns>
    public static PipelineBuilder ConfigurePipelineOptions(this PipelineBuilder builder, Action<PipelineOptions> configureOptions)
    {
        configureOptions(builder.Options);
        return builder;
    }

    /// <summary>
    /// Configures pipeline options with builder context.
    /// </summary>
    /// <param name="builder">The pipeline builder.</param>
    /// <param name="configureOptions">Action to configure pipeline options, receiving the builder as context.</param>
    /// <returns>The same builder instance for chaining.</returns>
    public static PipelineBuilder ConfigurePipelineOptions(this PipelineBuilder builder, Action<PipelineBuilder, PipelineOptions> configureOptions)
    {
        configureOptions(builder, builder.Options);
        return builder;
    }

    /// <summary>
    /// Builds the pipeline asynchronously with validation.
    /// </summary>
    /// <param name="builder">The pipeline builder.</param>
    /// <returns>A validated pipeline ready for execution.</returns>
    /// <remarks>This is an alias for <see cref="PipelineBuilder.BuildAsync"/> for backward compatibility.</remarks>
    public static Task<IPipeline> BuildHostAsync(this PipelineBuilder builder)
    {
        return builder.BuildAsync();
    }

    /// <summary>
    /// Adds a custom module estimated time provider.
    /// </summary>
    /// <param name="builder">The pipeline builder.</param>
    /// <typeparam name="TProvider">The type of estimated time provider to add.</typeparam>
    /// <returns>The same builder instance for chaining.</returns>
    public static PipelineBuilder AddModuleEstimatedTimeProvider<TProvider>(this PipelineBuilder builder)
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
    public static PipelineBuilder AddPipelineFileWriter<TWriter>(this PipelineBuilder builder)
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
    public static PipelineBuilder AddRequirement(this PipelineBuilder builder, IPipelineRequirementInstance requirement)
    {
        builder.Services.AddSingleton(requirement);
        return builder;
    }
}
