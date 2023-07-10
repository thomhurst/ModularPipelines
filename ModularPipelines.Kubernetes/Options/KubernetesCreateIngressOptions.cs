using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CommandPrecedingArguments("create", "ingress")]
public record KubernetesCreateIngressOptions([property: PositionalArgument] string Name) : KubernetesOptions
{
    [BooleanCommandSwitch("allow-missing-template-keys")]
    public bool? AllowMissingTemplateKeys { get; set; }

    [CommandLongSwitch("annotation", SwitchValueSeparator = " ")]
    public string[]? Annotation { get; set; }

    [CommandLongSwitch("class", SwitchValueSeparator = " ")]
    public string? Class { get; set; }

    [CommandLongSwitch("default-backend", SwitchValueSeparator = " ")]
    public string? DefaultBackend { get; set; }

    [CommandLongSwitch("dry-run", SwitchValueSeparator = " ")]
    public string? DryRun { get; set; }

    [CommandLongSwitch("field-manager", SwitchValueSeparator = " ")]
    public string? FieldManager { get; set; }

    [CommandLongSwitch("output", SwitchValueSeparator = " ")]
    public string? Output { get; set; }

    [CommandLongSwitch("rule", SwitchValueSeparator = " ")]
    public string[]? Rule { get; set; }

    [BooleanCommandSwitch("save-config")]
    public bool? SaveConfig { get; set; }

    [BooleanCommandSwitch("show-managed-fields")]
    public bool? ShowManagedFields { get; set; }

    [CommandLongSwitch("template", SwitchValueSeparator = " ")]
    public string? Template { get; set; }

    [BooleanCommandSwitch("validate")]
    public bool? Validate { get; set; }

}
