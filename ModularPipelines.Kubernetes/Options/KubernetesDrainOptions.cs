using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CommandPrecedingArguments("drain")]
public record KubernetesDrainOptions([property: PositionalArgument] string Node) : KubernetesOptions
{
    [CommandLongSwitch("chunk-size", SwitchValueSeparator = " ")]
    public int? ChunkSize { get; set; }

    [BooleanCommandSwitch("delete-emptydir-data")]
    public bool? DeleteEmptydirData { get; set; }

    [BooleanCommandSwitch("delete-local-data")]
    public bool? DeleteLocalData { get; set; }

    [BooleanCommandSwitch("disable-eviction")]
    public bool? DisableEviction { get; set; }

    [CommandLongSwitch("dry-run", SwitchValueSeparator = " ")]
    public string? DryRun { get; set; }

    [BooleanCommandSwitch("force")]
    public bool? Force { get; set; }

    [CommandLongSwitch("grace-period", SwitchValueSeparator = " ")]
    public int? GracePeriod { get; set; }

    [BooleanCommandSwitch("ignore-daemonsets")]
    public bool? IgnoreDaemonsets { get; set; }

    [BooleanCommandSwitch("ignore-errors")]
    public bool? IgnoreErrors { get; set; }

    [CommandLongSwitch("pod-selector", SwitchValueSeparator = " ")]
    public string? PodSelector { get; set; }

    [CommandLongSwitch("selector", SwitchValueSeparator = " ")]
    public string? Selector { get; set; }

    [CommandLongSwitch("skip-wait-for-delete-timeout", SwitchValueSeparator = " ")]
    public int? SkipWaitForDeleteTimeout { get; set; }

    [CommandLongSwitch("timeout", SwitchValueSeparator = " ")]
    public string? Timeout { get; set; }

}
