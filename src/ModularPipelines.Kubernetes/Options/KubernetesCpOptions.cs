using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CliCommand("cp", "<file-spec-src>", "<file-spec-dest>")]
[ExcludeFromCodeCoverage]
public record KubernetesCpOptions : KubernetesOptions
{
    [CliOption("--container")]
    public virtual string? Container { get; set; }

    [CliFlag("--no-preserve")]
    public virtual bool? NoPreserve { get; set; }
}