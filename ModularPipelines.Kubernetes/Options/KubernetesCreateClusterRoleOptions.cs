using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CommandPrecedingArguments("create", "clusterrole")]
public record KubernetesCreateClusterRoleOptions([property: PositionalArgument] string Name) : KubernetesOptions
{
    [CommandLongSwitch("aggregation-rule", SwitchValueSeparator = " ")]
    public string? AggregationRule { get; set; }

    [BooleanCommandSwitch("allow-missing-template-keys")]
    public bool? AllowMissingTemplateKeys { get; set; }

    [CommandLongSwitch("dry-run", SwitchValueSeparator = " ")]
    public string? DryRun { get; set; }

    [CommandLongSwitch("field-manager", SwitchValueSeparator = " ")]
    public string? FieldManager { get; set; }

    [CommandLongSwitch("non-resource-url", SwitchValueSeparator = " ")]
    public string[]? NonResourceUrl { get; set; }

    [CommandLongSwitch("output", SwitchValueSeparator = " ")]
    public string? Output { get; set; }

    [CommandLongSwitch("resource", SwitchValueSeparator = " ")]
    public string[]? Resource { get; set; }

    [CommandLongSwitch("resource-name", SwitchValueSeparator = " ")]
    public string[]? ResourceName { get; set; }

    [BooleanCommandSwitch("save-config")]
    public bool? SaveConfig { get; set; }

    [BooleanCommandSwitch("show-managed-fields")]
    public bool? ShowManagedFields { get; set; }

    [CommandLongSwitch("template", SwitchValueSeparator = " ")]
    public string? Template { get; set; }

    [BooleanCommandSwitch("validate")]
    public bool? Validate { get; set; }

    [CommandLongSwitch("verb", SwitchValueSeparator = " ")]
    public string[]? Verb { get; set; }

}
