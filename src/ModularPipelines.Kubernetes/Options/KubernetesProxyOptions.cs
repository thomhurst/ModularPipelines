using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[ExcludeFromCodeCoverage]
public record KubernetesProxyOptions : KubernetesOptions
{
    public KubernetesProxyOptions()
    {
        CommandParts = ["proxy"];
    }

    [CommandSwitch("--accept-hosts")]
    public string? AcceptHosts { get; set; }

    [CommandSwitch("--accept-paths")]
    public string? AcceptPaths { get; set; }

    [CommandSwitch("--address")]
    public string? Address { get; set; }

    [CommandSwitch("--api-prefix")]
    public string? ApiPrefix { get; set; }

    [BooleanCommandSwitch("--disable-filter")]
    public bool? DisableFilter { get; set; }

    [CommandSwitch("--keepalive")]
    public string? Keepalive { get; set; }

    [CommandSwitch("--port")]
    public string? Port { get; set; }

    [CommandSwitch("--reject-methods")]
    public string? RejectMethods { get; set; }

    [CommandSwitch("--reject-paths")]
    public string? RejectPaths { get; set; }

    [CommandSwitch("--unix-socket")]
    public string? UnixSocket { get; set; }

    [CommandSwitch("--www")]
    public string? Www { get; set; }

    [CommandSwitch("--www-prefix")]
    public string? WwwPrefix { get; set; }
}
