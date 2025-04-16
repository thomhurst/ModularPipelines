using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CommandPrecedingArguments("apply")]
[ExcludeFromCodeCoverage]
public record KubernetesApplyOptions : KubernetesOptions
{
    [BooleanCommandSwitch("--all")]
    public virtual bool? All { get; set; }

    [BooleanCommandSwitch("--allow-missing-template-keys")]
    public virtual bool? AllowMissingTemplateKeys { get; set; }

    [CommandEqualsSeparatorSwitch("--cascade", SwitchValueSeparator = " ")]
    public string? Cascade { get; set; }

    [CommandEqualsSeparatorSwitch("--dry-run", SwitchValueSeparator = " ")]
    public string? DryRun { get; set; }

    [CommandEqualsSeparatorSwitch("--field-manager", SwitchValueSeparator = " ")]
    public string? FieldManager { get; set; }

    [CommandEqualsSeparatorSwitch("--filename", SwitchValueSeparator = " ")]
    public string[]? Filename { get; set; }

    [BooleanCommandSwitch("--force")]
    public virtual bool? Force { get; set; }

    [BooleanCommandSwitch("--force-conflicts")]
    public virtual bool? ForceConflicts { get; set; }

    [CommandEqualsSeparatorSwitch("--grace-period", SwitchValueSeparator = " ")]
    public int? GracePeriod { get; set; }

    [CommandEqualsSeparatorSwitch("--kustomize", SwitchValueSeparator = " ")]
    public string? Kustomize { get; set; }

    [BooleanCommandSwitch("--openapi-patch")]
    public virtual bool? OpenapiPatch { get; set; }

    [CommandEqualsSeparatorSwitch("--output", SwitchValueSeparator = " ")]
    public string? Output { get; set; }

    [BooleanCommandSwitch("--overwrite")]
    public virtual bool? Overwrite { get; set; }

    [BooleanCommandSwitch("--prune")]
    public virtual bool? Prune { get; set; }

    [CommandEqualsSeparatorSwitch("--prune-whitelist", SwitchValueSeparator = " ")]
    public string[]? PruneWhitelist { get; set; }

    [BooleanCommandSwitch("--record")]
    public virtual bool? Record { get; set; }

    [BooleanCommandSwitch("--recursive")]
    public virtual bool? Recursive { get; set; }

    [CommandEqualsSeparatorSwitch("--selector", SwitchValueSeparator = " ")]
    public string? Selector { get; set; }

    [BooleanCommandSwitch("--server-side")]
    public virtual bool? ServerSide { get; set; }

    [BooleanCommandSwitch("--show-managed-fields")]
    public virtual bool? ShowManagedFields { get; set; }

    [CommandEqualsSeparatorSwitch("--template", SwitchValueSeparator = " ")]
    public string? Template { get; set; }

    [CommandEqualsSeparatorSwitch("--timeout", SwitchValueSeparator = " ")]
    public string? Timeout { get; set; }

    [BooleanCommandSwitch("--validate")]
    public virtual bool? Validate { get; set; }

    [BooleanCommandSwitch("--wait")]
    public virtual bool? Wait { get; set; }
}