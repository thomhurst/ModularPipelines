using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CliCommand("proxy")]
[ExcludeFromCodeCoverage]
public record KubernetesProxyOptions : KubernetesOptions
{
    [CliOption("--accept-hosts")]
    public string? AcceptHosts { get; set; }

    [CliOption("--accept-paths")]
    public string? AcceptPaths { get; set; }

    [CliOption("--address")]
    public string? Address { get; set; }

    [CliOption("--api-prefix")]
    public string? ApiPrefix { get; set; }

    [CliFlag("--disable-filter")]
    public virtual bool? DisableFilter { get; set; }

    [CliOption("--keepalive")]
    public string? Keepalive { get; set; }

    [CliOption("--port")]
    public int? Port { get; set; }

    [CliOption("--reject-methods")]
    public string? RejectMethods { get; set; }

    [CliOption("--reject-paths")]
    public string? RejectPaths { get; set; }

    [CliOption("--unix-socket")]
    public string? UnixSocket { get; set; }

    [CliOption("--www")]
    public string? Www { get; set; }

    [CliOption("--www-prefix")]
    public string? WwwPrefix { get; set; }
}