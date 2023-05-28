using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Interfaces;
using ModularPipelines.Modules;
using ModularPipelines.Requirements;

namespace ModularPipelines.Extensions;

public static class ServiceCollectionExtensions
{
     public static IServiceCollection AddModule<TModule>(this IServiceCollection services) where TModule : class, IModule
    {
        return services.AddSingleton<IModule, TModule>();
    }
    
    public static IServiceCollection AddModule<TModule>(this IServiceCollection services, TModule tModule) where TModule : class, IModule
    {
        return services.AddSingleton<IModule>(tModule);
    }
    
    public static IServiceCollection AddModule<TModule>(this IServiceCollection services, Func<IServiceProvider, TModule> tModuleFactory) where TModule : class, IModule
    {
        return services.AddSingleton<IModule>(tModuleFactory);
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
            .Where(type => type.IsAssignableTo(typeof(IModule)))
            .Where(type => type.IsClass)
            .Where(type => !type.IsAbstract);

        foreach (var module in modules)
        {
            services.AddSingleton(typeof(IModule), module);
        }

        return services;
    }

    public static IServiceCollection AddPipelineGlobalSetup<TGlobalSetup>(this IServiceCollection services) where TGlobalSetup : class, IPipelineGlobalHooks
    {
        return services.AddSingleton<IPipelineGlobalHooks, TGlobalSetup>();
    }
}