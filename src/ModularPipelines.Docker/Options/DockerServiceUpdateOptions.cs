using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("service update")]
[ExcludeFromCodeCoverage]
public record DockerServiceUpdateOptions([property: PositionalArgument(Position = Position.AfterSwitches)] string Service) : DockerOptions
{
    [CommandSwitch("--args")]
    public string? Args { get; set; }

    [CommandSwitch("--cap-add")]
    public string? CapAdd { get; set; }

    [CommandSwitch("--cap-drop")]
    public string? CapDrop { get; set; }

    [CommandSwitch("--config-add")]
    public string? ConfigAdd { get; set; }

    [CommandSwitch("--config-rm")]
    public string? ConfigRm { get; set; }

    [CommandSwitch("--constraint-add")]
    public string? ConstraintAdd { get; set; }

    [CommandSwitch("--constraint-rm")]
    public string? ConstraintRm { get; set; }

    [CommandSwitch("--container-label-add")]
    public string? ContainerLabelAdd { get; set; }

    [CommandSwitch("--container-label-rm")]
    public string? ContainerLabelRm { get; set; }

    [CommandSwitch("--credential-spec")]
    public string? CredentialSpec { get; set; }

    [BooleanCommandSwitch("--detach")]
    public bool? Detach { get; set; }

    [CommandSwitch("--dns-add")]
    public string? DnsAdd { get; set; }

    [CommandSwitch("--dns-option-add")]
    public string? DnsOptionAdd { get; set; }

    [CommandSwitch("--dns-option-rm")]
    public string? DnsOptionRm { get; set; }

    [CommandSwitch("--dns-rm")]
    public string? DnsRm { get; set; }

    [CommandSwitch("--dns-search-add")]
    public string? DnsSearchAdd { get; set; }

    [CommandSwitch("--dns-search-rm")]
    public string? DnsSearchRm { get; set; }

    [CommandSwitch("--endpoint-mode")]
    public string? EndpointMode { get; set; }

    [CommandSwitch("--entrypoint")]
    public string? Entrypoint { get; set; }

    [CommandSwitch("--env-add")]
    public string? EnvAdd { get; set; }

    [CommandSwitch("--env-rm")]
    public string? EnvRm { get; set; }

    [BooleanCommandSwitch("--force")]
    public bool? Force { get; set; }

    [CommandSwitch("--generic-resource-add")]
    public string? GenericResourceAdd { get; set; }

    [CommandSwitch("--generic-resource-rm")]
    public string? GenericResourceRm { get; set; }

    [CommandSwitch("--group-add")]
    public string? GroupAdd { get; set; }

    [CommandSwitch("--group-rm")]
    public string? GroupRm { get; set; }

    [CommandSwitch("--health-cmd")]
    public string? HealthCmd { get; set; }

    [CommandSwitch("--health-interval")]
    public string? HealthInterval { get; set; }

    [CommandSwitch("--health-retries")]
    public string? HealthRetries { get; set; }

    [CommandSwitch("--health-start-period")]
    public string? HealthStartPeriod { get; set; }

    [CommandSwitch("--health-timeout")]
    public string? HealthTimeout { get; set; }

    [CommandSwitch("--host-add")]
    public string? HostAdd { get; set; }

    [CommandSwitch("--host-rm")]
    public string? HostRm { get; set; }

    [CommandSwitch("--hostname")]
    public string? Hostname { get; set; }

    [CommandSwitch("--image")]
    public string? Image { get; set; }

    [CommandSwitch("--init")]
    public string? Init { get; set; }

    [CommandSwitch("--label-add")]
    public string? LabelAdd { get; set; }

    [CommandSwitch("--label-rm")]
    public string? LabelRm { get; set; }

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

    [CommandSwitch("--mount-rm")]
    public string? MountRm { get; set; }

    [CommandSwitch("--network-rm")]
    public string? NetworkRm { get; set; }

    [BooleanCommandSwitch("--no-healthcheck")]
    public bool? NoHealthcheck { get; set; }

    [CommandSwitch("--no-resolve-image")]
    public string? NoResolveImage { get; set; }

    [CommandSwitch("--placement-pref-add")]
    public string? PlacementPrefAdd { get; set; }

    [CommandSwitch("--placement-pref-rm")]
    public string? PlacementPrefRm { get; set; }

    [CommandSwitch("--publish-rm")]
    public string? PublishRm { get; set; }

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
    public string? RollbackParallelism { get; set; }

    [CommandSwitch("--secret-rm")]
    public string? SecretRm { get; set; }

    [CommandSwitch("--stop-grace-period")]
    public string? StopGracePeriod { get; set; }

    [CommandSwitch("--stop-signal")]
    public string? StopSignal { get; set; }

    [CommandSwitch("--sysctl-add")]
    public string? SysctlAdd { get; set; }

    [CommandSwitch("--sysctl-rm")]
    public string? SysctlRm { get; set; }

    [CommandSwitch("--tty")]
    public string? Tty { get; set; }

    [CommandSwitch("--ulimit-add")]
    public string? UlimitAdd { get; set; }

    [CommandSwitch("--ulimit-rm")]
    public string? UlimitRm { get; set; }

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

    [CommandSwitch("--user")]
    public string? User { get; set; }

    [CommandSwitch("--with-registry-auth")]
    public string? WithRegistryAuth { get; set; }

    [CommandSwitch("--workdir")]
    public string? Workdir { get; set; }

    [BooleanCommandSwitch("--isolation")]
    public bool? Isolation { get; set; }

    [CommandSwitch("--mount-add")]
    public string? MountAdd { get; set; }

    [CommandSwitch("--network-add")]
    public string? NetworkAdd { get; set; }

    [CommandSwitch("--publish-add")]
    public string? PublishAdd { get; set; }

    [CommandSwitch("--rollback")]
    public string? Rollback { get; set; }

    [CommandSwitch("--secret-add")]
    public string? SecretAdd { get; set; }

    [CommandSwitch("--update-parallelism")]
    public string? UpdateParallelism { get; set; }
}