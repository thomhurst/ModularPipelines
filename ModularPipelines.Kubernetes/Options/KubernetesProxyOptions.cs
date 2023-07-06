using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CommandPrecedingArguments("proxy")]
public record KubernetesProxyOptions : KubernetesOptions
{
    [CommandLongSwitch("accept-hosts", SwitchValueSeparator = " ")]
    public string? AcceptHosts { get; set; }

    [CommandLongSwitch("accept-paths", SwitchValueSeparator = " ")]
    public string? AcceptPaths { get; set; }

    [CommandLongSwitch("address", SwitchValueSeparator = " ")]
    public string? Address { get; set; }

    [CommandLongSwitch("api-prefix", SwitchValueSeparator = " ")]
    public string? ApiPrefix { get; set; }

    [BooleanCommandSwitch("disable-filter")]
    public bool? DisableFilter { get; set; }

    [CommandLongSwitch("keepalive", SwitchValueSeparator = " ")]
    public string? Keepalive { get; set; }

    [CommandLongSwitch("port", SwitchValueSeparator = " ")]
    public int? Port { get; set; }

    [CommandLongSwitch("reject-methods", SwitchValueSeparator = " ")]
    public string? RejectMethods { get; set; }

    [CommandLongSwitch("reject-paths", SwitchValueSeparator = " ")]
    public string? RejectPaths { get; set; }

    [CommandLongSwitch("unix-socket", SwitchValueSeparator = " ")]
    public string? UnixSocket { get; set; }

    [CommandLongSwitch("www", SwitchValueSeparator = " ")]
    public string? Www { get; set; }

    [CommandLongSwitch("www-prefix", SwitchValueSeparator = " ")]
    public string? WwwPrefix { get; set; }

}
