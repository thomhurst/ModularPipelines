using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CommandPrecedingArguments("port-forward")]
[ExcludeFromCodeCoverage]
public record KubernetesPortForwardOptions([property: PositionalArgument] string Name) : KubernetesOptions
{
    [CommandEqualsSeparatorSwitch("--address", SwitchValueSeparator = " ")]
    public string? Address { get; set; }

    [CommandEqualsSeparatorSwitch("--pod-running-timeout", SwitchValueSeparator = " ")]
    public string? PodRunningTimeout { get; set; }
}
