using ModularPipelines.Kubernetes.Options;

namespace ModularPipelines.Kubernetes;

public interface IKubernetes
{
    IKubernetesConfig Config { get; }

    Task Apply( KubernetesApplyOptions applyOptions, CancellationToken cancellationToken = default );
}
