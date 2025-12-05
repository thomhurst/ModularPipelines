using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CliCommand("scale")]
[ExcludeFromCodeCoverage]
public record KubernetesScaleOptions : KubernetesOptions
{
    [CliFlag("--all")]
    public virtual bool? All { get; set; }

    [CliFlag("--allow-missing-template-keys")]
    public virtual bool? AllowMissingTemplateKeys { get; set; }

    [CliOption("--current-replicas")]
    public virtual int? CurrentReplicas { get; set; }

    [CliOption("--dry-run")]
    public virtual string? DryRun { get; set; }

    [CliOption("--filename")]
    public virtual string[]? Filename { get; set; }

    [CliOption("--kustomize")]
    public virtual string? Kustomize { get; set; }

    [CliOption("--output")]
    public virtual string? Output { get; set; }

    [CliFlag("--record")]
    public virtual bool? Record { get; set; }

    [CliFlag("--recursive")]
    public virtual bool? Recursive { get; set; }

    [CliOption("--replicas")]
    public virtual int? Replicas { get; set; }

    [CliOption("--resource-version")]
    public virtual string? ResourceVersion { get; set; }

    [CliOption("--selector")]
    public virtual string? Selector { get; set; }

    [CliFlag("--show-managed-fields")]
    public virtual bool? ShowManagedFields { get; set; }

    [CliOption("--template")]
    public virtual string? Template { get; set; }

    [CliOption("--timeout")]
    public virtual string? Timeout { get; set; }
}