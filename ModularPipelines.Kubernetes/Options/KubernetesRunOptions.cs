using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CommandPrecedingArguments("run")]
public record KubernetesRunOptions(string Name) : KubernetesOptions
{
    [BooleanCommandSwitch("allow-missing-template-keys")]
    public bool? AllowMissingTemplateKeys { get; set; }

    [CommandLongSwitch("annotations", SwitchValueSeparator = " ")]
    public string[]? Annotations { get; set; }

    [BooleanCommandSwitch("attach")]
    public bool? Attach { get; set; }

    [CommandLongSwitch("cascade", SwitchValueSeparator = " ")]
    public string? Cascade { get; set; }

    [BooleanCommandSwitch("command")]
    public bool? Command { get; set; }

    [CommandLongSwitch("dry-run", SwitchValueSeparator = " ")]
    public string? DryRun { get; set; }

    [CommandLongSwitch("env", SwitchValueSeparator = " ")]
    public string[]? Env { get; set; }

    [BooleanCommandSwitch("expose")]
    public bool? Expose { get; set; }

    [CommandLongSwitch("field-manager", SwitchValueSeparator = " ")]
    public string? FieldManager { get; set; }

    [CommandLongSwitch("filename", SwitchValueSeparator = " ")]
    public string[]? Filename { get; set; }

    [BooleanCommandSwitch("force")]
    public bool? Force { get; set; }

    [CommandLongSwitch("grace-period", SwitchValueSeparator = " ")]
    public int? GracePeriod { get; set; }

    [CommandLongSwitch("hostport", SwitchValueSeparator = " ")]
    public int? Hostport { get; set; }

    [CommandLongSwitch("image", SwitchValueSeparator = " ")]
    public string? Image { get; set; }

    [CommandLongSwitch("image-pull-policy", SwitchValueSeparator = " ")]
    public string? ImagePullPolicy { get; set; }

    [CommandLongSwitch("kustomize", SwitchValueSeparator = " ")]
    public string? Kustomize { get; set; }

    [CommandLongSwitch("labels", SwitchValueSeparator = " ")]
    public string? Labels { get; set; }

    [BooleanCommandSwitch("leave-stdin-open")]
    public bool? LeaveStdinOpen { get; set; }

    [CommandLongSwitch("limits", SwitchValueSeparator = " ")]
    public string? Limits { get; set; }

    [CommandLongSwitch("output", SwitchValueSeparator = " ")]
    public string? Output { get; set; }

    [CommandLongSwitch("overrides", SwitchValueSeparator = " ")]
    public string? Overrides { get; set; }

    [CommandLongSwitch("pod-running-timeout", SwitchValueSeparator = " ")]
    public string? PodRunningTimeout { get; set; }

    [CommandLongSwitch("port", SwitchValueSeparator = " ")]
    public string? Port { get; set; }

    [BooleanCommandSwitch("privileged")]
    public bool? Privileged { get; set; }

    [BooleanCommandSwitch("quiet")]
    public bool? Quiet { get; set; }

    [BooleanCommandSwitch("record")]
    public bool? Record { get; set; }

    [BooleanCommandSwitch("recursive")]
    public bool? Recursive { get; set; }

    [CommandLongSwitch("requests", SwitchValueSeparator = " ")]
    public string? Requests { get; set; }

    [CommandLongSwitch("restart", SwitchValueSeparator = " ")]
    public string? Restart { get; set; }

    [BooleanCommandSwitch("rm")]
    public bool? Rm { get; set; }

    [BooleanCommandSwitch("save-config")]
    public bool? SaveConfig { get; set; }

    [CommandLongSwitch("serviceaccount", SwitchValueSeparator = " ")]
    public string? Serviceaccount { get; set; }

    [BooleanCommandSwitch("show-managed-fields")]
    public bool? ShowManagedFields { get; set; }

    [BooleanCommandSwitch("stdin")]
    public bool? Stdin { get; set; }

    [CommandLongSwitch("template", SwitchValueSeparator = " ")]
    public string? Template { get; set; }

    [CommandLongSwitch("timeout", SwitchValueSeparator = " ")]
    public string? Timeout { get; set; }

    [BooleanCommandSwitch("tty")]
    public bool? Tty { get; set; }

    [BooleanCommandSwitch("wait")]
    public bool? Wait { get; set; }

}
