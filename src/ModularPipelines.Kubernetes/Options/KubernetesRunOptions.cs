using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CommandPrecedingArguments("run")]
public record KubernetesRunOptions([property: PositionalArgument] string Name) : KubernetesOptions
{
    [BooleanCommandSwitch("--allow-missing-template-keys")]
    public bool? AllowMissingTemplateKeys { get; set; }

    [CommandEqualsSeparatorSwitch("--annotations", SwitchValueSeparator = " ")]
    public string[]? Annotations { get; set; }

    [BooleanCommandSwitch("--attach")]
    public bool? Attach { get; set; }

    [CommandEqualsSeparatorSwitch("--cascade", SwitchValueSeparator = " ")]
    public string? Cascade { get; set; }

    [BooleanCommandSwitch("--command")]
    public bool? Command { get; set; }

    [CommandEqualsSeparatorSwitch("--dry-run", SwitchValueSeparator = " ")]
    public string? DryRun { get; set; }

    [CommandEqualsSeparatorSwitch("--env", SwitchValueSeparator = " ")]
    public string[]? Env { get; set; }

    [BooleanCommandSwitch("--expose")]
    public bool? Expose { get; set; }

    [CommandEqualsSeparatorSwitch("--field-manager", SwitchValueSeparator = " ")]
    public string? FieldManager { get; set; }

    [CommandEqualsSeparatorSwitch("--filename", SwitchValueSeparator = " ")]
    public string[]? Filename { get; set; }

    [BooleanCommandSwitch("--force")]
    public bool? Force { get; set; }

    [CommandEqualsSeparatorSwitch("--grace-period", SwitchValueSeparator = " ")]
    public int? GracePeriod { get; set; }

    [CommandEqualsSeparatorSwitch("--hostport", SwitchValueSeparator = " ")]
    public int? Hostport { get; set; }

    [CommandEqualsSeparatorSwitch("--image", SwitchValueSeparator = " ")]
    public string? Image { get; set; }

    [CommandEqualsSeparatorSwitch("--image-pull-policy", SwitchValueSeparator = " ")]
    public string? ImagePullPolicy { get; set; }

    [CommandEqualsSeparatorSwitch("--kustomize", SwitchValueSeparator = " ")]
    public string? Kustomize { get; set; }

    [CommandEqualsSeparatorSwitch("--labels", SwitchValueSeparator = " ")]
    public string? Labels { get; set; }

    [BooleanCommandSwitch("--leave-stdin-open")]
    public bool? LeaveStdinOpen { get; set; }

    [CommandEqualsSeparatorSwitch("--limits", SwitchValueSeparator = " ")]
    public string? Limits { get; set; }

    [CommandEqualsSeparatorSwitch("--output", SwitchValueSeparator = " ")]
    public string? Output { get; set; }

    [CommandEqualsSeparatorSwitch("--overrides", SwitchValueSeparator = " ")]
    public string? Overrides { get; set; }

    [CommandEqualsSeparatorSwitch("--pod-running-timeout", SwitchValueSeparator = " ")]
    public string? PodRunningTimeout { get; set; }

    [CommandEqualsSeparatorSwitch("--port", SwitchValueSeparator = " ")]
    public string? Port { get; set; }

    [BooleanCommandSwitch("--privileged")]
    public bool? Privileged { get; set; }

    [BooleanCommandSwitch("--quiet")]
    public bool? Quiet { get; set; }

    [BooleanCommandSwitch("--record")]
    public bool? Record { get; set; }

    [BooleanCommandSwitch("--recursive")]
    public bool? Recursive { get; set; }

    [CommandEqualsSeparatorSwitch("--requests", SwitchValueSeparator = " ")]
    public string? Requests { get; set; }

    [CommandEqualsSeparatorSwitch("--restart", SwitchValueSeparator = " ")]
    public string? Restart { get; set; }

    [BooleanCommandSwitch("--rm")]
    public bool? Rm { get; set; }

    [BooleanCommandSwitch("--save-config")]
    public bool? SaveConfig { get; set; }

    [CommandEqualsSeparatorSwitch("--serviceaccount", SwitchValueSeparator = " ")]
    public string? Serviceaccount { get; set; }

    [BooleanCommandSwitch("--show-managed-fields")]
    public bool? ShowManagedFields { get; set; }

    [BooleanCommandSwitch("--stdin")]
    public bool? Stdin { get; set; }

    [CommandEqualsSeparatorSwitch("--template", SwitchValueSeparator = " ")]
    public string? Template { get; set; }

    [CommandEqualsSeparatorSwitch("--timeout", SwitchValueSeparator = " ")]
    public string? Timeout { get; set; }

    [BooleanCommandSwitch("--tty")]
    public bool? Tty { get; set; }

    [BooleanCommandSwitch("--wait")]
    public bool? Wait { get; set; }
}
