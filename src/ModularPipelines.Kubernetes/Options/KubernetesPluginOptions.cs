using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CommandPrecedingArguments("plugin")]
[ExcludeFromCodeCoverage]
public record KubernetesPluginOptions : KubernetesOptions
{
    [BooleanCommandSwitch("--name-only")]
    public bool? NameOnly { get; set; }
}
