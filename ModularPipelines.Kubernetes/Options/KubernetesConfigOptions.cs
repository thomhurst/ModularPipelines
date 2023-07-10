using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CommandPrecedingArguments("config")]
public record KubernetesConfigOptions([property: PositionalArgument] string Subcommand) : KubernetesOptions
{
    [BooleanCommandSwitch("no-headers")]
    public bool? NoHeaders { get; set; }

    [CommandLongSwitch("output", SwitchValueSeparator = " ")]
    public string? Output { get; set; }

}
