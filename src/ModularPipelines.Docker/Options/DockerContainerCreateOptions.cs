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

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public string? Image { get; set; }

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public string? Command { get; set; }

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public IEnumerable<string>? Arg { get; set; }

    [CliOption("--add-host")]
    public virtual string? AddHost { get; set; }

    [CliOption("--annotation")]
    public virtual string? Annotation { get; set; }

    [CliFlag("--attach")]
    public virtual bool? Attach { get; set; }

    [CliOption("--blkio-weight")]
    public virtual string? BlkioWeight { get; set; }

    [CliOption("--blkio-weight-device")]
    public virtual string? BlkioWeightDevice { get; set; }

    [CliOption("--cap-add")]
    public virtual string? CapAdd { get; set; }

    [CliOption("--cap-drop")]
    public virtual string? CapDrop { get; set; }

    [CliOption("--cgroup-parent")]
    public virtual string? CgroupParent { get; set; }

    [CliOption("--cgroupns")]
    public virtual string? Cgroupns { get; set; }

    [CliOption("--cidfile")]
    public virtual string? Cidfile { get; set; }

    [CliOption("--cpu-count")]
    public virtual string? CpuCount { get; set; }

    [CliOption("--cpu-percent")]
    public virtual string? CpuPercent { get; set; }

    [CliOption("--cpu-period")]
    public virtual string? CpuPeriod { get; set; }

    [CliOption("--cpu-quota")]
    public virtual string? CpuQuota { get; set; }

    [CliOption("--cpu-rt-period")]
    public virtual string? CpuRtPeriod { get; set; }

    [CliOption("--cpu-rt-runtime")]
    public virtual string? CpuRtRuntime { get; set; }

    [CliOption("--cpu-shares")]
    public virtual string? CpuShares { get; set; }

    [CliOption("--cpus")]
    public virtual string? Cpus { get; set; }

    [CliOption("--cpuset-cpus")]
    public virtual string? CpusetCpus { get; set; }

    [CliOption("--cpuset-mems")]
    public virtual string? CpusetMems { get; set; }

    [CliOption("--device")]
    public virtual string? Device { get; set; }

    [CliOption("--device-cgroup-rule")]
    public virtual string? DeviceCgroupRule { get; set; }

    [CliOption("--device-read-bps")]
    public virtual string? DeviceReadBps { get; set; }

    [CliOption("--device-read-iops")]
    public virtual string? DeviceReadIops { get; set; }

    [CliOption("--device-write-bps")]
    public virtual string? DeviceWriteBps { get; set; }

    [CliOption("--device-write-iops")]
    public virtual string? DeviceWriteIops { get; set; }

    [CliFlag("--disable-content-trust")]
    public virtual bool? DisableContentTrust { get; set; }

    [CliOption("--dns")]
    public virtual string? Dns { get; set; }

    [CliOption("--dns-opt")]
    public virtual string? DnsOpt { get; set; }

    [CliOption("--dns-option")]
    public virtual string? DnsOption { get; set; }

    [CliOption("--dns-search")]
    public virtual string? DnsSearch { get; set; }

    [CliOption("--domainname")]
    public virtual string? Domainname { get; set; }

    [CliOption("--entrypoint")]
    public virtual string? Entrypoint { get; set; }

    [CliOption("--env")]
    public virtual string? Env { get; set; }

    [CliOption("--env-file")]
    public virtual string? EnvFile { get; set; }

    [CliOption("--expose")]
    public virtual string? Expose { get; set; }

    [CliOption("--gpus")]
    public virtual string? Gpus { get; set; }

    [CliOption("--group-add")]
    public virtual string? GroupAdd { get; set; }

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

    [CliFlag("--interactive")]
    public virtual bool? Interactive { get; set; }

    [CliOption("--io-maxbandwidth")]
    public virtual string? IoMaxbandwidth { get; set; }

    [CliOption("--io-maxiops")]
    public virtual string? IoMaxiops { get; set; }

    [CliOption("--ip")]
    public virtual string? Ip { get; set; }

    [CliOption("--ip6")]
    public virtual string? Ip6 { get; set; }

    [CliOption("--ipc")]
    public virtual string? Ipc { get; set; }

    [CliFlag("--isolation")]
    public virtual bool? Isolation { get; set; }

    [CliOption("--kernel-memory")]
    public virtual string? KernelMemory { get; set; }

    [CliOption("--label")]
    public virtual string? Label { get; set; }

    [CliOption("--label-file")]
    public virtual string? LabelFile { get; set; }

    [CliOption("--link")]
    public virtual string? Link { get; set; }

    [CliOption("--link-local-ip")]
    public virtual string? LinkLocalIp { get; set; }

    [CliOption("--log-driver")]
    public virtual string? LogDriver { get; set; }

    [CliOption("--log-opt")]
    public virtual string? LogOpt { get; set; }

    [CliOption("--mac-address")]
    public virtual string? MacAddress { get; set; }

    [CliOption("--memory")]
    public virtual string? Memory { get; set; }

    [CliOption("--memory-reservation")]
    public virtual string? MemoryReservation { get; set; }

    [CliOption("--memory-swap")]
    public virtual string? MemorySwap { get; set; }

    [CliOption("--memory-swappiness")]
    public virtual int? MemorySwappiness { get; set; }

    [CliOption("--mount")]
    public virtual string? Mount { get; set; }

    [CliOption("--name")]
    public virtual string? Name { get; set; }

    [CliOption("--net")]
    public virtual string? Net { get; set; }

    [CliOption("--net-alias")]
    public virtual string? NetAlias { get; set; }

    [CliOption("--network")]
    public virtual string? Network { get; set; }

    [CliOption("--network-alias")]
    public virtual string? NetworkAlias { get; set; }

    [CliFlag("--no-healthcheck")]
    public virtual bool? NoHealthcheck { get; set; }

    [CliFlag("--oom-kill-disable")]
    public virtual bool? OomKillDisable { get; set; }

    [CliOption("--oom-score-adj")]
    public virtual string? OomScoreAdj { get; set; }

    [CliOption("--pid")]
    public virtual string? Pid { get; set; }

    [CliOption("--pids-limit")]
    public virtual string? PidsLimit { get; set; }

    [CliOption("--platform")]
    public virtual string? Platform { get; set; }

    [CliFlag("--privileged")]
    public virtual bool? Privileged { get; set; }

    [CliOption("--publish")]
    public virtual string? Publish { get; set; }

    [CliOption("--publish-all")]
    public virtual string? PublishAll { get; set; }

    [CliFlag("--pull")]
    public virtual bool? Pull { get; set; }

    [CliFlag("--quiet")]
    public virtual bool? Quiet { get; set; }

    [CliFlag("--read-only")]
    public virtual bool? ReadOnly { get; set; }

    [CliOption("--restart")]
    public virtual string? Restart { get; set; }

    [CliFlag("--rm")]
    public virtual bool? Rm { get; set; }

    [CliOption("--runtime")]
    public virtual string? Runtime { get; set; }

    [CliOption("--security-opt")]
    public virtual string? SecurityOpt { get; set; }

    [CliOption("--shm-size")]
    public virtual string? ShmSize { get; set; }

    [CliOption("--stop-signal")]
    public virtual string? StopSignal { get; set; }

    [CliOption("--stop-timeout")]
    public virtual string? StopTimeout { get; set; }

    [CliOption("--storage-opt")]
    public virtual string? StorageOpt { get; set; }

    [CliOption("--sysctl")]
    public virtual string? Sysctl { get; set; }

    [CliOption("--tmpfs")]
    public virtual string? Tmpfs { get; set; }

    [CliOption("--tty")]
    public virtual string? Tty { get; set; }

    [CliOption("--ulimit")]
    public virtual string? Ulimit { get; set; }

    [CliOption("--user")]
    public virtual string? User { get; set; }

    [CliOption("--userns")]
    public virtual string? Userns { get; set; }

    [CliOption("--uts")]
    public virtual string? Uts { get; set; }

    [CliOption("--volume")]
    public virtual string? Volume { get; set; }

    [CliOption("--volume-driver")]
    public virtual string? VolumeDriver { get; set; }

    [CliOption("--volumes-from")]
    public virtual string? VolumesFrom { get; set; }

    [CliOption("--workdir")]
    public virtual string? Workdir { get; set; }
}
