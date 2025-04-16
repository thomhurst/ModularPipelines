using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CommandPrecedingArguments("replace")]
[ExcludeFromCodeCoverage]
public record KubernetesReplaceOptions : KubernetesOptions
{
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

    [CommandEqualsSeparatorSwitch("--grace-period", SwitchValueSeparator = " ")]
    public int? GracePeriod { get; set; }

    [CommandEqualsSeparatorSwitch("--kustomize", SwitchValueSeparator = " ")]
    public string? Kustomize { get; set; }

    [CommandEqualsSeparatorSwitch("--output", SwitchValueSeparator = " ")]
    public string? Output { get; set; }

    [CommandEqualsSeparatorSwitch("--raw", SwitchValueSeparator = " ")]
    public string? Raw { get; set; }

    [BooleanCommandSwitch("--recursive")]
    public virtual bool? Recursive { get; set; }

    [BooleanCommandSwitch("--save-config")]
    public virtual bool? SaveConfig { get; set; }

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