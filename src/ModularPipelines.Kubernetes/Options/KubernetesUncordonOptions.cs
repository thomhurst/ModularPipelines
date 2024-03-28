using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[ExcludeFromCodeCoverage]
public record KubernetesUncordonOptions : KubernetesOptions
{
    public KubernetesUncordonOptions()
    {
        CommandParts = ["uncordon"];
    }

    [CommandSwitch("--dry-run")]
    public string? DryRun { get; set; }

    [PositionalArgument(PlaceholderName = "<NODE>")]
    public string? NODE { get; set; }

    [CommandSwitch("--selector")]
    public string? Selector { get; set; }
}
