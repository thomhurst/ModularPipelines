using ModularPipelines.Options;

namespace ModularPipelines.Kubernetes.Options;

public record KubernetesOptions() : CommandLineToolOptions( "kubectl" );
