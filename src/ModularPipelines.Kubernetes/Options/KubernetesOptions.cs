using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Options;

namespace ModularPipelines.Kubernetes.Options;

[ExcludeFromCodeCoverage]
public record KubernetesOptions() : CommandLineToolOptions("kubectl");
