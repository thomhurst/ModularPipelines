using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[ExcludeFromCodeCoverage]
public record DockerContainerUpdateOptions : DockerOptions
{
    public DockerContainerUpdateOptions(
        IEnumerable<string> container
    )
    {
        CommandParts = ["container", "update"];

        Container = container;
    }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public IEnumerable<string>? Container { get; set; }

    [CommandSwitch("--blkio-weight")]
    public virtual string? BlkioWeight { get; set; }

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

    [CommandSwitch("--kernel-memory")]
    public virtual string? KernelMemory { get; set; }

    [CommandSwitch("--memory")]
    public virtual string? Memory { get; set; }

    [CommandSwitch("--memory-reservation")]
    public virtual string? MemoryReservation { get; set; }

    [CommandSwitch("--memory-swap")]
    public virtual string? MemorySwap { get; set; }

    [CommandSwitch("--pids-limit")]
    public virtual string? PidsLimit { get; set; }

    [CommandSwitch("--restart")]
    public virtual string? Restart { get; set; }
}
