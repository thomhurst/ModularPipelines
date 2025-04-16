using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CommandPrecedingArguments("create", "secret")]
[ExcludeFromCodeCoverage]
public record KubernetesCreateSecretOptions : KubernetesOptions
{
    [BooleanCommandSwitch("--allow-missing-template-keys")]
    public virtual bool? AllowMissingTemplateKeys { get; set; }

    [BooleanCommandSwitch("--append-hash")]
    public virtual bool? AppendHash { get; set; }

    [CommandEqualsSeparatorSwitch("--docker-email", SwitchValueSeparator = " ")]
    public string? DockerEmail { get; set; }

    [CommandEqualsSeparatorSwitch("--docker-password", SwitchValueSeparator = " ")]
    public string? DockerPassword { get; set; }

    [CommandEqualsSeparatorSwitch("--docker-server", SwitchValueSeparator = " ")]
    public string? DockerServer { get; set; }

    [CommandEqualsSeparatorSwitch("--docker-username", SwitchValueSeparator = " ")]
    public string? DockerUsername { get; set; }

    [CommandEqualsSeparatorSwitch("--dry-run", SwitchValueSeparator = " ")]
    public string? DryRun { get; set; }

    [CommandEqualsSeparatorSwitch("--field-manager", SwitchValueSeparator = " ")]
    public string? FieldManager { get; set; }

    [CommandEqualsSeparatorSwitch("--from-file", SwitchValueSeparator = " ")]
    public string[]? FromFile { get; set; }

    [CommandEqualsSeparatorSwitch("--output", SwitchValueSeparator = " ")]
    public string? Output { get; set; }

    [BooleanCommandSwitch("--save-config")]
    public virtual bool? SaveConfig { get; set; }

    [BooleanCommandSwitch("--show-managed-fields")]
    public virtual bool? ShowManagedFields { get; set; }

    [CommandEqualsSeparatorSwitch("--template", SwitchValueSeparator = " ")]
    public string? Template { get; set; }

    [BooleanCommandSwitch("--validate")]
    public virtual bool? Validate { get; set; }
}