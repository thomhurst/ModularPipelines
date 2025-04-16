using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[ExcludeFromCodeCoverage]
public record DockerServiceCreateOptions : DockerOptions
{
    public DockerServiceCreateOptions(
        string image
    )
    {
        CommandParts = ["service", "create"];

        Image = image;
    }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? Image { get; set; }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? Command { get; set; }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public IEnumerable<string>? Arg { get; set; }

    [CommandSwitch("--cap-add")]
    public virtual string? CapAdd { get; set; }

    [CommandSwitch("--cap-drop")]
    public virtual string? CapDrop { get; set; }

    [CommandSwitch("--constraint")]
    public virtual string? Constraint { get; set; }

    [CommandSwitch("--container-label")]
    public virtual string? ContainerLabel { get; set; }

    [CommandSwitch("--credential-spec")]
    public virtual string? CredentialSpec { get; set; }

    [BooleanCommandSwitch("--detach")]
    public virtual bool? Detach { get; set; }

    [CommandSwitch("--dns")]
    public virtual string? Dns { get; set; }

    [CommandSwitch("--dns-option")]
    public virtual string? DnsOption { get; set; }

    [CommandSwitch("--dns-search")]
    public virtual string? DnsSearch { get; set; }

    [CommandSwitch("--endpoint-mode")]
    public virtual string? EndpointMode { get; set; }

    [CommandSwitch("--entrypoint")]
    public virtual string? Entrypoint { get; set; }

    [CommandSwitch("--env")]
    public virtual string? Env { get; set; }

    [CommandSwitch("--env-file")]
    public virtual string? EnvFile { get; set; }

    [CommandSwitch("--generic-resource")]
    public virtual string? GenericResource { get; set; }

    [CommandSwitch("--group")]
    public virtual string? Group { get; set; }

    [CommandSwitch("--health-cmd")]
    public virtual string? HealthCmd { get; set; }

    [CommandSwitch("--health-interval")]
    public virtual string? HealthInterval { get; set; }

    [CommandSwitch("--health-retries")]
    public virtual string? HealthRetries { get; set; }

    [CommandSwitch("--health-start-interval")]
    public virtual string? HealthStartInterval { get; set; }

    [CommandSwitch("--health-start-period")]
    public virtual string? HealthStartPeriod { get; set; }

    [CommandSwitch("--health-timeout")]
    public virtual string? HealthTimeout { get; set; }

    [CommandSwitch("--hostname")]
    public virtual string? Hostname { get; set; }

    [CommandSwitch("--init")]
    public virtual string? Init { get; set; }

    [BooleanCommandSwitch("--isolation")]
    public virtual bool? Isolation { get; set; }

    [CommandSwitch("--label")]
    public virtual string? Label { get; set; }

    [CommandSwitch("--limit-cpu")]
    public virtual string? LimitCpu { get; set; }

    [CommandSwitch("--limit-memory")]
    public virtual string? LimitMemory { get; set; }

    [CommandSwitch("--limit-pids")]
    public virtual string? LimitPids { get; set; }

    [CommandSwitch("--log-driver")]
    public virtual string? LogDriver { get; set; }

    [CommandSwitch("--log-opt")]
    public virtual string? LogOpt { get; set; }

    [CommandSwitch("--max-concurrent")]
    public virtual string? MaxConcurrent { get; set; }

    [CommandSwitch("--mode")]
    public virtual string? Mode { get; set; }

    [CommandSwitch("--mount")]
    public virtual string? Mount { get; set; }

    [CommandSwitch("--name")]
    public virtual string? Name { get; set; }

    [CommandSwitch("--network")]
    public virtual string? Network { get; set; }

    [BooleanCommandSwitch("--no-healthcheck")]
    public virtual bool? NoHealthcheck { get; set; }

    [CommandSwitch("--no-resolve-image")]
    public virtual string? NoResolveImage { get; set; }

    [CommandSwitch("--placement-pref")]
    public virtual string? PlacementPref { get; set; }

    [CommandSwitch("--publish")]
    public virtual string? Publish { get; set; }

    [BooleanCommandSwitch("--quiet")]
    public virtual bool? Quiet { get; set; }

    [BooleanCommandSwitch("--read-only")]
    public virtual bool? ReadOnly { get; set; }

    [CommandSwitch("--replicas")]
    public virtual string? Replicas { get; set; }

    [CommandSwitch("--replicas-max-per-node")]
    public virtual string? ReplicasMaxPerNode { get; set; }

    [CommandSwitch("--reserve-cpu")]
    public virtual string? ReserveCpu { get; set; }

    [CommandSwitch("--reserve-memory")]
    public virtual string? ReserveMemory { get; set; }

    [CommandSwitch("--restart-condition")]
    public virtual string? RestartCondition { get; set; }

    [CommandSwitch("--restart-delay")]
    public virtual string? RestartDelay { get; set; }

    [CommandSwitch("--restart-max-attempts")]
    public virtual string? RestartMaxAttempts { get; set; }

    [CommandSwitch("--restart-window")]
    public virtual string? RestartWindow { get; set; }

    [CommandSwitch("--rollback-delay")]
    public virtual string? RollbackDelay { get; set; }

    [CommandSwitch("--rollback-failure-action")]
    public virtual string? RollbackFailureAction { get; set; }

    [CommandSwitch("--rollback-max-failure-ratio")]
    public virtual string? RollbackMaxFailureRatio { get; set; }

    [CommandSwitch("--rollback-monitor")]
    public virtual string? RollbackMonitor { get; set; }

    [CommandSwitch("--rollback-order")]
    public virtual string? RollbackOrder { get; set; }

    [CommandSwitch("--rollback-parallelism")]
    public virtual int? RollbackParallelism { get; set; }

    [CommandSwitch("--secret")]
    public virtual string? Secret { get; set; }

    [CommandSwitch("--stop-grace-period")]
    public virtual string? StopGracePeriod { get; set; }

    [CommandSwitch("--stop-signal")]
    public virtual string? StopSignal { get; set; }

    [CommandSwitch("--sysctl")]
    public virtual string? Sysctl { get; set; }

    [CommandSwitch("--tty")]
    public virtual string? Tty { get; set; }

    [CommandSwitch("--ulimit")]
    public virtual string? Ulimit { get; set; }

    [CommandSwitch("--update-delay")]
    public virtual string? UpdateDelay { get; set; }

    [CommandSwitch("--update-failure-action")]
    public virtual string? UpdateFailureAction { get; set; }

    [CommandSwitch("--update-max-failure-ratio")]
    public virtual string? UpdateMaxFailureRatio { get; set; }

    [CommandSwitch("--update-monitor")]
    public virtual string? UpdateMonitor { get; set; }

    [CommandSwitch("--update-order")]
    public virtual string? UpdateOrder { get; set; }

    [CommandSwitch("--update-parallelism")]
    public virtual int? UpdateParallelism { get; set; }

    [CommandSwitch("--user")]
    public virtual string? User { get; set; }

    [CommandSwitch("--with-registry-auth")]
    public virtual string? WithRegistryAuth { get; set; }

    [CommandSwitch("--workdir")]
    public virtual string? Workdir { get; set; }
}
