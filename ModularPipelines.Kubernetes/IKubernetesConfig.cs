using ModularPipelines.Models;
using ModularPipelines.Kubernetes.Options;

namespace ModularPipelines.Kubernetes;

public interface IKubernetesConfig
{
    Task UseContext( KubernetesUseContextOptions kubernetesUseContextOptions );
    Task<CommandResult> View( KubernetesViewConfigOptions viewConfigOptions );
    Task<CommandResult> GetContexts();
    Task<CommandResult> CurrentContext();
}
