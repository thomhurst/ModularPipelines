using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CliCommand("apply", "view-last-applied")]
[ExcludeFromCodeCoverage]
public record KubernetesApplyViewLastAppliedOptions([property: CliArgument] string Name) : KubernetesOptions
{
    [CliFlag("--all")]
    public virtual bool? All { get; set; }

    [CliOption("--filename")]
    public virtual string[]? Filename { get; set; }

    [CliOption("--kustomize")]
    public virtual string? Kustomize { get; set; }

    [CliOption("--output")]
    public virtual string? Output { get; set; }

    [CliFlag("--recursive")]
    public virtual bool? Recursive { get; set; }

    [CliOption("--selector")]
    public virtual string? Selector { get; set; }
}