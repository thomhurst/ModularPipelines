using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CommandPrecedingArguments("completion")]
public record KubernetesCompletionOptions([property: PositionalArgument] string Shell) : KubernetesOptions
{
    [BooleanCommandSwitch("no-headers")]
    public bool? NoHeaders { get; set; }

    [CommandLongSwitch("output", SwitchValueSeparator = " ")]
    public string? Output { get; set; }

}
