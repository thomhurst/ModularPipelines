using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ModularPipelines.Context;
using ModularPipelines.Engine;

namespace ModularPipelines.Kubernetes.Extensions;

public static class KubernetesExtensions
{
#pragma warning disable CA2255
    [ModuleInitializer]
#pragma warning restore CA2255
    public static void RegisterKubernetesContext()
    {
        ModularPipelinesContextRegistry.RegisterContext(collection => RegisterKubernetesContext(collection));
    }
    
    public static IServiceCollection RegisterKubernetesContext(this IServiceCollection services)
    {
        services.TryAddTransient<IKubernetes, Kubernetes>();
        return services;
    }

    public static IKubernetes Kubernetes(this IModuleContext context) => context.ServiceProvider.GetRequiredService<IKubernetes>();

}