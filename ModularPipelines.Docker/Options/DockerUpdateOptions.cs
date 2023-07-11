using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("update")]
public record DockerUpdateOptions([property: PositionalArgument(Position = Position.AfterArguments)] IEnumerable<string> Container) : DockerOptions
{
    [CommandSwitch("--blkio-weight")]
    public string? BlkioWeight { get; set; }

    [CommandSwitch("--cpu-period")]
    public string? CpuPeriod { get; set; }

    [CommandSwitch("--cpu-quota")]
    public string? CpuQuota { get; set; }

    [CommandSwitch("--cpu-rt-period")]
    public string? CpuRtPeriod { get; set; }

    [CommandSwitch("--cpu-rt-runtime")]
    public string? CpuRtRuntime { get; set; }

    [CommandSwitch("--cpus")]
    public string? Cpus { get; set; }

    [CommandSwitch("--cpuset-cpus")]
    public string? CpusetCpus { get; set; }

    [CommandSwitch("--cpuset-mems")]
    public string? CpusetMems { get; set; }

    [CommandSwitch("--memory-reservation")]
    public string? MemoryReservation { get; set; }

    [CommandSwitch("--memory-swap")]
    public string? MemorySwap { get; set; }

    [CommandSwitch("--pids-limit")]
    public string? PidsLimit { get; set; }

    [CommandSwitch("--cpu-shares")]
    public string? CpuShares { get; set; }

    [CommandSwitch("--memory")]
    public string? Memory { get; set; }

    [CommandSwitch("--restart")]
    public string? Restart { get; set; }

}