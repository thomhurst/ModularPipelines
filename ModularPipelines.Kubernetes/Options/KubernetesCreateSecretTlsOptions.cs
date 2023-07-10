using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CommandPrecedingArguments("create", "tls")]
public record KubernetesCreateSecretTlsOptions([property: PositionalArgument] string Name) : KubernetesOptions
{
    [BooleanCommandSwitch("allow-missing-template-keys")]
    public bool? AllowMissingTemplateKeys { get; set; }

    [BooleanCommandSwitch("append-hash")]
    public bool? AppendHash { get; set; }

    [CommandLongSwitch("cert", SwitchValueSeparator = " ")]
    public string? Cert { get; set; }

    [CommandLongSwitch("dry-run", SwitchValueSeparator = " ")]
    public string? DryRun { get; set; }

    [CommandLongSwitch("field-manager", SwitchValueSeparator = " ")]
    public string? FieldManager { get; set; }

    [CommandLongSwitch("key", SwitchValueSeparator = " ")]
    public string? Key { get; set; }

    [CommandLongSwitch("output", SwitchValueSeparator = " ")]
    public string? Output { get; set; }

    [BooleanCommandSwitch("save-config")]
    public bool? SaveConfig { get; set; }

    [BooleanCommandSwitch("show-managed-fields")]
    public bool? ShowManagedFields { get; set; }

    [CommandLongSwitch("template", SwitchValueSeparator = " ")]
    public string? Template { get; set; }

    [BooleanCommandSwitch("validate")]
    public bool? Validate { get; set; }

}
