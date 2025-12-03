using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CliCommand("delete")]
[ExcludeFromCodeCoverage]
public record KubernetesDeleteOptions([property: CliArgument] string Name) : KubernetesOptions
{
    [CliFlag("--all")]
    public virtual bool? All { get; set; }

    [CliFlag("--all-namespaces")]
    public virtual bool? AllNamespaces { get; set; }

    [CliOption("--cascade")]
    public string? Cascade { get; set; }

    [CliOption("--dry-run")]
    public string? DryRun { get; set; }

    [CliOption("--field-selector")]
    public string? FieldSelector { get; set; }

    [CliOption("--filename")]
    public string[]? Filename { get; set; }

    [CliFlag("--force")]
    public virtual bool? Force { get; set; }

    [CliOption("--grace-period")]
    public int? GracePeriod { get; set; }

    [CliFlag("--ignore-not-found")]
    public virtual bool? IgnoreNotFound { get; set; }

    [CliOption("--kustomize")]
    public string? Kustomize { get; set; }

    [CliFlag("--now")]
    public virtual bool? Now { get; set; }

    [CliOption("--output")]
    public string? Output { get; set; }

    [CliOption("--raw")]
    public string? Raw { get; set; }

    [CliFlag("--recursive")]
    public virtual bool? Recursive { get; set; }

    [CliOption("--selector")]
    public string? Selector { get; set; }

    [CliOption("--timeout")]
    public string? Timeout { get; set; }

    [CliFlag("--wait")]
    public virtual bool? Wait { get; set; }
}