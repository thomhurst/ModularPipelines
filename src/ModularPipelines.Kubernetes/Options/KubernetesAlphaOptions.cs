using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CommandPrecedingArguments("alpha")]
[ExcludeFromCodeCoverage]
public record KubernetesAlphaOptions : KubernetesOptions
{
    [CommandEqualsSeparatorSwitch("--api-group", SwitchValueSeparator = " ")]
    public string? ApiGroup { get; set; }

    [BooleanCommandSwitch("--cached")]
    public virtual bool? Cached { get; set; }

    [BooleanCommandSwitch("--namespaced")]
    public virtual bool? Namespaced { get; set; }

    [BooleanCommandSwitch("--no-headers")]
    public virtual bool? NoHeaders { get; set; }

    [CommandEqualsSeparatorSwitch("--output", SwitchValueSeparator = " ")]
    public string? Output { get; set; }

    [CommandEqualsSeparatorSwitch("--sort-by", SwitchValueSeparator = " ")]
    public string? SortBy { get; set; }

    [CommandEqualsSeparatorSwitch("--verbs", SwitchValueSeparator = " ")]
    public string[]? Verbs { get; set; }
}