using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CommandPrecedingArguments("top", "pod")]
[ExcludeFromCodeCoverage]
public record KubernetesTopPodOptions([property: PositionalArgument] string Name) : KubernetesOptions
{
    [BooleanCommandSwitch("--all-namespaces")]
    public virtual bool? AllNamespaces { get; set; }

    [BooleanCommandSwitch("--containers")]
    public virtual bool? Containers { get; set; }

    [CommandEqualsSeparatorSwitch("--field-selector", SwitchValueSeparator = " ")]
    public string? FieldSelector { get; set; }

    [BooleanCommandSwitch("--no-headers")]
    public virtual bool? NoHeaders { get; set; }

    [CommandEqualsSeparatorSwitch("--selector", SwitchValueSeparator = " ")]
    public string? Selector { get; set; }

    [CommandEqualsSeparatorSwitch("--sort-by", SwitchValueSeparator = " ")]
    public string? SortBy { get; set; }

    [BooleanCommandSwitch("--use-protocol-buffers")]
    public virtual bool? UseProtocolBuffers { get; set; }
}