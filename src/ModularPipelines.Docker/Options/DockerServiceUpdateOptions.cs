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

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public virtual string? Service { get; set; }

    [CliOption("--args")]
    public virtual string? Args { get; set; }

    [CliOption("--cap-add")]
    public virtual string? CapAdd { get; set; }

    [CliOption("--cap-drop")]
    public virtual string? CapDrop { get; set; }

    [CliOption("--config-add")]
    public virtual string? ConfigAdd { get; set; }

    [CliOption("--config-rm")]
    public virtual string? ConfigRm { get; set; }

    [CliOption("--constraint-add")]
    public virtual string? ConstraintAdd { get; set; }

    [CliOption("--constraint-rm")]
    public virtual string? ConstraintRm { get; set; }

    [CliOption("--container-label-add")]
    public virtual string? ContainerLabelAdd { get; set; }

    [CliOption("--container-label-rm")]
    public virtual string? ContainerLabelRm { get; set; }

    [CliOption("--credential-spec")]
    public virtual string? CredentialSpec { get; set; }

    [CliFlag("--detach")]
    public virtual bool? Detach { get; set; }

    [CliOption("--dns-add")]
    public virtual string? DnsAdd { get; set; }

    [CliOption("--dns-option-add")]
    public virtual string? DnsOptionAdd { get; set; }

    [CliOption("--dns-option-rm")]
    public virtual string? DnsOptionRm { get; set; }

    [CliOption("--dns-rm")]
    public virtual string? DnsRm { get; set; }

    [CliOption("--dns-search-add")]
    public virtual string? DnsSearchAdd { get; set; }

    [CliOption("--dns-search-rm")]
    public virtual string? DnsSearchRm { get; set; }

    [CliOption("--endpoint-mode")]
    public virtual string? EndpointMode { get; set; }

    [CliOption("--entrypoint")]
    public virtual string? Entrypoint { get; set; }

    [CliOption("--env-add")]
    public virtual string? EnvAdd { get; set; }

    [CliOption("--env-rm")]
    public virtual string? EnvRm { get; set; }

    [CliFlag("--force")]
    public virtual bool? Force { get; set; }

    [CliOption("--generic-resource-add")]
    public virtual string? GenericResourceAdd { get; set; }

    [CliOption("--generic-resource-rm")]
    public virtual string? GenericResourceRm { get; set; }

    [CliOption("--group-add")]
    public virtual string? GroupAdd { get; set; }

    [CliOption("--group-rm")]
    public virtual string? GroupRm { get; set; }

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

    [CliOption("--host-add")]
    public virtual string? HostAdd { get; set; }

    [CliOption("--host-rm")]
    public virtual string? HostRm { get; set; }

    [CliOption("--hostname")]
    public virtual string? Hostname { get; set; }

    [CliOption("--image")]
    public virtual string? Image { get; set; }

    [CliOption("--init")]
    public virtual string? Init { get; set; }

    [CliFlag("--isolation")]
    public virtual bool? Isolation { get; set; }

    [CliOption("--label-add")]
    public virtual string? LabelAdd { get; set; }

    [CliOption("--label-rm")]
    public virtual string? LabelRm { get; set; }

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

    [CliOption("--mount-add")]
    public virtual string? MountAdd { get; set; }

    [CliOption("--mount-rm")]
    public virtual string? MountRm { get; set; }

    [CliOption("--network-add")]
    public virtual string? NetworkAdd { get; set; }

    [CliOption("--network-rm")]
    public virtual string? NetworkRm { get; set; }

    [CliFlag("--no-healthcheck")]
    public virtual bool? NoHealthcheck { get; set; }

    [CliOption("--no-resolve-image")]
    public virtual string? NoResolveImage { get; set; }

    [CliOption("--placement-pref-add")]
    public virtual string? PlacementPrefAdd { get; set; }

    [CliOption("--placement-pref-rm")]
    public virtual string? PlacementPrefRm { get; set; }

    [CliOption("--publish-add")]
    public virtual string? PublishAdd { get; set; }

    [CliOption("--publish-rm")]
    public virtual string? PublishRm { get; set; }

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

    [CliOption("--rollback")]
    public virtual string? Rollback { get; set; }

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
    public virtual string? RollbackParallelism { get; set; }

    [CliOption("--secret-add")]
    public virtual string? SecretAdd { get; set; }

    [CliOption("--secret-rm")]
    public virtual string? SecretRm { get; set; }

    [CliOption("--stop-grace-period")]
    public virtual string? StopGracePeriod { get; set; }

    [CliOption("--stop-signal")]
    public virtual string? StopSignal { get; set; }

    [CliOption("--sysctl-add")]
    public virtual string? SysctlAdd { get; set; }

    [CliOption("--sysctl-rm")]
    public virtual string? SysctlRm { get; set; }

    [CliOption("--tty")]
    public virtual string? Tty { get; set; }

    [CliOption("--ulimit-add")]
    public virtual string? UlimitAdd { get; set; }

    [CliOption("--ulimit-rm")]
    public virtual string? UlimitRm { get; set; }

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
    public virtual string? UpdateParallelism { get; set; }

    [CliOption("--user")]
    public virtual string? User { get; set; }

    [CliOption("--with-registry-auth")]
    public virtual string? WithRegistryAuth { get; set; }

    [CliOption("--workdir")]
    public virtual string? Workdir { get; set; }
}
