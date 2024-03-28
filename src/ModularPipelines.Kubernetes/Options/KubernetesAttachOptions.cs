using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[ExcludeFromCodeCoverage]
public record KubernetesAttachOptions : KubernetesOptions
{
    [CommandSwitch("--container")]
    public string? Container { get; set; }

    [PositionalArgument(PlaceholderName = "<Pod>")]
    public string? Pod { get; set; }

    [CommandSwitch("--pod-running-timeout")]
    public string? PodRunningTimeout { get; set; }

    [BooleanCommandSwitch("--quiet")]
    public bool? Quiet { get; set; }

    [BooleanCommandSwitch("--stdin")]
    public bool? Stdin { get; set; }

    [BooleanCommandSwitch("--tty")]
    public bool? Tty { get; set; }

    [PositionalArgument(PlaceholderName = "<TypeName>")]
    public string? TypeName { get; set; }
}
