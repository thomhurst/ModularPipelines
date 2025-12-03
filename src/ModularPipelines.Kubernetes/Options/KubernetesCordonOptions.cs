using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CliCommand("cordon")]
[ExcludeFromCodeCoverage]
public record KubernetesCordonOptions([property: CliArgument] string Node) : KubernetesOptions
{
    [CliOption("--dry-run")]
    public string? DryRun { get; set; }

    [CliOption("--selector")]
    public string? Selector { get; set; }
}