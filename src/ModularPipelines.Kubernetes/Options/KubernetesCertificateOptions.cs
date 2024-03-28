using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[ExcludeFromCodeCoverage]
public record KubernetesCertificateOptions : KubernetesOptions
{
    public KubernetesCertificateOptions()
    {
        CommandParts = ["certificate"];
    }

    [PositionalArgument(PlaceholderName = "<SUBCOMMAND>")]
    public string? SUBCOMMAND { get; set; }
}
