using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CommandPrecedingArguments("drain")]
public record KubernetesDrainOptions([property: PositionalArgument] string Node) : KubernetesOptions
{
    [CommandEqualsSeparatorSwitch("--chunk-size", SwitchValueSeparator = " ")]
    public int? ChunkSize { get; set; }

    [BooleanCommandSwitch("--delete-emptydir-data")]
    public bool? DeleteEmptydirData { get; set; }

    [BooleanCommandSwitch("--delete-local-data")]
    public bool? DeleteLocalData { get; set; }

    [BooleanCommandSwitch("--disable-eviction")]
    public bool? DisableEviction { get; set; }

    [CommandEqualsSeparatorSwitch("--dry-run", SwitchValueSeparator = " ")]
    public string? DryRun { get; set; }

    [BooleanCommandSwitch("--force")]
    public bool? Force { get; set; }

    [CommandEqualsSeparatorSwitch("--grace-period", SwitchValueSeparator = " ")]
    public int? GracePeriod { get; set; }

    [BooleanCommandSwitch("--ignore-daemonsets")]
    public bool? IgnoreDaemonsets { get; set; }

    [BooleanCommandSwitch("--ignore-errors")]
    public bool? IgnoreErrors { get; set; }

    [CommandEqualsSeparatorSwitch("--pod-selector", SwitchValueSeparator = " ")]
    public string? PodSelector { get; set; }

    [CommandEqualsSeparatorSwitch("--selector", SwitchValueSeparator = " ")]
    public string? Selector { get; set; }

    [CommandEqualsSeparatorSwitch("--skip-wait-for-delete-timeout", SwitchValueSeparator = " ")]
    public int? SkipWaitForDeleteTimeout { get; set; }

    [CommandEqualsSeparatorSwitch("--timeout", SwitchValueSeparator = " ")]
    public string? Timeout { get; set; }

}
