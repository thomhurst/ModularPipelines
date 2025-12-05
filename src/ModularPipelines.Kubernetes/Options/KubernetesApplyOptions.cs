using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CliCommand("apply")]
[ExcludeFromCodeCoverage]
public record KubernetesApplyOptions : KubernetesOptions
{
    [CliFlag("--all")]
    public virtual bool? All { get; set; }

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

    [CliFlag("--force-conflicts")]
    public virtual bool? ForceConflicts { get; set; }

    [CliOption("--grace-period")]
    public virtual int? GracePeriod { get; set; }

    [CliOption("--kustomize")]
    public virtual string? Kustomize { get; set; }

    [CliFlag("--openapi-patch")]
    public virtual bool? OpenapiPatch { get; set; }

    [CliOption("--output")]
    public virtual string? Output { get; set; }

    [CliFlag("--overwrite")]
    public virtual bool? Overwrite { get; set; }

    [CliFlag("--prune")]
    public virtual bool? Prune { get; set; }

    [CliOption("--prune-whitelist")]
    public virtual string[]? PruneWhitelist { get; set; }

    [CliFlag("--record")]
    public virtual bool? Record { get; set; }

    [CliFlag("--recursive")]
    public virtual bool? Recursive { get; set; }

    [CliOption("--selector")]
    public virtual string? Selector { get; set; }

    [CliFlag("--server-side")]
    public virtual bool? ServerSide { get; set; }

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