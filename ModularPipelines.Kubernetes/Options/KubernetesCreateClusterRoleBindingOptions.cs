using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CommandPrecedingArguments("create", "clusterrolebinding")]
public record KubernetesCreateClusterRoleBindingOptions([property: PositionalArgument] string Name) : KubernetesOptions
{
    [BooleanCommandSwitch("allow-missing-template-keys")]
    public bool? AllowMissingTemplateKeys { get; set; }

    [CommandLongSwitch("clusterrole", SwitchValueSeparator = " ")]
    public string? Clusterrole { get; set; }

    [CommandLongSwitch("dry-run", SwitchValueSeparator = " ")]
    public string? DryRun { get; set; }

    [CommandLongSwitch("field-manager", SwitchValueSeparator = " ")]
    public string? FieldManager { get; set; }

    [CommandLongSwitch("group", SwitchValueSeparator = " ")]
    public string[]? Group { get; set; }

    [CommandLongSwitch("output", SwitchValueSeparator = " ")]
    public string? Output { get; set; }

    [BooleanCommandSwitch("save-config")]
    public bool? SaveConfig { get; set; }

    [CommandLongSwitch("serviceaccount", SwitchValueSeparator = " ")]
    public string[]? Serviceaccount { get; set; }

    [BooleanCommandSwitch("show-managed-fields")]
    public bool? ShowManagedFields { get; set; }

    [CommandLongSwitch("template", SwitchValueSeparator = " ")]
    public string? Template { get; set; }

    [BooleanCommandSwitch("validate")]
    public bool? Validate { get; set; }

}
