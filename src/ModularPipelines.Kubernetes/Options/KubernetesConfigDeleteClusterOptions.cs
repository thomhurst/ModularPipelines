using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CommandPrecedingArguments("config", "delete-cluster")]
[ExcludeFromCodeCoverage]
public record KubernetesConfigDeleteClusterOptions([property: PositionalArgument] string Name) : KubernetesOptions
{
    [BooleanCommandSwitch("--no-headers")]
    public virtual bool? NoHeaders { get; set; }

    [CommandEqualsSeparatorSwitch("--output", SwitchValueSeparator = " ")]
    public string? Output { get; set; }
}