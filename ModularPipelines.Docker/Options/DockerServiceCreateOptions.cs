using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("service create")]
public record DockerServiceCreateOptions : DockerOptions
{
    [CommandLongSwitch("cap-add")]
    public string? CapAdd { get; set; }

    [CommandLongSwitch("cap-drop")]
    public string? CapDrop { get; set; }

    [CommandLongSwitch("container-label")]
    public string? ContainerLabel { get; set; }

    [CommandLongSwitch("credential-spec")]
    public string? CredentialSpec { get; set; }

    [CommandLongSwitch("detach")]
    public string? Detach { get; set; }

    [CommandLongSwitch("dns")]
    public string? Dns { get; set; }

    [CommandLongSwitch("dns-option")]
    public string? DnsOption { get; set; }

    [CommandLongSwitch("dns-search")]
    public string? DnsSearch { get; set; }

    [CommandLongSwitch("endpoint-mode")]
    public string? EndpointMode { get; set; }

    [CommandLongSwitch("entrypoint")]
    public string? Entrypoint { get; set; }

    [CommandLongSwitch("env-file")]
    public string? EnvFile { get; set; }

    [CommandLongSwitch("generic-resource")]
    public string? GenericResource { get; set; }

    [CommandLongSwitch("group")]
    public string? Group { get; set; }

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

    [CommandLongSwitch("host")]
    public string? Host { get; set; }

    [CommandLongSwitch("init")]
    public string? Init { get; set; }

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

    [CommandLongSwitch("mode")]
    public string? Mode { get; set; }

    [CommandLongSwitch("name")]
    public string? Name { get; set; }

    [CommandLongSwitch("no-healthcheck")]
    public string? NoHealthcheck { get; set; }

    [CommandLongSwitch("no-resolve-image")]
    public string? NoResolveImage { get; set; }

    [CommandLongSwitch("quiet")]
    public string? Quiet { get; set; }

    [CommandLongSwitch("read-only")]
    public string? ReadOnly { get; set; }

    [CommandLongSwitch("reserve-cpu")]
    public string? ReserveCpu { get; set; }

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

    [CommandLongSwitch("stop-grace-period")]
    public string? StopGracePeriod { get; set; }

    [CommandLongSwitch("stop-signal")]
    public string? StopSignal { get; set; }

    [CommandLongSwitch("sysctl")]
    public string? Sysctl { get; set; }

    [CommandLongSwitch("tty")]
    public string? Tty { get; set; }

    [CommandLongSwitch("ulimit")]
    public string? Ulimit { get; set; }

    [CommandLongSwitch("update-failure-action")]
    public string? UpdateFailureAction { get; set; }

    [CommandLongSwitch("update-max-failure-ratio")]
    public string? UpdateMaxFailureRatio { get; set; }

    [CommandLongSwitch("update-monitor")]
    public string? UpdateMonitor { get; set; }

    [CommandLongSwitch("update-order")]
    public string? UpdateOrder { get; set; }

    [CommandLongSwitch("update-parallelism")]
    public string? UpdateParallelism { get; set; }

    [CommandLongSwitch("user")]
    public string? User { get; set; }

    [CommandLongSwitch("workdir")]
    public string? Workdir { get; set; }

}