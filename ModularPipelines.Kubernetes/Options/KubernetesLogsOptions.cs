using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CommandPrecedingArguments("logs")]
public record KubernetesLogsOptions(string Name) : KubernetesOptions
{
    [BooleanCommandSwitch("all-containers")]
    public bool? AllContainers { get; set; }

    [CommandLongSwitch("container", SwitchValueSeparator = " ")]
    public string? Container { get; set; }

    [BooleanCommandSwitch("follow")]
    public bool? Follow { get; set; }

    [BooleanCommandSwitch("ignore-errors")]
    public bool? IgnoreErrors { get; set; }

    [BooleanCommandSwitch("insecure-skip-tls-verify-backend")]
    public bool? InsecureSkipTlsVerifyBackend { get; set; }

    [CommandLongSwitch("limit-bytes", SwitchValueSeparator = " ")]
    public int? LimitBytes { get; set; }

    [CommandLongSwitch("max-log-requests", SwitchValueSeparator = " ")]
    public int? MaxLogRequests { get; set; }

    [CommandLongSwitch("pod-running-timeout", SwitchValueSeparator = " ")]
    public string? PodRunningTimeout { get; set; }

    [BooleanCommandSwitch("prefix")]
    public bool? Prefix { get; set; }

    [BooleanCommandSwitch("previous")]
    public bool? Previous { get; set; }

    [CommandLongSwitch("selector", SwitchValueSeparator = " ")]
    public string? Selector { get; set; }

    [CommandLongSwitch("since", SwitchValueSeparator = " ")]
    public string? Since { get; set; }

    [CommandLongSwitch("since-time", SwitchValueSeparator = " ")]
    public string? SinceTime { get; set; }

    [CommandLongSwitch("tail", SwitchValueSeparator = " ")]
    public int? Tail { get; set; }

    [BooleanCommandSwitch("timestamps")]
    public bool? Timestamps { get; set; }

}
