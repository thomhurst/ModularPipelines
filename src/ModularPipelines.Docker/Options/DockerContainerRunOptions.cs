using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("container run")]
[ExcludeFromCodeCoverage]
public record DockerContainerRunOptions([property: PositionalArgument(Position = Position.AfterSwitches)] string Image) : DockerOptions
{
    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? Command { get; set; }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public IEnumerable<string>? DockerArgs { get; set; }

    [CommandSwitch("--add-host")]
    public string? AddHost { get; set; }

    [CommandSwitch("--annotation")]
    public string? Annotation { get; set; }

    [BooleanCommandSwitch("--attach")]
    public bool? Attach { get; set; }

    [CommandSwitch("--blkio-weight")]
    public string? BlkioWeight { get; set; }

    [CommandSwitch("--blkio-weight-device")]
    public string? BlkioWeightDevice { get; set; }

    [CommandSwitch("--cap-add")]
    public string? CapAdd { get; set; }

    [CommandSwitch("--cap-drop")]
    public string? CapDrop { get; set; }

    [CommandSwitch("--cgroup-parent")]
    public string? CgroupParent { get; set; }

    [CommandSwitch("--cgroupns")]
    public string? Cgroupns { get; set; }

    [CommandSwitch("--cidfile")]
    public string? Cidfile { get; set; }

    [CommandSwitch("--cpu-count")]
    public string? CpuCount { get; set; }

    [CommandSwitch("--cpu-percent")]
    public string? CpuPercent { get; set; }

    [CommandSwitch("--cpu-period")]
    public string? CpuPeriod { get; set; }

    [CommandSwitch("--cpu-quota")]
    public string? CpuQuota { get; set; }

    [CommandSwitch("--cpu-rt-period")]
    public string? CpuRtPeriod { get; set; }

    [CommandSwitch("--cpu-rt-runtime")]
    public string? CpuRtRuntime { get; set; }

    [CommandSwitch("--cpu-shares")]
    public string? CpuShares { get; set; }

    [CommandSwitch("--cpus")]
    public string? Cpus { get; set; }

    [CommandSwitch("--cpuset-cpus")]
    public string? CpusetCpus { get; set; }

    [CommandSwitch("--cpuset-mems")]
    public string? CpusetMems { get; set; }

    [BooleanCommandSwitch("--detach")]
    public bool? Detach { get; set; }

    [CommandSwitch("--detach-keys")]
    public string? DetachKeys { get; set; }

    [CommandSwitch("--device")]
    public string? Device { get; set; }

    [CommandSwitch("--device-cgroup-rule")]
    public string? DeviceCgroupRule { get; set; }

    [CommandSwitch("--device-read-bps")]
    public string? DeviceReadBps { get; set; }

    [CommandSwitch("--device-read-iops")]
    public string? DeviceReadIops { get; set; }

    [CommandSwitch("--device-write-bps")]
    public string? DeviceWriteBps { get; set; }

    [CommandSwitch("--device-write-iops")]
    public string? DeviceWriteIops { get; set; }

    [BooleanCommandSwitch("--disable-content-trust")]
    public bool? DisableContentTrust { get; set; }

    [CommandSwitch("--dns")]
    public string? Dns { get; set; }

    [CommandSwitch("--dns-option")]
    public string? DnsOption { get; set; }

    [CommandSwitch("--dns-search")]
    public string? DnsSearch { get; set; }

    [CommandSwitch("--domainname")]
    public string? Domainname { get; set; }

    [CommandSwitch("--entrypoint")]
    public string? Entrypoint { get; set; }

    [CommandSwitch("--env")]
    public string? Env { get; set; }

    [CommandSwitch("--env-file")]
    public string? EnvFile { get; set; }

    [CommandSwitch("--expose")]
    public string? Expose { get; set; }

    [CommandSwitch("--gpus")]
    public string? Gpus { get; set; }

    [CommandSwitch("--group-add")]
    public string? GroupAdd { get; set; }

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

    [CommandSwitch("--hostname")]
    public string? Hostname { get; set; }

    [CommandSwitch("--init")]
    public string? Init { get; set; }

    [BooleanCommandSwitch("--interactive")]
    public bool? Interactive { get; set; }

    [CommandSwitch("--io-maxbandwidth")]
    public string? IoMaxbandwidth { get; set; }

    [CommandSwitch("--io-maxiops")]
    public string? IoMaxiops { get; set; }

    [CommandSwitch("--ip")]
    public string? Ip { get; set; }

    [CommandSwitch("--ip6")]
    public string? Ip6 { get; set; }

    [CommandSwitch("--ipc")]
    public string? Ipc { get; set; }

    [BooleanCommandSwitch("--isolation")]
    public bool? Isolation { get; set; }

    [CommandSwitch("--kernel-memory")]
    public string? KernelMemory { get; set; }

    [CommandSwitch("--label")]
    public string? Label { get; set; }

    [CommandSwitch("--label-file")]
    public string? LabelFile { get; set; }

    [CommandSwitch("--link")]
    public string? Link { get; set; }

    [CommandSwitch("--link-local-ip")]
    public string? LinkLocalIp { get; set; }

    [CommandSwitch("--log-driver")]
    public string? LogDriver { get; set; }

    [CommandSwitch("--log-opt")]
    public string? LogOpt { get; set; }

    [CommandSwitch("--mac-address")]
    public string? MacAddress { get; set; }

    [CommandSwitch("--memory")]
    public string? Memory { get; set; }

    [CommandSwitch("--memory-reservation")]
    public string? MemoryReservation { get; set; }

    [CommandSwitch("--memory-swap")]
    public string? MemorySwap { get; set; }

    [CommandSwitch("--memory-swappiness")]
    public int? MemorySwappiness { get; set; }

    [CommandSwitch("--mount")]
    public string? Mount { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--network")]
    public string? Network { get; set; }

    [CommandSwitch("--network-alias")]
    public string? NetworkAlias { get; set; }

    [BooleanCommandSwitch("--no-healthcheck")]
    public bool? NoHealthcheck { get; set; }

    [BooleanCommandSwitch("--oom-kill-disable")]
    public bool? OomKillDisable { get; set; }

    [CommandSwitch("--oom-score-adj")]
    public string? OomScoreAdj { get; set; }

    [CommandSwitch("--pid")]
    public string? Pid { get; set; }

    [CommandSwitch("--pids-limit")]
    public string? PidsLimit { get; set; }

    [CommandSwitch("--platform")]
    public string? Platform { get; set; }

    [BooleanCommandSwitch("--privileged")]
    public bool? Privileged { get; set; }

    [CommandSwitch("--publish")]
    public string? Publish { get; set; }

    [CommandSwitch("--publish-all")]
    public string? PublishAll { get; set; }

    [BooleanCommandSwitch("--pull")]
    public bool? Pull { get; set; }

    [BooleanCommandSwitch("--quiet")]
    public bool? Quiet { get; set; }

    [BooleanCommandSwitch("--read-only")]
    public bool? ReadOnly { get; set; }

    [CommandSwitch("--restart")]
    public string? Restart { get; set; }

    [BooleanCommandSwitch("--rm")]
    public bool? Rm { get; set; }

    [CommandSwitch("--runtime")]
    public string? Runtime { get; set; }

    [CommandSwitch("--security-opt")]
    public string? SecurityOpt { get; set; }

    [CommandSwitch("--shm-size")]
    public string? ShmSize { get; set; }

    [BooleanCommandSwitch("--sig-proxy")]
    public bool? SigProxy { get; set; }

    [CommandSwitch("--stop-signal")]
    public string? StopSignal { get; set; }

    [CommandSwitch("--stop-timeout")]
    public string? StopTimeout { get; set; }

    [CommandSwitch("--storage-opt")]
    public string? StorageOpt { get; set; }

    [CommandSwitch("--sysctl")]
    public string? Sysctl { get; set; }

    [CommandSwitch("--tmpfs")]
    public string? Tmpfs { get; set; }

    [CommandSwitch("--tty")]
    public string? Tty { get; set; }

    [CommandSwitch("--ulimit")]
    public string? Ulimit { get; set; }

    [CommandSwitch("--user")]
    public string? User { get; set; }

    [CommandSwitch("--userns")]
    public string? Userns { get; set; }

    [CommandSwitch("--uts")]
    public string? Uts { get; set; }

    [CommandSwitch("--volume")]
    public string? Volume { get; set; }

    [CommandSwitch("--volume-driver")]
    public string? VolumeDriver { get; set; }

    [CommandSwitch("--volumes-from")]
    public string? VolumesFrom { get; set; }

    [CommandSwitch("--workdir")]
    public string? Workdir { get; set; }
}
