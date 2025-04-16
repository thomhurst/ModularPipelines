using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[ExcludeFromCodeCoverage]
public record DockerContainerCreateOptions : DockerOptions
{
    public DockerContainerCreateOptions(
        string image
    )
    {
        CommandParts = ["container", "create"];

        Image = image;
    }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? Image { get; set; }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? Command { get; set; }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public IEnumerable<string>? Arg { get; set; }

    [CommandSwitch("--add-host")]
    public virtual string? AddHost { get; set; }

    [CommandSwitch("--annotation")]
    public virtual string? Annotation { get; set; }

    [BooleanCommandSwitch("--attach")]
    public virtual bool? Attach { get; set; }

    [CommandSwitch("--blkio-weight")]
    public virtual string? BlkioWeight { get; set; }

    [CommandSwitch("--blkio-weight-device")]
    public virtual string? BlkioWeightDevice { get; set; }

    [CommandSwitch("--cap-add")]
    public virtual string? CapAdd { get; set; }

    [CommandSwitch("--cap-drop")]
    public virtual string? CapDrop { get; set; }

    [CommandSwitch("--cgroup-parent")]
    public virtual string? CgroupParent { get; set; }

    [CommandSwitch("--cgroupns")]
    public virtual string? Cgroupns { get; set; }

    [CommandSwitch("--cidfile")]
    public virtual string? Cidfile { get; set; }

    [CommandSwitch("--cpu-count")]
    public virtual string? CpuCount { get; set; }

    [CommandSwitch("--cpu-percent")]
    public virtual string? CpuPercent { get; set; }

    [CommandSwitch("--cpu-period")]
    public virtual string? CpuPeriod { get; set; }

    [CommandSwitch("--cpu-quota")]
    public virtual string? CpuQuota { get; set; }

    [CommandSwitch("--cpu-rt-period")]
    public virtual string? CpuRtPeriod { get; set; }

    [CommandSwitch("--cpu-rt-runtime")]
    public virtual string? CpuRtRuntime { get; set; }

    [CommandSwitch("--cpu-shares")]
    public virtual string? CpuShares { get; set; }

    [CommandSwitch("--cpus")]
    public virtual string? Cpus { get; set; }

    [CommandSwitch("--cpuset-cpus")]
    public virtual string? CpusetCpus { get; set; }

    [CommandSwitch("--cpuset-mems")]
    public virtual string? CpusetMems { get; set; }

    [CommandSwitch("--device")]
    public virtual string? Device { get; set; }

    [CommandSwitch("--device-cgroup-rule")]
    public virtual string? DeviceCgroupRule { get; set; }

    [CommandSwitch("--device-read-bps")]
    public virtual string? DeviceReadBps { get; set; }

    [CommandSwitch("--device-read-iops")]
    public virtual string? DeviceReadIops { get; set; }

    [CommandSwitch("--device-write-bps")]
    public virtual string? DeviceWriteBps { get; set; }

    [CommandSwitch("--device-write-iops")]
    public virtual string? DeviceWriteIops { get; set; }

    [BooleanCommandSwitch("--disable-content-trust")]
    public virtual bool? DisableContentTrust { get; set; }

    [CommandSwitch("--dns")]
    public virtual string? Dns { get; set; }

    [CommandSwitch("--dns-opt")]
    public virtual string? DnsOpt { get; set; }

    [CommandSwitch("--dns-option")]
    public virtual string? DnsOption { get; set; }

    [CommandSwitch("--dns-search")]
    public virtual string? DnsSearch { get; set; }

    [CommandSwitch("--domainname")]
    public virtual string? Domainname { get; set; }

    [CommandSwitch("--entrypoint")]
    public virtual string? Entrypoint { get; set; }

    [CommandSwitch("--env")]
    public virtual string? Env { get; set; }

    [CommandSwitch("--env-file")]
    public virtual string? EnvFile { get; set; }

    [CommandSwitch("--expose")]
    public virtual string? Expose { get; set; }

    [CommandSwitch("--gpus")]
    public virtual string? Gpus { get; set; }

    [CommandSwitch("--group-add")]
    public virtual string? GroupAdd { get; set; }

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

    [BooleanCommandSwitch("--interactive")]
    public virtual bool? Interactive { get; set; }

    [CommandSwitch("--io-maxbandwidth")]
    public virtual string? IoMaxbandwidth { get; set; }

    [CommandSwitch("--io-maxiops")]
    public virtual string? IoMaxiops { get; set; }

    [CommandSwitch("--ip")]
    public virtual string? Ip { get; set; }

    [CommandSwitch("--ip6")]
    public virtual string? Ip6 { get; set; }

    [CommandSwitch("--ipc")]
    public virtual string? Ipc { get; set; }

    [BooleanCommandSwitch("--isolation")]
    public virtual bool? Isolation { get; set; }

    [CommandSwitch("--kernel-memory")]
    public virtual string? KernelMemory { get; set; }

    [CommandSwitch("--label")]
    public virtual string? Label { get; set; }

    [CommandSwitch("--label-file")]
    public virtual string? LabelFile { get; set; }

    [CommandSwitch("--link")]
    public virtual string? Link { get; set; }

    [CommandSwitch("--link-local-ip")]
    public virtual string? LinkLocalIp { get; set; }

    [CommandSwitch("--log-driver")]
    public virtual string? LogDriver { get; set; }

    [CommandSwitch("--log-opt")]
    public virtual string? LogOpt { get; set; }

    [CommandSwitch("--mac-address")]
    public virtual string? MacAddress { get; set; }

    [CommandSwitch("--memory")]
    public virtual string? Memory { get; set; }

    [CommandSwitch("--memory-reservation")]
    public virtual string? MemoryReservation { get; set; }

    [CommandSwitch("--memory-swap")]
    public virtual string? MemorySwap { get; set; }

    [CommandSwitch("--memory-swappiness")]
    public virtual int? MemorySwappiness { get; set; }

    [CommandSwitch("--mount")]
    public virtual string? Mount { get; set; }

    [CommandSwitch("--name")]
    public virtual string? Name { get; set; }

    [CommandSwitch("--net")]
    public virtual string? Net { get; set; }

    [CommandSwitch("--net-alias")]
    public virtual string? NetAlias { get; set; }

    [CommandSwitch("--network")]
    public virtual string? Network { get; set; }

    [CommandSwitch("--network-alias")]
    public virtual string? NetworkAlias { get; set; }

    [BooleanCommandSwitch("--no-healthcheck")]
    public virtual bool? NoHealthcheck { get; set; }

    [BooleanCommandSwitch("--oom-kill-disable")]
    public virtual bool? OomKillDisable { get; set; }

    [CommandSwitch("--oom-score-adj")]
    public virtual string? OomScoreAdj { get; set; }

    [CommandSwitch("--pid")]
    public virtual string? Pid { get; set; }

    [CommandSwitch("--pids-limit")]
    public virtual string? PidsLimit { get; set; }

    [CommandSwitch("--platform")]
    public virtual string? Platform { get; set; }

    [BooleanCommandSwitch("--privileged")]
    public virtual bool? Privileged { get; set; }

    [CommandSwitch("--publish")]
    public virtual string? Publish { get; set; }

    [CommandSwitch("--publish-all")]
    public virtual string? PublishAll { get; set; }

    [BooleanCommandSwitch("--pull")]
    public virtual bool? Pull { get; set; }

    [BooleanCommandSwitch("--quiet")]
    public virtual bool? Quiet { get; set; }

    [BooleanCommandSwitch("--read-only")]
    public virtual bool? ReadOnly { get; set; }

    [CommandSwitch("--restart")]
    public virtual string? Restart { get; set; }

    [BooleanCommandSwitch("--rm")]
    public virtual bool? Rm { get; set; }

    [CommandSwitch("--runtime")]
    public virtual string? Runtime { get; set; }

    [CommandSwitch("--security-opt")]
    public virtual string? SecurityOpt { get; set; }

    [CommandSwitch("--shm-size")]
    public virtual string? ShmSize { get; set; }

    [CommandSwitch("--stop-signal")]
    public virtual string? StopSignal { get; set; }

    [CommandSwitch("--stop-timeout")]
    public virtual string? StopTimeout { get; set; }

    [CommandSwitch("--storage-opt")]
    public virtual string? StorageOpt { get; set; }

    [CommandSwitch("--sysctl")]
    public virtual string? Sysctl { get; set; }

    [CommandSwitch("--tmpfs")]
    public virtual string? Tmpfs { get; set; }

    [CommandSwitch("--tty")]
    public virtual string? Tty { get; set; }

    [CommandSwitch("--ulimit")]
    public virtual string? Ulimit { get; set; }

    [CommandSwitch("--user")]
    public virtual string? User { get; set; }

    [CommandSwitch("--userns")]
    public virtual string? Userns { get; set; }

    [CommandSwitch("--uts")]
    public virtual string? Uts { get; set; }

    [CommandSwitch("--volume")]
    public virtual string? Volume { get; set; }

    [CommandSwitch("--volume-driver")]
    public virtual string? VolumeDriver { get; set; }

    [CommandSwitch("--volumes-from")]
    public virtual string? VolumesFrom { get; set; }

    [CommandSwitch("--workdir")]
    public virtual string? Workdir { get; set; }
}
