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

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public string? Image { get; set; }

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public string? Command { get; set; }

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public IEnumerable<string>? Arg { get; set; }

    [CliOption("--cap-add")]
    public virtual string? CapAdd { get; set; }

    [CliOption("--cap-drop")]
    public virtual string? CapDrop { get; set; }

    [CliOption("--constraint")]
    public virtual string? Constraint { get; set; }

    [CliOption("--container-label")]
    public virtual string? ContainerLabel { get; set; }

    [CliOption("--credential-spec")]
    public virtual string? CredentialSpec { get; set; }

    [CliFlag("--detach")]
    public virtual bool? Detach { get; set; }

    [CliOption("--dns")]
    public virtual string? Dns { get; set; }

    [CliOption("--dns-option")]
    public virtual string? DnsOption { get; set; }

    [CliOption("--dns-search")]
    public virtual string? DnsSearch { get; set; }

    [CliOption("--endpoint-mode")]
    public virtual string? EndpointMode { get; set; }

    [CliOption("--entrypoint")]
    public virtual string? Entrypoint { get; set; }

    [CliOption("--env")]
    public virtual string? Env { get; set; }

    [CliOption("--env-file")]
    public virtual string? EnvFile { get; set; }

    [CliOption("--generic-resource")]
    public virtual string? GenericResource { get; set; }

    [CliOption("--group")]
    public virtual string? Group { get; set; }

    [CliOption("--health-cmd")]
    public virtual string? HealthCmd { get; set; }

    [CliOption("--health-interval")]
    public virtual string? HealthInterval { get; set; }

    [CliOption("--health-retries")]
    public virtual string? HealthRetries { get; set; }

    [CliOption("--health-start-interval")]
    public virtual string? HealthStartInterval { get; set; }

    [CliOption("--health-start-period")]
    public virtual string? HealthStartPeriod { get; set; }

    [CliOption("--health-timeout")]
    public virtual string? HealthTimeout { get; set; }

    [CliOption("--hostname")]
    public virtual string? Hostname { get; set; }

    [CliOption("--init")]
    public virtual string? Init { get; set; }

    [CliFlag("--isolation")]
    public virtual bool? Isolation { get; set; }

    [CliOption("--label")]
    public virtual string? Label { get; set; }

    [CliOption("--limit-cpu")]
    public virtual string? LimitCpu { get; set; }

    [CliOption("--limit-memory")]
    public virtual string? LimitMemory { get; set; }

    [CliOption("--limit-pids")]
    public virtual string? LimitPids { get; set; }

    [CliOption("--log-driver")]
    public virtual string? LogDriver { get; set; }

    [CliOption("--log-opt")]
    public virtual string? LogOpt { get; set; }

    [CliOption("--max-concurrent")]
    public virtual string? MaxConcurrent { get; set; }

    [CliOption("--mode")]
    public virtual string? Mode { get; set; }

    [CliOption("--mount")]
    public virtual string? Mount { get; set; }

    [CliOption("--name")]
    public virtual string? Name { get; set; }

    [CliOption("--network")]
    public virtual string? Network { get; set; }

    [CliFlag("--no-healthcheck")]
    public virtual bool? NoHealthcheck { get; set; }

    [CliOption("--no-resolve-image")]
    public virtual string? NoResolveImage { get; set; }

    [CliOption("--placement-pref")]
    public virtual string? PlacementPref { get; set; }

    [CliOption("--publish")]
    public virtual string? Publish { get; set; }

    [CliFlag("--quiet")]
    public virtual bool? Quiet { get; set; }

    [CliFlag("--read-only")]
    public virtual bool? ReadOnly { get; set; }

    [CliOption("--replicas")]
    public virtual string? Replicas { get; set; }

    [CliOption("--replicas-max-per-node")]
    public virtual string? ReplicasMaxPerNode { get; set; }

    [CliOption("--reserve-cpu")]
    public virtual string? ReserveCpu { get; set; }

    [CliOption("--reserve-memory")]
    public virtual string? ReserveMemory { get; set; }

    [CliOption("--restart-condition")]
    public virtual string? RestartCondition { get; set; }

    [CliOption("--restart-delay")]
    public virtual string? RestartDelay { get; set; }

    [CliOption("--restart-max-attempts")]
    public virtual string? RestartMaxAttempts { get; set; }

    [CliOption("--restart-window")]
    public virtual string? RestartWindow { get; set; }

    [CliOption("--rollback-delay")]
    public virtual string? RollbackDelay { get; set; }

    [CliOption("--rollback-failure-action")]
    public virtual string? RollbackFailureAction { get; set; }

    [CliOption("--rollback-max-failure-ratio")]
    public virtual string? RollbackMaxFailureRatio { get; set; }

    [CliOption("--rollback-monitor")]
    public virtual string? RollbackMonitor { get; set; }

    [CliOption("--rollback-order")]
    public virtual string? RollbackOrder { get; set; }

    [CliOption("--rollback-parallelism")]
    public virtual int? RollbackParallelism { get; set; }

    [CliOption("--secret")]
    public virtual string? Secret { get; set; }

    [CliOption("--stop-grace-period")]
    public virtual string? StopGracePeriod { get; set; }

    [CliOption("--stop-signal")]
    public virtual string? StopSignal { get; set; }

    [CliOption("--sysctl")]
    public virtual string? Sysctl { get; set; }

    [CliOption("--tty")]
    public virtual string? Tty { get; set; }

    [CliOption("--ulimit")]
    public virtual string? Ulimit { get; set; }

    [CliOption("--update-delay")]
    public virtual string? UpdateDelay { get; set; }

    [CliOption("--update-failure-action")]
    public virtual string? UpdateFailureAction { get; set; }

    [CliOption("--update-max-failure-ratio")]
    public virtual string? UpdateMaxFailureRatio { get; set; }

    [CliOption("--update-monitor")]
    public virtual string? UpdateMonitor { get; set; }

    [CliOption("--update-order")]
    public virtual string? UpdateOrder { get; set; }

    [CliOption("--update-parallelism")]
    public virtual int? UpdateParallelism { get; set; }

    [CliOption("--user")]
    public virtual string? User { get; set; }

    [CliOption("--with-registry-auth")]
    public virtual string? WithRegistryAuth { get; set; }

    [CliOption("--workdir")]
    public virtual string? Workdir { get; set; }
}
