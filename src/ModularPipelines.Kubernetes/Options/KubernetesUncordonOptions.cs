using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CliSubCommand("uncordon")]
[ExcludeFromCodeCoverage]
public record KubernetesUncordonOptions([property: CliArgument] string Node) : KubernetesOptions
{
    [CliOption("--dry-run")]
    public virtual string? DryRun { get; set; }

    [CliOption("--selector")]
    public virtual string? Selector { get; set; }
}