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

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public IEnumerable<string>? Container { get; set; }

    [CliOption("--blkio-weight")]
    public virtual string? BlkioWeight { get; set; }

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

    [CliOption("--kernel-memory")]
    public virtual string? KernelMemory { get; set; }

    [CliOption("--memory")]
    public virtual string? Memory { get; set; }

    [CliOption("--memory-reservation")]
    public virtual string? MemoryReservation { get; set; }

    [CliOption("--memory-swap")]
    public virtual string? MemorySwap { get; set; }

    [CliOption("--pids-limit")]
    public virtual string? PidsLimit { get; set; }

    [CliOption("--restart")]
    public virtual string? Restart { get; set; }
}
