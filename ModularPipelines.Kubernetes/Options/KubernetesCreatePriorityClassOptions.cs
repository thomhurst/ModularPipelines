using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CommandPrecedingArguments("create", "priorityclass")]
public record KubernetesCreatePriorityClassOptions([property: PositionalArgument] string Name) : KubernetesOptions
{
    [BooleanCommandSwitch("allow-missing-template-keys")]
    public bool? AllowMissingTemplateKeys { get; set; }

    [CommandLongSwitch("description", SwitchValueSeparator = " ")]
    public string? Description { get; set; }

    [CommandLongSwitch("dry-run", SwitchValueSeparator = " ")]
    public string? DryRun { get; set; }

    [CommandLongSwitch("field-manager", SwitchValueSeparator = " ")]
    public string? FieldManager { get; set; }

    [BooleanCommandSwitch("global-default")]
    public bool? GlobalDefault { get; set; }

    [CommandLongSwitch("output", SwitchValueSeparator = " ")]
    public string? Output { get; set; }

    [CommandLongSwitch("preemption-policy", SwitchValueSeparator = " ")]
    public string? PreemptionPolicy { get; set; }

    [BooleanCommandSwitch("save-config")]
    public bool? SaveConfig { get; set; }

    [BooleanCommandSwitch("show-managed-fields")]
    public bool? ShowManagedFields { get; set; }

    [CommandLongSwitch("template", SwitchValueSeparator = " ")]
    public string? Template { get; set; }

    [BooleanCommandSwitch("validate")]
    public bool? Validate { get; set; }

    [CommandLongSwitch("value", SwitchValueSeparator = " ")]
    public int? Value { get; set; }

}
