using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CommandPrecedingArguments("completion")]
[ExcludeFromCodeCoverage]
public record KubernetesCompletionOptions([property: PositionalArgument] string Shell) : KubernetesOptions
{
    [BooleanCommandSwitch("--no-headers")]
    public bool? NoHeaders { get; set; }

    [CommandEqualsSeparatorSwitch("--output", SwitchValueSeparator = " ")]
    public string? Output { get; set; }
}