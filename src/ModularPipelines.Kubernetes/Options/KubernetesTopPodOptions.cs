using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[ExcludeFromCodeCoverage]
public record KubernetesTopPodOptions : KubernetesOptions
{
    public KubernetesTopPodOptions()
    {
        CommandParts = ["top", "pod"];
    }

    [BooleanCommandSwitch("--all-namespaces")]
    public bool? AllNamespaces { get; set; }

    [BooleanCommandSwitch("--containers")]
    public bool? Containers { get; set; }

    [CommandSwitch("--field-selector")]
    public string? FieldSelector { get; set; }

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
