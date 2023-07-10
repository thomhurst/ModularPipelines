using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("container run")]
public record DockerContainerRunOptions : DockerOptions
{
    [CommandLongSwitch("add-host")]
    public string? AddHost { get; set; }

    [CommandLongSwitch("annotation")]
    public string? Annotation { get; set; }

    [CommandLongSwitch("attach")]
    public string? Attach { get; set; }

    [CommandLongSwitch("blkio-weight")]
    public string? BlkioWeight { get; set; }

    [CommandLongSwitch("blkio-weight-device")]
    public string? BlkioWeightDevice { get; set; }

    [CommandLongSwitch("cap-add")]
    public string? CapAdd { get; set; }

    [CommandLongSwitch("cap-drop")]
    public string? CapDrop { get; set; }

    [CommandLongSwitch("cgroup-parent")]
    public string? CgroupParent { get; set; }

    [CommandLongSwitch("cgroupns")]
    public string? Cgroupns { get; set; }

    [CommandLongSwitch("cidfile")]
    public string? Cidfile { get; set; }

    [CommandLongSwitch("cpu-count")]
    public string? CpuCount { get; set; }

    [CommandLongSwitch("cpu-percent")]
    public string? CpuPercent { get; set; }

    [CommandLongSwitch("cpu-period")]
    public string? CpuPeriod { get; set; }

    [CommandLongSwitch("cpu-quota")]
    public string? CpuQuota { get; set; }

    [CommandLongSwitch("cpu-rt-period")]
    public string? CpuRtPeriod { get; set; }

    [CommandLongSwitch("cpu-rt-runtime")]
    public string? CpuRtRuntime { get; set; }

    [CommandLongSwitch("cpu-shares")]
    public string? CpuShares { get; set; }

    [CommandLongSwitch("cpus")]
    public string? Cpus { get; set; }

    [CommandLongSwitch("cpuset-cpus")]
    public string? CpusetCpus { get; set; }

    [CommandLongSwitch("cpuset-mems")]
    public string? CpusetMems { get; set; }

    [CommandLongSwitch("detach")]
    public string? Detach { get; set; }

    [CommandLongSwitch("detach-keys")]
    public string? DetachKeys { get; set; }

    [CommandLongSwitch("device")]
    public string? Device { get; set; }

    [CommandLongSwitch("device-cgroup-rule")]
    public string? DeviceCgroupRule { get; set; }

    [CommandLongSwitch("device-read-bps")]
    public string? DeviceReadBps { get; set; }

    [CommandLongSwitch("device-read-iops")]
    public string? DeviceReadIops { get; set; }

    [CommandLongSwitch("device-write-bps")]
    public string? DeviceWriteBps { get; set; }

    [CommandLongSwitch("device-write-iops")]
    public string? DeviceWriteIops { get; set; }

    [BooleanCommandSwitch("disable-content-trust")]
    public bool? DisableContentTrust { get; set; }

    [CommandLongSwitch("dns")]
    public string? Dns { get; set; }

    [CommandLongSwitch("dns-option")]
    public string? DnsOption { get; set; }

    [CommandLongSwitch("dns-search")]
    public string? DnsSearch { get; set; }

    [CommandLongSwitch("domainname")]
    public string? Domainname { get; set; }

    [CommandLongSwitch("entrypoint")]
    public string? Entrypoint { get; set; }

    [CommandLongSwitch("env")]
    public string? Env { get; set; }

    [CommandLongSwitch("env-file")]
    public string? EnvFile { get; set; }

    [CommandLongSwitch("expose")]
    public string? Expose { get; set; }

    [CommandLongSwitch("gpus")]
    public string? Gpus { get; set; }

    [CommandLongSwitch("group-add")]
    public string? GroupAdd { get; set; }

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

    [CommandLongSwitch("hostname")]
    public string? Hostname { get; set; }

    [CommandLongSwitch("init")]
    public string? Init { get; set; }

    [CommandLongSwitch("interactive")]
    public string? Interactive { get; set; }

    [CommandLongSwitch("io-maxbandwidth")]
    public string? IoMaxbandwidth { get; set; }

    [CommandLongSwitch("io-maxiops")]
    public string? IoMaxiops { get; set; }

    [CommandLongSwitch("ip")]
    public string? Ip { get; set; }

    [CommandLongSwitch("ip6")]
    public string? Ip6 { get; set; }

    [CommandLongSwitch("ipc")]
    public string? Ipc { get; set; }

    [CommandLongSwitch("isolation")]
    public string? Isolation { get; set; }

    [CommandLongSwitch("kernel-memory")]
    public string? KernelMemory { get; set; }

    [CommandLongSwitch("label")]
    public string? Label { get; set; }

    [CommandLongSwitch("label-file")]
    public string? LabelFile { get; set; }

    [CommandLongSwitch("link")]
    public string? Link { get; set; }

    [CommandLongSwitch("link-local-ip")]
    public string? LinkLocalIp { get; set; }

    [CommandLongSwitch("log-driver")]
    public string? LogDriver { get; set; }

    [CommandLongSwitch("log-opt")]
    public string? LogOpt { get; set; }

    [CommandLongSwitch("mac-address")]
    public string? MacAddress { get; set; }

    [CommandLongSwitch("memory")]
    public string? Memory { get; set; }

    [CommandLongSwitch("memory-reservation")]
    public string? MemoryReservation { get; set; }

    [CommandLongSwitch("memory-swap")]
    public string? MemorySwap { get; set; }

    [CommandLongSwitch("memory-swappiness")]
    public string? MemorySwappiness { get; set; }

    [CommandLongSwitch("mount")]
    public string? Mount { get; set; }

    [CommandLongSwitch("name")]
    public string? Name { get; set; }

    [CommandLongSwitch("network")]
    public string? Network { get; set; }

    [CommandLongSwitch("network-alias")]
    public string? NetworkAlias { get; set; }

    [CommandLongSwitch("no-healthcheck")]
    public string? NoHealthcheck { get; set; }

    [CommandLongSwitch("oom-kill-disable")]
    public string? OomKillDisable { get; set; }

    [CommandLongSwitch("oom-score-adj")]
    public string? OomScoreAdj { get; set; }

    [CommandLongSwitch("pid")]
    public string? Pid { get; set; }

    [CommandLongSwitch("pids-limit")]
    public string? PidsLimit { get; set; }

    [CommandLongSwitch("platform")]
    public string? Platform { get; set; }

    [CommandLongSwitch("privileged")]
    public string? Privileged { get; set; }

    [CommandLongSwitch("publish")]
    public string? Publish { get; set; }

    [CommandLongSwitch("publish-all")]
    public string? PublishAll { get; set; }

    [CommandLongSwitch("pull")]
    public string? Pull { get; set; }

    [CommandLongSwitch("quiet")]
    public string? Quiet { get; set; }

    [CommandLongSwitch("read-only")]
    public string? ReadOnly { get; set; }

    [CommandLongSwitch("restart")]
    public string? Restart { get; set; }

    [CommandLongSwitch("rm")]
    public string? Rm { get; set; }

    [CommandLongSwitch("runtime")]
    public string? Runtime { get; set; }

    [CommandLongSwitch("security-opt")]
    public string? SecurityOpt { get; set; }

    [CommandLongSwitch("shm-size")]
    public string? ShmSize { get; set; }

    [BooleanCommandSwitch("sig-proxy")]
    public bool? SigProxy { get; set; }

    [CommandLongSwitch("stop-signal")]
    public string? StopSignal { get; set; }

    [CommandLongSwitch("stop-timeout")]
    public string? StopTimeout { get; set; }

    [CommandLongSwitch("storage-opt")]
    public string? StorageOpt { get; set; }

    [CommandLongSwitch("sysctl")]
    public string? Sysctl { get; set; }

    [CommandLongSwitch("tmpfs")]
    public string? Tmpfs { get; set; }

    [CommandLongSwitch("tty")]
    public string? Tty { get; set; }

    [CommandLongSwitch("ulimit")]
    public string? Ulimit { get; set; }

    [CommandLongSwitch("user")]
    public string? User { get; set; }

    [CommandLongSwitch("userns")]
    public string? Userns { get; set; }

    [CommandLongSwitch("uts")]
    public string? Uts { get; set; }

    [CommandLongSwitch("volume")]
    public string? Volume { get; set; }

    [CommandLongSwitch("volume-driver")]
    public string? VolumeDriver { get; set; }

    [CommandLongSwitch("volumes-from")]
    public string? VolumesFrom { get; set; }

    [CommandLongSwitch("workdir")]
    public string? Workdir { get; set; }

}