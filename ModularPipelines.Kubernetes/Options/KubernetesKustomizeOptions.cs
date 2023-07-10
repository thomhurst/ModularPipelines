using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CommandPrecedingArguments("kustomize")]
public record KubernetesKustomizeOptions([property: PositionalArgument] string Dir) : KubernetesOptions
{
    [BooleanCommandSwitch("as-current-user")]
    public bool? AsCurrentUser { get; set; }

    [BooleanCommandSwitch("enable-alpha-plugins")]
    public bool? EnableAlphaPlugins { get; set; }

    [BooleanCommandSwitch("enable-helm")]
    public bool? EnableHelm { get; set; }

    [BooleanCommandSwitch("enable-managedby-label")]
    public bool? EnableManagedbyLabel { get; set; }

    [CommandLongSwitch("env", SwitchValueSeparator = " ")]
    public string[]? Env { get; set; }

    [CommandLongSwitch("helm-command", SwitchValueSeparator = " ")]
    public string? HelmCommand { get; set; }

    [CommandLongSwitch("load-restrictor", SwitchValueSeparator = " ")]
    public string? LoadRestrictor { get; set; }

    [CommandLongSwitch("mount", SwitchValueSeparator = " ")]
    public string[]? Mount { get; set; }

    [BooleanCommandSwitch("network")]
    public bool? Network { get; set; }

    [CommandLongSwitch("network-name", SwitchValueSeparator = " ")]
    public string? NetworkName { get; set; }

    [CommandLongSwitch("output", SwitchValueSeparator = " ")]
    public string? Output { get; set; }

    [CommandLongSwitch("reorder", SwitchValueSeparator = " ")]
    public string? Reorder { get; set; }

}
