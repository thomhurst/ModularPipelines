using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CommandPrecedingArguments("proxy")]
[ExcludeFromCodeCoverage]
public record KubernetesProxyOptions : KubernetesOptions
{
    [CommandEqualsSeparatorSwitch("--accept-hosts", SwitchValueSeparator = " ")]
    public string? AcceptHosts { get; set; }

    [CommandEqualsSeparatorSwitch("--accept-paths", SwitchValueSeparator = " ")]
    public string? AcceptPaths { get; set; }

    [CommandEqualsSeparatorSwitch("--address", SwitchValueSeparator = " ")]
    public string? Address { get; set; }

    [CommandEqualsSeparatorSwitch("--api-prefix", SwitchValueSeparator = " ")]
    public string? ApiPrefix { get; set; }

    [BooleanCommandSwitch("--disable-filter")]
    public virtual bool? DisableFilter { get; set; }

    [CommandEqualsSeparatorSwitch("--keepalive", SwitchValueSeparator = " ")]
    public string? Keepalive { get; set; }

    [CommandEqualsSeparatorSwitch("--port", SwitchValueSeparator = " ")]
    public int? Port { get; set; }

    [CommandEqualsSeparatorSwitch("--reject-methods", SwitchValueSeparator = " ")]
    public string? RejectMethods { get; set; }

    [CommandEqualsSeparatorSwitch("--reject-paths", SwitchValueSeparator = " ")]
    public string? RejectPaths { get; set; }

    [CommandEqualsSeparatorSwitch("--unix-socket", SwitchValueSeparator = " ")]
    public string? UnixSocket { get; set; }

    [CommandEqualsSeparatorSwitch("--www", SwitchValueSeparator = " ")]
    public string? Www { get; set; }

    [CommandEqualsSeparatorSwitch("--www-prefix", SwitchValueSeparator = " ")]
    public string? WwwPrefix { get; set; }
}