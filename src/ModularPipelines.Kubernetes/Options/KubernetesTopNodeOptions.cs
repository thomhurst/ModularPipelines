using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[ExcludeFromCodeCoverage]
public record KubernetesTopNodeOptions : KubernetesOptions
{
    public KubernetesTopNodeOptions()
    {
        CommandParts = ["top", "node"];
    }

    [PositionalArgument(PlaceholderName = "<NameLLabel>")]
    public string? NameLLabel { get; set; }

    [BooleanCommandSwitch("--no-headers")]
    public bool? NoHeaders { get; set; }

    [CommandSwitch("--selector")]
    public string? Selector { get; set; }

    [CommandSwitch("--sort-by")]
    public string? SortBy { get; set; }

    [BooleanCommandSwitch("--use-protocol-buffers")]
    public bool? UseProtocolBuffers { get; set; }
}
