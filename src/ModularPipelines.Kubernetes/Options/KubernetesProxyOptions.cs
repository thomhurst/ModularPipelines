using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CliCommand("proxy")]
[ExcludeFromCodeCoverage]
public record KubernetesProxyOptions : KubernetesOptions
{
    [CliOption("--accept-hosts")]
    public virtual string? AcceptHosts { get; set; }

    [CliOption("--accept-paths")]
    public virtual string? AcceptPaths { get; set; }

    [CliOption("--address")]
    public virtual string? Address { get; set; }

    [CliOption("--api-prefix")]
    public virtual string? ApiPrefix { get; set; }

    [CliFlag("--disable-filter")]
    public virtual bool? DisableFilter { get; set; }

    [CliOption("--keepalive")]
    public virtual string? Keepalive { get; set; }

    [CliOption("--port")]
    public virtual int? Port { get; set; }

    [CliOption("--reject-methods")]
    public virtual string? RejectMethods { get; set; }

    [CliOption("--reject-paths")]
    public virtual string? RejectPaths { get; set; }

    [CliOption("--unix-socket")]
    public virtual string? UnixSocket { get; set; }

    [CliOption("--www")]
    public virtual string? Www { get; set; }

    [CliOption("--www-prefix")]
    public virtual string? WwwPrefix { get; set; }
}