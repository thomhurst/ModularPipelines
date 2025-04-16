using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CommandPrecedingArguments("logs")]
[ExcludeFromCodeCoverage]
public record KubernetesLogsOptions([property: PositionalArgument] string Name) : KubernetesOptions
{
    [BooleanCommandSwitch("--all-containers")]
    public virtual bool? AllContainers { get; set; }

    [CommandEqualsSeparatorSwitch("--container", SwitchValueSeparator = " ")]
    public string? Container { get; set; }

    [BooleanCommandSwitch("--follow")]
    public virtual bool? Follow { get; set; }

    [BooleanCommandSwitch("--ignore-errors")]
    public virtual bool? IgnoreErrors { get; set; }

    [BooleanCommandSwitch("--insecure-skip-tls-verify-backend")]
    public virtual bool? InsecureSkipTlsVerifyBackend { get; set; }

    [CommandEqualsSeparatorSwitch("--limit-bytes", SwitchValueSeparator = " ")]
    public int? LimitBytes { get; set; }

    [CommandEqualsSeparatorSwitch("--max-log-requests", SwitchValueSeparator = " ")]
    public int? MaxLogRequests { get; set; }

    [CommandEqualsSeparatorSwitch("--pod-running-timeout", SwitchValueSeparator = " ")]
    public string? PodRunningTimeout { get; set; }

    [BooleanCommandSwitch("--prefix")]
    public virtual bool? Prefix { get; set; }

    [BooleanCommandSwitch("--previous")]
    public virtual bool? Previous { get; set; }

    [CommandEqualsSeparatorSwitch("--selector", SwitchValueSeparator = " ")]
    public string? Selector { get; set; }

    [CommandEqualsSeparatorSwitch("--since", SwitchValueSeparator = " ")]
    public string? Since { get; set; }

    [CommandEqualsSeparatorSwitch("--since-time", SwitchValueSeparator = " ")]
    public string? SinceTime { get; set; }

    [CommandEqualsSeparatorSwitch("--tail", SwitchValueSeparator = " ")]
    public int? Tail { get; set; }

    [BooleanCommandSwitch("--timestamps")]
    public virtual bool? Timestamps { get; set; }
}