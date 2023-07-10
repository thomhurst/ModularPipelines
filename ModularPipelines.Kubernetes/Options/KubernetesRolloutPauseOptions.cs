using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CommandPrecedingArguments("rollout", "pause")]
public record KubernetesRolloutPauseOptions([property: PositionalArgument] string Resource) : KubernetesOptions
{
    [BooleanCommandSwitch("allow-missing-template-keys")]
    public bool? AllowMissingTemplateKeys { get; set; }

    [CommandLongSwitch("field-manager", SwitchValueSeparator = " ")]
    public string? FieldManager { get; set; }

    [CommandLongSwitch("filename", SwitchValueSeparator = " ")]
    public string[]? Filename { get; set; }

    [CommandLongSwitch("kustomize", SwitchValueSeparator = " ")]
    public string? Kustomize { get; set; }

    [CommandLongSwitch("output", SwitchValueSeparator = " ")]
    public string? Output { get; set; }

    [BooleanCommandSwitch("recursive")]
    public bool? Recursive { get; set; }

    [BooleanCommandSwitch("show-managed-fields")]
    public bool? ShowManagedFields { get; set; }

    [CommandLongSwitch("template", SwitchValueSeparator = " ")]
    public string? Template { get; set; }

}
