using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

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
    public string? CapAdd { get; set; }

    [CommandSwitch("--cap-drop")]
    public string? CapDrop { get; set; }

    [CommandSwitch("--constraint")]
    public string? Constraint { get; set; }

    [CommandSwitch("--container-label")]
    public string? ContainerLabel { get; set; }

    [CommandSwitch("--credential-spec")]
    public string? CredentialSpec { get; set; }

    [BooleanCommandSwitch("--detach")]
    public bool? Detach { get; set; }

    [CommandSwitch("--dns")]
    public string? Dns { get; set; }

    [CommandSwitch("--dns-option")]
    public string? DnsOption { get; set; }

    [CommandSwitch("--dns-search")]
    public string? DnsSearch { get; set; }

    [CommandSwitch("--endpoint-mode")]
    public string? EndpointMode { get; set; }

    [CommandSwitch("--entrypoint")]
    public string? Entrypoint { get; set; }

    [CommandSwitch("--env")]
    public string? Env { get; set; }

    [CommandSwitch("--env-file")]
    public string? EnvFile { get; set; }

    [CommandSwitch("--generic-resource")]
    public string? GenericResource { get; set; }

    [CommandSwitch("--group")]
    public string? Group { get; set; }

    [CommandSwitch("--health-cmd")]
    public string? HealthCmd { get; set; }

    [CommandSwitch("--health-interval")]
    public string? HealthInterval { get; set; }

    [CommandSwitch("--health-retries")]
    public string? HealthRetries { get; set; }

    [CommandSwitch("--health-start-interval")]
    public string? HealthStartInterval { get; set; }

    [CommandSwitch("--health-start-period")]
    public string? HealthStartPeriod { get; set; }

    [CommandSwitch("--health-timeout")]
    public string? HealthTimeout { get; set; }

    [CommandSwitch("--hostname")]
    public string? Hostname { get; set; }

    [CommandSwitch("--init")]
    public string? Init { get; set; }

    [BooleanCommandSwitch("--isolation")]
    public bool? Isolation { get; set; }

    [CommandSwitch("--label")]
    public string? Label { get; set; }

    [CommandSwitch("--limit-cpu")]
    public string? LimitCpu { get; set; }

    [CommandSwitch("--limit-memory")]
    public string? LimitMemory { get; set; }

    [CommandSwitch("--limit-pids")]
    public string? LimitPids { get; set; }

    [CommandSwitch("--log-driver")]
    public string? LogDriver { get; set; }

    [CommandSwitch("--log-opt")]
    public string? LogOpt { get; set; }

    [CommandSwitch("--max-concurrent")]
    public string? MaxConcurrent { get; set; }

    [CommandSwitch("--mode")]
    public string? Mode { get; set; }

    [CommandSwitch("--mount")]
    public string? Mount { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--network")]
    public string? Network { get; set; }

    [BooleanCommandSwitch("--no-healthcheck")]
    public bool? NoHealthcheck { get; set; }

    [CommandSwitch("--no-resolve-image")]
    public string? NoResolveImage { get; set; }

    [CommandSwitch("--placement-pref")]
    public string? PlacementPref { get; set; }

    [CommandSwitch("--publish")]
    public string? Publish { get; set; }

    [BooleanCommandSwitch("--quiet")]
    public bool? Quiet { get; set; }

    [BooleanCommandSwitch("--read-only")]
    public bool? ReadOnly { get; set; }

    [CommandSwitch("--replicas")]
    public string? Replicas { get; set; }

    [CommandSwitch("--replicas-max-per-node")]
    public string? ReplicasMaxPerNode { get; set; }

    [CommandSwitch("--reserve-cpu")]
    public string? ReserveCpu { get; set; }

    [CommandSwitch("--reserve-memory")]
    public string? ReserveMemory { get; set; }

    [CommandSwitch("--restart-condition")]
    public string? RestartCondition { get; set; }

    [CommandSwitch("--restart-delay")]
    public string? RestartDelay { get; set; }

    [CommandSwitch("--restart-max-attempts")]
    public string? RestartMaxAttempts { get; set; }

    [CommandSwitch("--restart-window")]
    public string? RestartWindow { get; set; }

    [CommandSwitch("--rollback-delay")]
    public string? RollbackDelay { get; set; }

    [CommandSwitch("--rollback-failure-action")]
    public string? RollbackFailureAction { get; set; }

    [CommandSwitch("--rollback-max-failure-ratio")]
    public string? RollbackMaxFailureRatio { get; set; }

    [CommandSwitch("--rollback-monitor")]
    public string? RollbackMonitor { get; set; }

    [CommandSwitch("--rollback-order")]
    public string? RollbackOrder { get; set; }

    [CommandSwitch("--rollback-parallelism")]
    public int? RollbackParallelism { get; set; }

    [CommandSwitch("--secret")]
    public string? Secret { get; set; }

    [CommandSwitch("--stop-grace-period")]
    public string? StopGracePeriod { get; set; }

    [CommandSwitch("--stop-signal")]
    public string? StopSignal { get; set; }

    [CommandSwitch("--sysctl")]
    public string? Sysctl { get; set; }

    [CommandSwitch("--tty")]
    public string? Tty { get; set; }

    [CommandSwitch("--ulimit")]
    public string? Ulimit { get; set; }

    [CommandSwitch("--update-delay")]
    public string? UpdateDelay { get; set; }

    [CommandSwitch("--update-failure-action")]
    public string? UpdateFailureAction { get; set; }

    [CommandSwitch("--update-max-failure-ratio")]
    public string? UpdateMaxFailureRatio { get; set; }

    [CommandSwitch("--update-monitor")]
    public string? UpdateMonitor { get; set; }

    [CommandSwitch("--update-order")]
    public string? UpdateOrder { get; set; }

    [CommandSwitch("--update-parallelism")]
    public int? UpdateParallelism { get; set; }

    [CommandSwitch("--user")]
    public string? User { get; set; }

    [CommandSwitch("--with-registry-auth")]
    public string? WithRegistryAuth { get; set; }

    [CommandSwitch("--workdir")]
    public string? Workdir { get; set; }
}
