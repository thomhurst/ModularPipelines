using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ModularPipelines.Context;
using ModularPipelines.Engine;

namespace ModularPipelines.Kubernetes.Extensions;

[ExcludeFromCodeCoverage]
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
        services.TryAddScoped<IKubernetes, Kubernetes>();
        return services;
    }

    public static IKubernetes Kubernetes(this IPipelineHookContext context) => context.ServiceProvider.GetRequiredService<IKubernetes>();
}
