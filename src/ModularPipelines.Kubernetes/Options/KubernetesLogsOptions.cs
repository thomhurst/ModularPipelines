using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[ExcludeFromCodeCoverage]
public record KubernetesLogsOptions : KubernetesOptions
{
    [BooleanCommandSwitch("--all-containers")]
    public bool? AllContainers { get; set; }

    [CommandSwitch("--container")]
    public string? Container { get; set; }

    [BooleanCommandSwitch("--follow")]
    public bool? Follow { get; set; }

    [BooleanCommandSwitch("--ignore-errors")]
    public bool? IgnoreErrors { get; set; }

    [BooleanCommandSwitch("--insecure-skip-tls-verify-backend")]
    public bool? InsecureSkipTlsVerifyBackend { get; set; }

    [CommandSwitch("--limit-bytes")]
    public int? LimitBytes { get; set; }

    [CommandSwitch("--max-log-requests")]
    public string? MaxLogRequests { get; set; }

    [PositionalArgument(PlaceholderName = "<Pod>")]
    public string? Pod { get; set; }

    [CommandSwitch("--pod-running-timeout")]
    public string? PodRunningTimeout { get; set; }

    [BooleanCommandSwitch("--prefix")]
    public bool? Prefix { get; set; }

    [BooleanCommandSwitch("--previous")]
    public bool? Previous { get; set; }

    [CommandSwitch("--selector")]
    public string? Selector { get; set; }

    [CommandSwitch("--since")]
    public string? Since { get; set; }

    [CommandSwitch("--since-time")]
    public string? SinceTime { get; set; }

    [CommandSwitch("--tail")]
    public int? Tail { get; set; }

    [BooleanCommandSwitch("--timestamps")]
    public bool? Timestamps { get; set; }

    [PositionalArgument(PlaceholderName = "<TypeName>")]
    public string? TypeName { get; set; }
}
