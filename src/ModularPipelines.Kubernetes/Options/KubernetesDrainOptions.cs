using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[ExcludeFromCodeCoverage]
public record KubernetesDrainOptions : KubernetesOptions
{
    public KubernetesDrainOptions()
    {
        CommandParts = ["drain"];
    }

    [CommandSwitch("--chunk-size")]
    public string? ChunkSize { get; set; }

    [BooleanCommandSwitch("--delete-emptydir-data")]
    public bool? DeleteEmptydirData { get; set; }

    [BooleanCommandSwitch("--delete-local-data")]
    public bool? DeleteLocalData { get; set; }

    [BooleanCommandSwitch("--disable-eviction")]
    public bool? DisableEviction { get; set; }

    [CommandSwitch("--dry-run")]
    public string? DryRun { get; set; }

    [BooleanCommandSwitch("--force")]
    public bool? Force { get; set; }

    [CommandSwitch("--grace-period")]
    public int? GracePeriod { get; set; }

    [BooleanCommandSwitch("--ignore-daemonsets")]
    public bool? IgnoreDaemonsets { get; set; }

    [BooleanCommandSwitch("--ignore-errors")]
    public bool? IgnoreErrors { get; set; }

    [PositionalArgument(PlaceholderName = "<NODE>")]
    public string? NODE { get; set; }

    [CommandSwitch("--pod-selector")]
    public string? PodSelector { get; set; }

    [CommandSwitch("--selector")]
    public string? Selector { get; set; }

    [CommandSwitch("--skip-wait-for-delete-timeout")]
    public int? SkipWaitForDeleteTimeout { get; set; }

    [CommandSwitch("--timeout")]
    public string? Timeout { get; set; }
}
