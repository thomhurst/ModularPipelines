using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CommandPrecedingArguments("version")]
[ExcludeFromCodeCoverage]
public record KubernetesVersionOptions : KubernetesOptions
{
    [BooleanCommandSwitch("--client")]
    public virtual bool? Client { get; set; }

    [CommandEqualsSeparatorSwitch("--output", SwitchValueSeparator = " ")]
    public string? Output { get; set; }

    [BooleanCommandSwitch("--short")]
    public virtual bool? Short { get; set; }
}