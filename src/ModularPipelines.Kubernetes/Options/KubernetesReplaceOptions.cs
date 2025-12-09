using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CliSubCommand("replace")]
[ExcludeFromCodeCoverage]
public record KubernetesReplaceOptions : KubernetesOptions
{
    [CliFlag("--allow-missing-template-keys")]
    public virtual bool? AllowMissingTemplateKeys { get; set; }

    [CliOption("--cascade")]
    public virtual string? Cascade { get; set; }

    [CliOption("--dry-run")]
    public virtual string? DryRun { get; set; }

    [CliOption("--field-manager")]
    public virtual string? FieldManager { get; set; }

    [CliOption("--filename")]
    public virtual string[]? Filename { get; set; }

    [CliFlag("--force")]
    public virtual bool? Force { get; set; }

    [CliOption("--grace-period")]
    public virtual int? GracePeriod { get; set; }

    [CliOption("--kustomize")]
    public virtual string? Kustomize { get; set; }

    [CliOption("--output")]
    public virtual string? Output { get; set; }

    [CliOption("--raw")]
    public virtual string? Raw { get; set; }

    [CliFlag("--recursive")]
    public virtual bool? Recursive { get; set; }

    [CliFlag("--save-config")]
    public virtual bool? SaveConfig { get; set; }

    [CliFlag("--show-managed-fields")]
    public virtual bool? ShowManagedFields { get; set; }

    [CliOption("--template")]
    public virtual string? Template { get; set; }

    [CliOption("--timeout")]
    public virtual string? Timeout { get; set; }

    [CliFlag("--validate")]
    public virtual bool? Validate { get; set; }

    [CliFlag("--wait")]
    public virtual bool? Wait { get; set; }
}