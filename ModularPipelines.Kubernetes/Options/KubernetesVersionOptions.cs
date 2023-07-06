using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CommandPrecedingArguments("version")]
public record KubernetesVersionOptions : KubernetesOptions
{
    [BooleanCommandSwitch("client")]
    public bool? Client { get; set; }

    [CommandLongSwitch("output", SwitchValueSeparator = " ")]
    public string? Output { get; set; }

    [BooleanCommandSwitch("short")]
    public bool? Short { get; set; }

}
