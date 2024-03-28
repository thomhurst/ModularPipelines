using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[ExcludeFromCodeCoverage]
public record KubernetesAuthCanIOptions : KubernetesOptions
{
    public KubernetesAuthCanIOptions()
    {
        CommandParts = ["auth", "can-i"];
    }

    [BooleanCommandSwitch("--all-namespaces")]
    public bool? AllNamespaces { get; set; }

    [BooleanCommandSwitch("--list")]
    public bool? List { get; set; }

    [BooleanCommandSwitch("--no-headers")]
    public bool? NoHeaders { get; set; }

    [BooleanCommandSwitch("--quiet")]
    public bool? Quiet { get; set; }

    [CommandSwitch("--subresource")]
    public string? Subresource { get; set; }

    [PositionalArgument(PlaceholderName = "<TypeTypeNameNonresourceurl>")]
    public string? TypeTypeNameNonresourceurl { get; set; }

    [PositionalArgument(PlaceholderName = "<VERB>")]
    public string? VERB { get; set; }
}
