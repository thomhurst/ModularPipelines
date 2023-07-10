using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CommandPrecedingArguments("attach")]
public record KubernetesAttachOptions([property: PositionalArgument] string Name) : KubernetesOptions
{
    [CommandLongSwitch("container", SwitchValueSeparator = " ")]
    public string? Container { get; set; }

    [CommandLongSwitch("pod-running-timeout", SwitchValueSeparator = " ")]
    public string? PodRunningTimeout { get; set; }

    [BooleanCommandSwitch("quiet")]
    public bool? Quiet { get; set; }

    [BooleanCommandSwitch("stdin")]
    public bool? Stdin { get; set; }

    [BooleanCommandSwitch("tty")]
    public bool? Tty { get; set; }

}
