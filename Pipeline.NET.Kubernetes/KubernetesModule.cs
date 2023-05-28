using k8s;
using Pipeline.NET.Context;
using Pipeline.NET.Modules;

namespace Pipeline.NET.Kubernetes;

public abstract class KubernetesModule : Module
{
    protected KubernetesModule(IModuleContext context) : base(context)
    {
        KubernetesClient = null!;
    }
    
    protected abstract KubernetesClientConfiguration KubernetesClientConfiguration { get; }

    protected override Task InitialiseAsync()
    {
        KubernetesClient = new k8s.Kubernetes(KubernetesClientConfiguration);
        return base.InitialiseAsync();
    }

    protected k8s.Kubernetes KubernetesClient { get; set; }
}