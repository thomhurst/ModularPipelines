using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CommandPrecedingArguments("exec")]
[ExcludeFromCodeCoverage]
public record KubernetesExecOptions([property: PositionalArgument] string Name) : KubernetesOptions
{
    [CommandEqualsSeparatorSwitch("--container", SwitchValueSeparator = " ")]
    public string? Container { get; set; }

    [CommandEqualsSeparatorSwitch("--filename", SwitchValueSeparator = " ")]
    public string[]? Filename { get; set; }

    [CommandEqualsSeparatorSwitch("--pod-running-timeout", SwitchValueSeparator = " ")]
    public string? PodRunningTimeout { get; set; }

    [BooleanCommandSwitch("--quiet")]
    public virtual bool? Quiet { get; set; }

    [BooleanCommandSwitch("--stdin")]
    public virtual bool? Stdin { get; set; }

    [BooleanCommandSwitch("--tty")]
    public virtual bool? Tty { get; set; }
}