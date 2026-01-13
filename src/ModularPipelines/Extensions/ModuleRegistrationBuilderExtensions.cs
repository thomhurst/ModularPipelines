using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.DependencyInjection;
using ModularPipelines.Interfaces;
using ModularPipelines.Modules;
using ModularPipelines.Requirements;

namespace ModularPipelines.Extensions;

/// <summary>
/// Extensions for chaining module registrations from a builder.
/// </summary>
public static class ModuleRegistrationBuilderExtensions
{
    /// <summary>
    /// Adds a Module to the pipeline.
    /// </summary>
    /// <param name="builder">The registration builder.</param>
    /// <typeparam name="TModule">The type of Module to add.</typeparam>
    /// <returns>A builder for configuring the module registration.</returns>
    public static IModuleRegistrationBuilder AddModule<TModule>(this IModuleRegistrationBuilder builder)
        where TModule : class, IModule
    {
        return builder.Services.AddModule<TModule>();
    }

    /// <summary>
    /// Adds a Module to the pipeline.
    /// </summary>
    /// <param name="builder">The registration builder.</param>
    /// <param name="tModule">The module to add.</param>
    /// <typeparam name="TModule">The type of Module to add.</typeparam>
    /// <returns>A builder for configuring the module registration.</returns>
    public static IModuleRegistrationBuilder AddModule<TModule>(this IModuleRegistrationBuilder builder, TModule tModule)
        where TModule : class, IModule
    {
        return builder.Services.AddModule(tModule);
    }

    /// <summary>
    /// Adds a Module to the pipeline.
    /// </summary>
    /// <param name="builder">The registration builder.</param>
    /// <typeparam name="TModule">The type of Module to add.</typeparam>
    /// <param name="tModuleFactory">A factory method for creating the module.</param>
    /// <returns>A builder for configuring the module registration.</returns>
    public static IModuleRegistrationBuilder AddModule<TModule>(this IModuleRegistrationBuilder builder, Func<IServiceProvider, TModule> tModuleFactory)
        where TModule : class, IModule
    {
        return builder.Services.AddModule(tModuleFactory);
    }

    /// <summary>
    /// Adds global hooks to run before or after all the modules have executed.
    /// </summary>
    /// <param name="builder">The registration builder.</param>
    /// <typeparam name="TGlobalSetup">The type of hook class.</typeparam>
    /// <returns>The pipeline's service collection.</returns>
    public static IServiceCollection AddPipelineGlobalHooks<TGlobalSetup>(this IModuleRegistrationBuilder builder)
        where TGlobalSetup : class, IPipelineGlobalHooks
    {
        return builder.Services.AddPipelineGlobalHooks<TGlobalSetup>();
    }

    /// <summary>
    /// Adds module hooks to run before or after each module has executed.
    /// </summary>
    /// <param name="builder">The registration builder.</param>
    /// <typeparam name="TModuleHooks">The type of hook class.</typeparam>
    /// <returns>The pipeline's service collection.</returns>
    public static IServiceCollection AddPipelineModuleHooks<TModuleHooks>(this IModuleRegistrationBuilder builder)
        where TModuleHooks : class, IPipelineModuleHooks
    {
        return builder.Services.AddPipelineModuleHooks<TModuleHooks>();
    }

    /// <summary>
    /// Adds a requirement to the pipeline.
    /// </summary>
    /// <param name="builder">The registration builder.</param>
    /// <typeparam name="TRequirement">The type of requirement to add.</typeparam>
    /// <returns>The pipeline's service collection.</returns>
    public static IServiceCollection AddRequirement<TRequirement>(this IModuleRegistrationBuilder builder)
        where TRequirement : class, IPipelineRequirement
    {
        return builder.Services.AddRequirement<TRequirement>();
    }

    /// <summary>
    /// Adds a singleton service to the pipeline's service collection.
    /// </summary>
    /// <typeparam name="TService">The type of the service to add.</typeparam>
    /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
    /// <param name="builder">The registration builder.</param>
    /// <returns>The pipeline's service collection.</returns>
    public static IServiceCollection AddSingleton<TService, TImplementation>(this IModuleRegistrationBuilder builder)
        where TService : class
        where TImplementation : class, TService
    {
        return builder.Services.AddSingleton<TService, TImplementation>();
    }

    /// <summary>
    /// Adds a singleton service to the pipeline's service collection.
    /// </summary>
    /// <typeparam name="TService">The type of the service to add.</typeparam>
    /// <param name="builder">The registration builder.</param>
    /// <param name="implementationInstance">The instance of the service.</param>
    /// <returns>The pipeline's service collection.</returns>
    public static IServiceCollection AddSingleton<TService>(this IModuleRegistrationBuilder builder, TService implementationInstance)
        where TService : class
    {
        return builder.Services.AddSingleton(implementationInstance);
    }

    /// <summary>
    /// Configures options using the specified configure action.
    /// </summary>
    /// <typeparam name="TOptions">The options type to be configured.</typeparam>
    /// <param name="builder">The registration builder.</param>
    /// <param name="configureOptions">The action used to configure the options.</param>
    /// <returns>The pipeline's service collection.</returns>
    public static IServiceCollection Configure<TOptions>(this IModuleRegistrationBuilder builder, Action<TOptions> configureOptions)
        where TOptions : class
    {
        return builder.Services.Configure(configureOptions);
    }
}
