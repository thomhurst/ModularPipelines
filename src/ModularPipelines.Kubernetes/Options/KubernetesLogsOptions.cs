using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CliCommand("logs")]
[ExcludeFromCodeCoverage]
public record KubernetesLogsOptions([property: CliArgument] string Name) : KubernetesOptions
{
    [CliFlag("--all-containers")]
    public virtual bool? AllContainers { get; set; }

    [CliOption("--container")]
    public virtual string? Container { get; set; }

    [CliFlag("--follow")]
    public virtual bool? Follow { get; set; }

    [CliFlag("--ignore-errors")]
    public virtual bool? IgnoreErrors { get; set; }

    [CliFlag("--insecure-skip-tls-verify-backend")]
    public virtual bool? InsecureSkipTlsVerifyBackend { get; set; }

    [CliOption("--limit-bytes")]
    public virtual int? LimitBytes { get; set; }

    [CliOption("--max-log-requests")]
    public virtual int? MaxLogRequests { get; set; }

    [CliOption("--pod-running-timeout")]
    public virtual string? PodRunningTimeout { get; set; }

    [CliFlag("--prefix")]
    public virtual bool? Prefix { get; set; }

    [CliFlag("--previous")]
    public virtual bool? Previous { get; set; }

    [CliOption("--selector")]
    public virtual string? Selector { get; set; }

    [CliOption("--since")]
    public virtual string? Since { get; set; }

    [CliOption("--since-time")]
    public virtual string? SinceTime { get; set; }

    [CliOption("--tail")]
    public virtual int? Tail { get; set; }

    [CliFlag("--timestamps")]
    public virtual bool? Timestamps { get; set; }
}