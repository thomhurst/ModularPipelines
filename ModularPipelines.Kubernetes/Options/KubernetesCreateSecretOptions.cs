using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CommandPrecedingArguments("create", "secret")]
public record KubernetesCreateSecretOptions : KubernetesOptions
{
    [BooleanCommandSwitch("allow-missing-template-keys")]
    public bool? AllowMissingTemplateKeys { get; set; }

    [BooleanCommandSwitch("append-hash")]
    public bool? AppendHash { get; set; }

    [CommandLongSwitch("docker-email", SwitchValueSeparator = " ")]
    public string? DockerEmail { get; set; }

    [CommandLongSwitch("docker-password", SwitchValueSeparator = " ")]
    public string? DockerPassword { get; set; }

    [CommandLongSwitch("docker-server", SwitchValueSeparator = " ")]
    public string? DockerServer { get; set; }

    [CommandLongSwitch("docker-username", SwitchValueSeparator = " ")]
    public string? DockerUsername { get; set; }

    [CommandLongSwitch("dry-run", SwitchValueSeparator = " ")]
    public string? DryRun { get; set; }

    [CommandLongSwitch("field-manager", SwitchValueSeparator = " ")]
    public string? FieldManager { get; set; }

    [CommandLongSwitch("from-file", SwitchValueSeparator = " ")]
    public string[]? FromFile { get; set; }

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
