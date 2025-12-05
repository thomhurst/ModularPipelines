using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CliCommand("swarm", "update")]
[ExcludeFromCodeCoverage]
public record DockerSwarmUpdateOptions : DockerOptions
{
    [CliOption("--autolock")]
    public virtual string? Autolock { get; set; }

    [CliOption("--cert-expiry")]
    public virtual string? CertExpiry { get; set; }

    [CliOption("--dispatcher-heartbeat")]
    public virtual string? DispatcherHeartbeat { get; set; }

    [CliOption("--external-ca")]
    public virtual string? ExternalCa { get; set; }

    [CliOption("--max-snapshots")]
    public virtual string? MaxSnapshots { get; set; }

    [CliOption("--snapshot-interval")]
    public virtual int? SnapshotInterval { get; set; }

    [CliOption("--task-history-limit")]
    public virtual int? TaskHistoryLimit { get; set; }
}
