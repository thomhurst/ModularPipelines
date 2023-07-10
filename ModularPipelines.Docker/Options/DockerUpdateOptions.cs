using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("update")]
public record DockerUpdateOptions : DockerOptions
{
    [CommandLongSwitch("blkio-weight")]
    public string? BlkioWeight { get; set; }

    [CommandLongSwitch("cpu-period")]
    public string? CpuPeriod { get; set; }

    [CommandLongSwitch("cpu-quota")]
    public string? CpuQuota { get; set; }

    [CommandLongSwitch("cpu-rt-period")]
    public string? CpuRtPeriod { get; set; }

    [CommandLongSwitch("cpu-rt-runtime")]
    public string? CpuRtRuntime { get; set; }

    [CommandLongSwitch("cpus")]
    public string? Cpus { get; set; }

    [CommandLongSwitch("cpuset-cpus")]
    public string? CpusetCpus { get; set; }

    [CommandLongSwitch("cpuset-mems")]
    public string? CpusetMems { get; set; }

    [CommandLongSwitch("memory-reservation")]
    public string? MemoryReservation { get; set; }

    [CommandLongSwitch("memory-swap")]
    public string? MemorySwap { get; set; }

    [CommandLongSwitch("pids-limit")]
    public string? PidsLimit { get; set; }

}