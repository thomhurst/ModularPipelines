namespace ModularPipelines.Kubernetes.Options;

public record KubernetesApplyOptions( IReadOnlyCollection<string> Files ) : KubernetesOptions;
