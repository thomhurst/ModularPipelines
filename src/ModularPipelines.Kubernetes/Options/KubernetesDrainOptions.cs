using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CliCommand("drain")]
[ExcludeFromCodeCoverage]
public record KubernetesDrainOptions([property: CliArgument] string Node) : KubernetesOptions
{
    [CliOption("--chunk-size")]
    public int? ChunkSize { get; set; }

    [CliFlag("--delete-emptydir-data")]
    public virtual bool? DeleteEmptydirData { get; set; }

    [CliFlag("--delete-local-data")]
    public virtual bool? DeleteLocalData { get; set; }

    [CliFlag("--disable-eviction")]
    public virtual bool? DisableEviction { get; set; }

    [CliOption("--dry-run")]
    public string? DryRun { get; set; }

    [CliFlag("--force")]
    public virtual bool? Force { get; set; }

    [CliOption("--grace-period")]
    public int? GracePeriod { get; set; }

    [CliFlag("--ignore-daemonsets")]
    public virtual bool? IgnoreDaemonsets { get; set; }

    [CliFlag("--ignore-errors")]
    public virtual bool? IgnoreErrors { get; set; }

    [CliOption("--pod-selector")]
    public string? PodSelector { get; set; }

    [CliOption("--selector")]
    public string? Selector { get; set; }

    [CliOption("--skip-wait-for-delete-timeout")]
    public int? SkipWaitForDeleteTimeout { get; set; }

    [CliOption("--timeout")]
    public string? Timeout { get; set; }
}