using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CliSubCommand("exec")]
[ExcludeFromCodeCoverage]
public record KubernetesExecOptions([property: CliArgument] string Name) : KubernetesOptions
{
    [CliOption("--container")]
    public virtual string? Container { get; set; }

    [CliOption("--filename")]
    public virtual string[]? Filename { get; set; }

    [CliOption("--pod-running-timeout")]
    public virtual string? PodRunningTimeout { get; set; }

    [CliFlag("--quiet")]
    public virtual bool? Quiet { get; set; }

    [CliFlag("--stdin")]
    public virtual bool? Stdin { get; set; }

    [CliFlag("--tty")]
    public virtual bool? Tty { get; set; }
}