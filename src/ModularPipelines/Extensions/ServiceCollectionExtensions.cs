using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.DependencyInjection;
using ModularPipelines.Interfaces;
using ModularPipelines.Modules;
using ModularPipelines.Requirements;

namespace ModularPipelines.Extensions;

/// <summary>
/// Extensions for adding pipeline services to the service provider.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds a Module to the pipeline.
    /// </summary>
    /// <param name="services">The pipeline's service collection.</param>
    /// <typeparam name="TModule">The type of Module to add.</typeparam>
    /// <returns>The pipeline's same service collection.</returns>
    public static IServiceCollection AddModule<TModule>(this IServiceCollection services)
        where TModule : ModuleBase
    {
        return services.AddSingleton<ModuleBase, TModule>();
    }

    /// <summary>
    /// Adds a Module to the pipeline.
    /// </summary>
    /// <param name="services">The pipeline's service collection.</param>
    /// <param name="tModule">The module to add.</param>
    /// <typeparam name="TModule">The type of Module to add.</typeparam>
    /// <returns>The pipeline's same service collection.</returns>
    public static IServiceCollection AddModule<TModule>(this IServiceCollection services, TModule tModule)
        where TModule : ModuleBase
    {
        return services.AddSingleton<ModuleBase>(tModule);
    }

    /// <summary>
    /// Adds a Module to the pipeline.
    /// </summary>
    /// <param name="services">The pipeline's service collection.</param>
    /// <typeparam name="TModule">The type of Module to add.</typeparam>
    /// /// <param name="tModuleFactory">A factory method for creating the module.</param>
    /// <returns>The pipeline's same service collection.</returns>
    public static IServiceCollection AddModule<TModule>(this IServiceCollection services, Func<IServiceProvider, TModule> tModuleFactory)
        where TModule : ModuleBase
    {
        return services.AddSingleton<ModuleBase>(tModuleFactory);
    }

    /// <summary>
    /// Adds a requirement to the pipeline.
    /// </summary>
    /// <param name="services">The pipeline's service collection.</param>
    /// <typeparam name="TRequirement">The type of requirement to add.</typeparam>
    /// <returns>The pipeline's same service collection.</returns>
    public static IServiceCollection AddRequirement<TRequirement>(this IServiceCollection services)
        where TRequirement : class, IPipelineRequirement
    {
        return services.AddScoped<IPipelineRequirement, TRequirement>();
    }

    /// <summary>
    /// Adds a requirement to the pipeline.
    /// </summary>
    /// <param name="services">The pipeline's service collection.</param>
    /// <param name="tRequirement">The requirement to add.</param>
    /// <typeparam name="TRequirement">The type of requirement to add.</typeparam>
    /// <returns>The pipeline's same service collection.</returns>
    public static IServiceCollection AddRequirement<TRequirement>(this IServiceCollection services, TRequirement tRequirement)
        where TRequirement : class, IPipelineRequirement
    {
        return services.AddSingleton<IPipelineRequirement>(tRequirement);
    }

    /// <summary>
    /// Adds a requirement to the pipeline.
    /// </summary>
    /// <param name="services">The pipeline's service collection.</param>
    /// <param name="tRequirementFactory">A factory method for creating the requirement.</param>
    /// <typeparam name="TRequirement">The type of requirement to add.</typeparam>
    /// <returns>The pipeline's same service collection.</returns>
    public static IServiceCollection AddRequirement<TRequirement>(this IServiceCollection services, Func<IServiceProvider, TRequirement> tRequirementFactory)
        where TRequirement : class, IPipelineRequirement
    {
        return services.AddSingleton<IPipelineRequirement>(tRequirementFactory);
    }

    /// <summary>
    /// Adds all modules from an assembly to the pipeline.
    /// </summary>
    /// <param name="services">The pipeline's service collection.</param>
    /// <typeparam name="T">Any type from the assembly to add modules from.</typeparam>
    /// <returns>The pipeline's same service collection.</returns>
    public static IServiceCollection AddModulesFromAssemblyContainingType<T>(this IServiceCollection services)
    {
        return AddModulesFromAssembly(services, typeof(T).Assembly);
    }

    /// <summary>
    /// Adds all modules from an assembly to the pipeline.
    /// </summary>
    /// <param name="services">The pipeline's service collection.</param>
    /// <param name="assembly">The assembly to add modules from.</param>
    /// <returns>The pipeline's same service collection.</returns>
    public static IServiceCollection AddModulesFromAssembly(this IServiceCollection services, Assembly assembly)
    {
        var modules = assembly.GetTypes()
            .Where(type => type.IsAssignableTo(typeof(ModuleBase)))
            .Where(type => type.IsClass)
            .Where(type => !type.IsAbstract);

        foreach (var module in modules)
        {
            services.AddSingleton(typeof(ModuleBase), module);
        }

        return services;
    }

    /// <summary>
    /// Adds global hooks to run before or after all the modules have executed.
    /// </summary>
    /// <param name="services">The pipeline's service collection.</param>
    /// <typeparam name="TGlobalSetup">The type of hook class.</typeparam>
    /// <returns>The pipeline's same service collection.</returns>
    public static IServiceCollection AddPipelineGlobalHooks<TGlobalSetup>(this IServiceCollection services)
        where TGlobalSetup : class, IPipelineGlobalHooks
    {
        return services.AddScoped<IPipelineGlobalHooks, TGlobalSetup>();
    }

    /// <summary>
    /// Adds module hooks to run before or after each module has executed.
    /// </summary>
    /// <param name="services">The pipeline's service collection.</param>
    /// <typeparam name="TModuleHooks">The type of hook class.</typeparam>
    /// <returns>The pipeline's same service collection.</returns>
    public static IServiceCollection AddPipelineModuleHooks<TModuleHooks>(this IServiceCollection services)
        where TModuleHooks : class, IPipelineModuleHooks
    {
        return services.AddSingleton<IPipelineModuleHooks, TModuleHooks>();
    }

    internal static IServiceCollection AddServiceCollection(this IServiceCollection serviceCollection)
    {
        return serviceCollection.AddSingleton<IPipelineServiceContainerWrapper>(new PipelineServiceContainerWrapper(serviceCollection));
    }
}