using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Interfaces;
using ModularPipelines.Modules;
using ModularPipelines.Requirements;

namespace ModularPipelines.Extensions;

public static class ServiceCollectionExtensions
{
     public static IServiceCollection AddModule<TModule>(this IServiceCollection services) where TModule : ModuleBase
    {
        return services.AddSingleton<ModuleBase, TModule>();
    }
    
    public static IServiceCollection AddModule<TModule>(this IServiceCollection services, TModule tModule) where TModule : ModuleBase
    {
        return services.AddSingleton<ModuleBase>(tModule);
    }
    
    public static IServiceCollection AddModule<TModule>(this IServiceCollection services, Func<IServiceProvider, TModule> tModuleFactory) where TModule : ModuleBase
    {
        return services.AddSingleton<ModuleBase>(tModuleFactory);
    }
    
    public static IServiceCollection AddRequirement<TRequirement>(this IServiceCollection services) where TRequirement : class, IPipelineRequirement
    {
        return services.AddSingleton<IPipelineRequirement, TRequirement>();
    }
    
    public static IServiceCollection AddRequirement<TRequirement>(this IServiceCollection services, TRequirement tRequirement) where TRequirement : class, IPipelineRequirement
    {
        return services.AddSingleton<IPipelineRequirement>(tRequirement);
    }
    
    public static IServiceCollection AddRequirement<TRequirement>(this IServiceCollection services, Func<IServiceProvider, TRequirement> tRequirementFactory) where TRequirement : class, IPipelineRequirement
    {
        return services.AddSingleton<IPipelineRequirement>(tRequirementFactory);
    }
    
    public static IServiceCollection AddModulesFromAssemblyContainingType<T>(this IServiceCollection services)
    {
        return AddModulesFromAssembly(services, typeof(T).Assembly);
    }

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

    public static IServiceCollection AddPipelineGlobalHooks<TGlobalSetup>(this IServiceCollection services) where TGlobalSetup : class, IPipelineGlobalHooks
    {
        return services.AddSingleton<IPipelineGlobalHooks, TGlobalSetup>();
    }
    
    public static IServiceCollection AddPipelineModuleHooks<TModuleHooks>(this IServiceCollection services) where TModuleHooks : class, IPipelineModuleHooks
    {
        return services.AddSingleton<IPipelineModuleHooks, TModuleHooks>();
    }
}