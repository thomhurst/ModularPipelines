using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[ExcludeFromCodeCoverage]
public record KubernetesApplyViewLastAppliedOptions : KubernetesOptions
{
    public KubernetesApplyViewLastAppliedOptions(
        IEnumerable<string> filename
)
    {
        CommandParts = ["apply", "view-last-applied"];
        Filename = filename;
    }

    [BooleanCommandSwitch("--all")]
    public bool? All { get; set; }

    [CommandSwitch("--filename")]
    public IEnumerable<string>? Filename { get; set; }

    [CommandSwitch("--kustomize")]
    public string? Kustomize { get; set; }

    [PositionalArgument(PlaceholderName = "<NameLLabel>")]
    public string? NameLLabel { get; set; }

    [CommandSwitch("--output")]
    public string? Output { get; set; }

    [BooleanCommandSwitch("--recursive")]
    public bool? Recursive { get; set; }

    [CommandSwitch("--selector")]
    public string? Selector { get; set; }

    [PositionalArgument(PlaceholderName = "<Type>")]
    public string? Type { get; set; }

    [PositionalArgument(PlaceholderName = "<TypeName>")]
    public string? TypeName { get; set; }
}
