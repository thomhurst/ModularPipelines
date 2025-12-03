using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CliCommand("replace")]
[ExcludeFromCodeCoverage]
public record KubernetesReplaceOptions : KubernetesOptions
{
    [CliFlag("--allow-missing-template-keys")]
    public virtual bool? AllowMissingTemplateKeys { get; set; }

    [CliOption("--cascade")]
    public string? Cascade { get; set; }

    [CliOption("--dry-run")]
    public string? DryRun { get; set; }

    [CliOption("--field-manager")]
    public string? FieldManager { get; set; }

    [CliOption("--filename")]
    public string[]? Filename { get; set; }

    [CliFlag("--force")]
    public virtual bool? Force { get; set; }

    [CliOption("--grace-period")]
    public int? GracePeriod { get; set; }

    [CliOption("--kustomize")]
    public string? Kustomize { get; set; }

    [CliOption("--output")]
    public string? Output { get; set; }

    [CliOption("--raw")]
    public string? Raw { get; set; }

    [CliFlag("--recursive")]
    public virtual bool? Recursive { get; set; }

    [CliFlag("--save-config")]
    public virtual bool? SaveConfig { get; set; }

    [CliFlag("--show-managed-fields")]
    public virtual bool? ShowManagedFields { get; set; }

    [CliOption("--template")]
    public string? Template { get; set; }

    [CliOption("--timeout")]
    public string? Timeout { get; set; }

    [CliFlag("--validate")]
    public virtual bool? Validate { get; set; }

    [CliFlag("--wait")]
    public virtual bool? Wait { get; set; }
}