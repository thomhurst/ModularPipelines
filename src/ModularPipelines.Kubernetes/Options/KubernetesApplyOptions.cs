using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[ExcludeFromCodeCoverage]
public record KubernetesApplyOptions : KubernetesOptions
{
    public KubernetesApplyOptions(
        IEnumerable<string> filename
)
    {
        CommandParts = ["apply"];
        Filename = filename;
    }

    public KubernetesApplyOptions(
        string kustomize
)
    {
        CommandParts = ["apply"];
        Kustomize = kustomize;
    }

    [BooleanCommandSwitch("--all")]
    public bool? All { get; set; }

    [BooleanCommandSwitch("--allow-missing-template-keys")]
    public bool? AllowMissingTemplateKeys { get; set; }

    [CommandSwitch("--cascade")]
    public string? Cascade { get; set; }

    [CommandSwitch("--dry-run")]
    public string? DryRun { get; set; }

    [CommandSwitch("--field-manager")]
    public string? FieldManager { get; set; }

    [CommandSwitch("--filename")]
    public IEnumerable<string>? Filename { get; set; }

    [BooleanCommandSwitch("--force")]
    public bool? Force { get; set; }

    [BooleanCommandSwitch("--force-conflicts")]
    public bool? ForceConflicts { get; set; }

    [CommandSwitch("--grace-period")]
    public int? GracePeriod { get; set; }

    [CommandSwitch("--kustomize")]
    public string? Kustomize { get; set; }

    [BooleanCommandSwitch("--openapi-patch")]
    public bool? OpenapiPatch { get; set; }

    [CommandSwitch("--output")]
    public string? Output { get; set; }

    [BooleanCommandSwitch("--overwrite")]
    public bool? Overwrite { get; set; }

    [BooleanCommandSwitch("--prune")]
    public bool? Prune { get; set; }

    [CommandSwitch("--prune-whitelist")]
    public IEnumerable<string>? PruneWhitelist { get; set; }

    [BooleanCommandSwitch("--record")]
    public bool? Record { get; set; }

    [BooleanCommandSwitch("--recursive")]
    public bool? Recursive { get; set; }

    [CommandSwitch("--selector")]
    public string? Selector { get; set; }

    [BooleanCommandSwitch("--server-side")]
    public bool? ServerSide { get; set; }

    [BooleanCommandSwitch("--show-managed-fields")]
    public bool? ShowManagedFields { get; set; }

    [CommandSwitch("--template")]
    public string? Template { get; set; }

    [CommandSwitch("--timeout")]
    public string? Timeout { get; set; }

    [BooleanCommandSwitch("--validate")]
    public bool? Validate { get; set; }

    [BooleanCommandSwitch("--wait")]
    public bool? Wait { get; set; }
}
