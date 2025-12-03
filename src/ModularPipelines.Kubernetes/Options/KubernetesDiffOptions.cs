using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CliCommand("diff")]
[ExcludeFromCodeCoverage]
public record KubernetesDiffOptions : KubernetesOptions
{
    [CliOption("--field-manager")]
    public string? FieldManager { get; set; }

    [CliOption("--filename")]
    public string[]? Filename { get; set; }

    [CliFlag("--force-conflicts")]
    public virtual bool? ForceConflicts { get; set; }

    [CliOption("--kustomize")]
    public string? Kustomize { get; set; }

    [CliFlag("--recursive")]
    public virtual bool? Recursive { get; set; }

    [CliOption("--selector")]
    public string? Selector { get; set; }

    [CliFlag("--server-side")]
    public virtual bool? ServerSide { get; set; }
}