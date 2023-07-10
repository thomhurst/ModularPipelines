using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CommandPrecedingArguments("config", "delete-context")]
public record KubernetesConfigDeleteContextOptions([property: PositionalArgument] string Name) : KubernetesOptions
{
    [BooleanCommandSwitch("no-headers")]
    public bool? NoHeaders { get; set; }

    [CommandLongSwitch("output", SwitchValueSeparator = " ")]
    public string? Output { get; set; }

}
