using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[ExcludeFromCodeCoverage]
public record KubernetesDescribeOptions : KubernetesOptions
{
    public KubernetesDescribeOptions(
        IEnumerable<string> filename
)
    {
        CommandParts = ["describe"];
        Filename = filename;
    }

    [BooleanCommandSwitch("--all-namespaces")]
    public bool? AllNamespaces { get; set; }

    [CommandSwitch("--chunk-size")]
    public string? ChunkSize { get; set; }

    [CommandSwitch("--filename")]
    public IEnumerable<string>? Filename { get; set; }

    [CommandSwitch("--kustomize")]
    public string? Kustomize { get; set; }

    [PositionalArgument(PlaceholderName = "<NamePrefixLLabel>")]
    public string? NamePrefixLLabel { get; set; }

    [BooleanCommandSwitch("--recursive")]
    public bool? Recursive { get; set; }

    [CommandSwitch("--selector")]
    public string? Selector { get; set; }

    [BooleanCommandSwitch("--show-events")]
    public bool? ShowEvents { get; set; }

    [PositionalArgument(PlaceholderName = "<Type>")]
    public string? Type { get; set; }

    [PositionalArgument(PlaceholderName = "<TypeName>")]
    public string? TypeName { get; set; }
}
