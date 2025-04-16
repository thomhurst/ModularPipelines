using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CommandPrecedingArguments("drain")]
[ExcludeFromCodeCoverage]
public record KubernetesDrainOptions([property: PositionalArgument] string Node) : KubernetesOptions
{
    [CommandEqualsSeparatorSwitch("--chunk-size", SwitchValueSeparator = " ")]
    public int? ChunkSize { get; set; }

    [BooleanCommandSwitch("--delete-emptydir-data")]
    public virtual bool? DeleteEmptydirData { get; set; }

    [BooleanCommandSwitch("--delete-local-data")]
    public virtual bool? DeleteLocalData { get; set; }

    [BooleanCommandSwitch("--disable-eviction")]
    public virtual bool? DisableEviction { get; set; }

    [CommandEqualsSeparatorSwitch("--dry-run", SwitchValueSeparator = " ")]
    public string? DryRun { get; set; }

    [BooleanCommandSwitch("--force")]
    public virtual bool? Force { get; set; }

    [CommandEqualsSeparatorSwitch("--grace-period", SwitchValueSeparator = " ")]
    public int? GracePeriod { get; set; }

    [BooleanCommandSwitch("--ignore-daemonsets")]
    public virtual bool? IgnoreDaemonsets { get; set; }

    [BooleanCommandSwitch("--ignore-errors")]
    public virtual bool? IgnoreErrors { get; set; }

    [CommandEqualsSeparatorSwitch("--pod-selector", SwitchValueSeparator = " ")]
    public string? PodSelector { get; set; }

    [CommandEqualsSeparatorSwitch("--selector", SwitchValueSeparator = " ")]
    public string? Selector { get; set; }

    [CommandEqualsSeparatorSwitch("--skip-wait-for-delete-timeout", SwitchValueSeparator = " ")]
    public int? SkipWaitForDeleteTimeout { get; set; }

    [CommandEqualsSeparatorSwitch("--timeout", SwitchValueSeparator = " ")]
    public string? Timeout { get; set; }
}