using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CommandPrecedingArguments("top", "node")]
[ExcludeFromCodeCoverage]
public record KubernetesTopNodeOptions([property: PositionalArgument] string Name) : KubernetesOptions
{
    [BooleanCommandSwitch("--no-headers")]
    public bool? NoHeaders { get; set; }

    [CommandEqualsSeparatorSwitch("--selector", SwitchValueSeparator = " ")]
    public string? Selector { get; set; }

    [CommandEqualsSeparatorSwitch("--sort-by", SwitchValueSeparator = " ")]
    public string? SortBy { get; set; }

    [BooleanCommandSwitch("--use-protocol-buffers")]
    public bool? UseProtocolBuffers { get; set; }
}
