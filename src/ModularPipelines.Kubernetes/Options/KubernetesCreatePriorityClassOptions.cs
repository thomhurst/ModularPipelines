using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CommandPrecedingArguments("create", "priorityclass")]
[ExcludeFromCodeCoverage]
public record KubernetesCreatePriorityClassOptions([property: PositionalArgument] string Name) : KubernetesOptions
{
    [BooleanCommandSwitch("--allow-missing-template-keys")]
    public virtual bool? AllowMissingTemplateKeys { get; set; }

    [CommandEqualsSeparatorSwitch("--description", SwitchValueSeparator = " ")]
    public string? Description { get; set; }

    [CommandEqualsSeparatorSwitch("--dry-run", SwitchValueSeparator = " ")]
    public string? DryRun { get; set; }

    [CommandEqualsSeparatorSwitch("--field-manager", SwitchValueSeparator = " ")]
    public string? FieldManager { get; set; }

    [BooleanCommandSwitch("--global-default")]
    public virtual bool? GlobalDefault { get; set; }

    [CommandEqualsSeparatorSwitch("--output", SwitchValueSeparator = " ")]
    public string? Output { get; set; }

    [CommandEqualsSeparatorSwitch("--preemption-policy", SwitchValueSeparator = " ")]
    public string? PreemptionPolicy { get; set; }

    [BooleanCommandSwitch("--save-config")]
    public virtual bool? SaveConfig { get; set; }

    [BooleanCommandSwitch("--show-managed-fields")]
    public virtual bool? ShowManagedFields { get; set; }

    [CommandEqualsSeparatorSwitch("--template", SwitchValueSeparator = " ")]
    public string? Template { get; set; }

    [BooleanCommandSwitch("--validate")]
    public virtual bool? Validate { get; set; }

    [CommandEqualsSeparatorSwitch("--value", SwitchValueSeparator = " ")]
    public int? Value { get; set; }
}