using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CommandPrecedingArguments("api-resources")]
[ExcludeFromCodeCoverage]
public record KubernetesApiResourcesOptions : KubernetesOptions
{
    [CommandEqualsSeparatorSwitch("--api-group", SwitchValueSeparator = " ")]
    public string? ApiGroup { get; set; }

    [BooleanCommandSwitch("--cached")]
    public bool? Cached { get; set; }

    [BooleanCommandSwitch("--namespaced")]
    public bool? Namespaced { get; set; }

    [BooleanCommandSwitch("--no-headers")]
    public bool? NoHeaders { get; set; }

    [CommandEqualsSeparatorSwitch("--output", SwitchValueSeparator = " ")]
    public string? Output { get; set; }

    [CommandEqualsSeparatorSwitch("--sort-by", SwitchValueSeparator = " ")]
    public string? SortBy { get; set; }

    [CommandEqualsSeparatorSwitch("--verbs", SwitchValueSeparator = " ")]
    public string[]? Verbs { get; set; }
}