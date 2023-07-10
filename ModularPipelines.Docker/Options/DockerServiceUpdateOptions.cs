using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("service update")]
public record DockerServiceUpdateOptions : DockerOptions
{
    [CommandLongSwitch("args")]
    public string? Args { get; set; }

    [CommandLongSwitch("cap-add")]
    public string? CapAdd { get; set; }

    [CommandLongSwitch("cap-drop")]
    public string? CapDrop { get; set; }

    [CommandLongSwitch("config-add")]
    public string? ConfigAdd { get; set; }

    [CommandLongSwitch("config-rm")]
    public string? ConfigRm { get; set; }

    [CommandLongSwitch("constraint-add")]
    public string? ConstraintAdd { get; set; }

    [CommandLongSwitch("constraint-rm")]
    public string? ConstraintRm { get; set; }

    [CommandLongSwitch("container-label-add")]
    public string? ContainerLabelAdd { get; set; }

    [CommandLongSwitch("container-label-rm")]
    public string? ContainerLabelRm { get; set; }

    [CommandLongSwitch("credential-spec")]
    public string? CredentialSpec { get; set; }

    [CommandLongSwitch("detach")]
    public string? Detach { get; set; }

    [CommandLongSwitch("dns-add")]
    public string? DnsAdd { get; set; }

    [CommandLongSwitch("dns-option-add")]
    public string? DnsOptionAdd { get; set; }

    [CommandLongSwitch("dns-option-rm")]
    public string? DnsOptionRm { get; set; }

    [CommandLongSwitch("dns-rm")]
    public string? DnsRm { get; set; }

    [CommandLongSwitch("dns-search-add")]
    public string? DnsSearchAdd { get; set; }

    [CommandLongSwitch("dns-search-rm")]
    public string? DnsSearchRm { get; set; }

    [CommandLongSwitch("endpoint-mode")]
    public string? EndpointMode { get; set; }

    [CommandLongSwitch("entrypoint")]
    public string? Entrypoint { get; set; }

    [CommandLongSwitch("env-add")]
    public string? EnvAdd { get; set; }

    [CommandLongSwitch("env-rm")]
    public string? EnvRm { get; set; }

    [CommandLongSwitch("force")]
    public string? Force { get; set; }

    [CommandLongSwitch("generic-resource-add")]
    public string? GenericResourceAdd { get; set; }

    [CommandLongSwitch("generic-resource-rm")]
    public string? GenericResourceRm { get; set; }

    [CommandLongSwitch("group-add")]
    public string? GroupAdd { get; set; }

    [CommandLongSwitch("group-rm")]
    public string? GroupRm { get; set; }

    [CommandLongSwitch("health-cmd")]
    public string? HealthCmd { get; set; }

    [CommandLongSwitch("health-interval")]
    public string? HealthInterval { get; set; }

    [CommandLongSwitch("health-retries")]
    public string? HealthRetries { get; set; }

    [CommandLongSwitch("health-start-period")]
    public string? HealthStartPeriod { get; set; }

    [CommandLongSwitch("health-timeout")]
    public string? HealthTimeout { get; set; }

    [CommandLongSwitch("host-add")]
    public string? HostAdd { get; set; }

    [CommandLongSwitch("host-rm")]
    public string? HostRm { get; set; }

    [CommandLongSwitch("hostname")]
    public string? Hostname { get; set; }

    [CommandLongSwitch("image")]
    public string? Image { get; set; }

    [CommandLongSwitch("init")]
    public string? Init { get; set; }

    [CommandLongSwitch("label-add")]
    public string? LabelAdd { get; set; }

    [CommandLongSwitch("label-rm")]
    public string? LabelRm { get; set; }

    [CommandLongSwitch("limit-cpu")]
    public string? LimitCpu { get; set; }

    [CommandLongSwitch("limit-memory")]
    public string? LimitMemory { get; set; }

    [CommandLongSwitch("limit-pids")]
    public string? LimitPids { get; set; }

    [CommandLongSwitch("log-driver")]
    public string? LogDriver { get; set; }

    [CommandLongSwitch("log-opt")]
    public string? LogOpt { get; set; }

    [CommandLongSwitch("max-concurrent")]
    public string? MaxConcurrent { get; set; }

    [CommandLongSwitch("mount-rm")]
    public string? MountRm { get; set; }

    [CommandLongSwitch("network-rm")]
    public string? NetworkRm { get; set; }

    [CommandLongSwitch("no-healthcheck")]
    public string? NoHealthcheck { get; set; }

    [CommandLongSwitch("no-resolve-image")]
    public string? NoResolveImage { get; set; }

    [CommandLongSwitch("placement-pref-add")]
    public string? PlacementPrefAdd { get; set; }

    [CommandLongSwitch("placement-pref-rm")]
    public string? PlacementPrefRm { get; set; }

    [CommandLongSwitch("publish-rm")]
    public string? PublishRm { get; set; }

    [CommandLongSwitch("quiet")]
    public string? Quiet { get; set; }

    [CommandLongSwitch("read-only")]
    public string? ReadOnly { get; set; }

    [CommandLongSwitch("replicas")]
    public string? Replicas { get; set; }

    [CommandLongSwitch("replicas-max-per-node")]
    public string? ReplicasMaxPerNode { get; set; }

    [CommandLongSwitch("reserve-cpu")]
    public string? ReserveCpu { get; set; }

    [CommandLongSwitch("reserve-memory")]
    public string? ReserveMemory { get; set; }

    [CommandLongSwitch("restart-condition")]
    public string? RestartCondition { get; set; }

    [CommandLongSwitch("restart-delay")]
    public string? RestartDelay { get; set; }

    [CommandLongSwitch("restart-max-attempts")]
    public string? RestartMaxAttempts { get; set; }

    [CommandLongSwitch("restart-window")]
    public string? RestartWindow { get; set; }

    [CommandLongSwitch("rollback-delay")]
    public string? RollbackDelay { get; set; }

    [CommandLongSwitch("rollback-failure-action")]
    public string? RollbackFailureAction { get; set; }

    [CommandLongSwitch("rollback-max-failure-ratio")]
    public string? RollbackMaxFailureRatio { get; set; }

    [CommandLongSwitch("rollback-monitor")]
    public string? RollbackMonitor { get; set; }

    [CommandLongSwitch("rollback-order")]
    public string? RollbackOrder { get; set; }

    [CommandLongSwitch("rollback-parallelism")]
    public string? RollbackParallelism { get; set; }

    [CommandLongSwitch("secret-rm")]
    public string? SecretRm { get; set; }

    [CommandLongSwitch("stop-grace-period")]
    public string? StopGracePeriod { get; set; }

    [CommandLongSwitch("stop-signal")]
    public string? StopSignal { get; set; }

    [CommandLongSwitch("sysctl-add")]
    public string? SysctlAdd { get; set; }

    [CommandLongSwitch("sysctl-rm")]
    public string? SysctlRm { get; set; }

    [CommandLongSwitch("tty")]
    public string? Tty { get; set; }

    [CommandLongSwitch("ulimit-add")]
    public string? UlimitAdd { get; set; }

    [CommandLongSwitch("ulimit-rm")]
    public string? UlimitRm { get; set; }

    [CommandLongSwitch("update-delay")]
    public string? UpdateDelay { get; set; }

    [CommandLongSwitch("update-failure-action")]
    public string? UpdateFailureAction { get; set; }

    [CommandLongSwitch("update-max-failure-ratio")]
    public string? UpdateMaxFailureRatio { get; set; }

    [CommandLongSwitch("update-monitor")]
    public string? UpdateMonitor { get; set; }

    [CommandLongSwitch("update-order")]
    public string? UpdateOrder { get; set; }

    [CommandLongSwitch("user")]
    public string? User { get; set; }

    [CommandLongSwitch("with-registry-auth")]
    public string? WithRegistryAuth { get; set; }

    [CommandLongSwitch("workdir")]
    public string? Workdir { get; set; }

}