using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CommandPrecedingArguments("auth", "can-i")]
public record KubernetesAuthCanIOptions([property: PositionalArgument] string Verb) : KubernetesOptions
{
    [BooleanCommandSwitch("all-namespaces")]
    public bool? AllNamespaces { get; set; }

    [BooleanCommandSwitch("list")]
    public bool? List { get; set; }

    [BooleanCommandSwitch("no-headers")]
    public bool? NoHeaders { get; set; }

    [BooleanCommandSwitch("quiet")]
    public bool? Quiet { get; set; }

    [CommandLongSwitch("subresource", SwitchValueSeparator = " ")]
    public string? Subresource { get; set; }

}
