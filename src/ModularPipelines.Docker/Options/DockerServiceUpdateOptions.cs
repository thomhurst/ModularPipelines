using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[ExcludeFromCodeCoverage]
public record DockerServiceUpdateOptions : DockerOptions
{
    public DockerServiceUpdateOptions(
        string service
    )
    {
        CommandParts = ["service", "update"];

        Service = service;
    }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? Service { get; set; }

    [CommandSwitch("--args")]
    public virtual string? Args { get; set; }

    [CommandSwitch("--cap-add")]
    public virtual string? CapAdd { get; set; }

    [CommandSwitch("--cap-drop")]
    public virtual string? CapDrop { get; set; }

    [CommandSwitch("--config-add")]
    public virtual string? ConfigAdd { get; set; }

    [CommandSwitch("--config-rm")]
    public virtual string? ConfigRm { get; set; }

    [CommandSwitch("--constraint-add")]
    public virtual string? ConstraintAdd { get; set; }

    [CommandSwitch("--constraint-rm")]
    public virtual string? ConstraintRm { get; set; }

    [CommandSwitch("--container-label-add")]
    public virtual string? ContainerLabelAdd { get; set; }

    [CommandSwitch("--container-label-rm")]
    public virtual string? ContainerLabelRm { get; set; }

    [CommandSwitch("--credential-spec")]
    public virtual string? CredentialSpec { get; set; }

    [BooleanCommandSwitch("--detach")]
    public virtual bool? Detach { get; set; }

    [CommandSwitch("--dns-add")]
    public virtual string? DnsAdd { get; set; }

    [CommandSwitch("--dns-option-add")]
    public virtual string? DnsOptionAdd { get; set; }

    [CommandSwitch("--dns-option-rm")]
    public virtual string? DnsOptionRm { get; set; }

    [CommandSwitch("--dns-rm")]
    public virtual string? DnsRm { get; set; }

    [CommandSwitch("--dns-search-add")]
    public virtual string? DnsSearchAdd { get; set; }

    [CommandSwitch("--dns-search-rm")]
    public virtual string? DnsSearchRm { get; set; }

    [CommandSwitch("--endpoint-mode")]
    public virtual string? EndpointMode { get; set; }

    [CommandSwitch("--entrypoint")]
    public virtual string? Entrypoint { get; set; }

    [CommandSwitch("--env-add")]
    public virtual string? EnvAdd { get; set; }

    [CommandSwitch("--env-rm")]
    public virtual string? EnvRm { get; set; }

    [BooleanCommandSwitch("--force")]
    public virtual bool? Force { get; set; }

    [CommandSwitch("--generic-resource-add")]
    public virtual string? GenericResourceAdd { get; set; }

    [CommandSwitch("--generic-resource-rm")]
    public virtual string? GenericResourceRm { get; set; }

    [CommandSwitch("--group-add")]
    public virtual string? GroupAdd { get; set; }

    [CommandSwitch("--group-rm")]
    public virtual string? GroupRm { get; set; }

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

    [CommandSwitch("--host-add")]
    public virtual string? HostAdd { get; set; }

    [CommandSwitch("--host-rm")]
    public virtual string? HostRm { get; set; }

    [CommandSwitch("--hostname")]
    public virtual string? Hostname { get; set; }

    [CommandSwitch("--image")]
    public virtual string? Image { get; set; }

    [CommandSwitch("--init")]
    public virtual string? Init { get; set; }

    [BooleanCommandSwitch("--isolation")]
    public virtual bool? Isolation { get; set; }

    [CommandSwitch("--label-add")]
    public virtual string? LabelAdd { get; set; }

    [CommandSwitch("--label-rm")]
    public virtual string? LabelRm { get; set; }

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

    [CommandSwitch("--mount-add")]
    public virtual string? MountAdd { get; set; }

    [CommandSwitch("--mount-rm")]
    public virtual string? MountRm { get; set; }

    [CommandSwitch("--network-add")]
    public virtual string? NetworkAdd { get; set; }

    [CommandSwitch("--network-rm")]
    public virtual string? NetworkRm { get; set; }

    [BooleanCommandSwitch("--no-healthcheck")]
    public virtual bool? NoHealthcheck { get; set; }

    [CommandSwitch("--no-resolve-image")]
    public virtual string? NoResolveImage { get; set; }

    [CommandSwitch("--placement-pref-add")]
    public virtual string? PlacementPrefAdd { get; set; }

    [CommandSwitch("--placement-pref-rm")]
    public virtual string? PlacementPrefRm { get; set; }

    [CommandSwitch("--publish-add")]
    public virtual string? PublishAdd { get; set; }

    [CommandSwitch("--publish-rm")]
    public virtual string? PublishRm { get; set; }

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

    [CommandSwitch("--rollback")]
    public virtual string? Rollback { get; set; }

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
    public virtual string? RollbackParallelism { get; set; }

    [CommandSwitch("--secret-add")]
    public virtual string? SecretAdd { get; set; }

    [CommandSwitch("--secret-rm")]
    public virtual string? SecretRm { get; set; }

    [CommandSwitch("--stop-grace-period")]
    public virtual string? StopGracePeriod { get; set; }

    [CommandSwitch("--stop-signal")]
    public virtual string? StopSignal { get; set; }

    [CommandSwitch("--sysctl-add")]
    public virtual string? SysctlAdd { get; set; }

    [CommandSwitch("--sysctl-rm")]
    public virtual string? SysctlRm { get; set; }

    [CommandSwitch("--tty")]
    public virtual string? Tty { get; set; }

    [CommandSwitch("--ulimit-add")]
    public virtual string? UlimitAdd { get; set; }

    [CommandSwitch("--ulimit-rm")]
    public virtual string? UlimitRm { get; set; }

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
    public virtual string? UpdateParallelism { get; set; }

    [CommandSwitch("--user")]
    public virtual string? User { get; set; }

    [CommandSwitch("--with-registry-auth")]
    public virtual string? WithRegistryAuth { get; set; }

    [CommandSwitch("--workdir")]
    public virtual string? Workdir { get; set; }
}
