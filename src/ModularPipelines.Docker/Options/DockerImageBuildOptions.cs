using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Docker.Options;

[ExcludeFromCodeCoverage]
public record DockerImageBuildOptions : DockerOptions
{
    public DockerImageBuildOptions(
        string pathOrUrl
    )
    {
        CommandParts = ["image", "build"];

        PathOrUrl = pathOrUrl;
    }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? PathOrUrl { get; set; }

    [CommandSwitch("--add-host")]
    public virtual string? AddHost { get; set; }

    [CommandSwitch("--build-arg")]
    public virtual IEnumerable<KeyValue>? BuildArg { get; set; }

    [CommandSwitch("--cache-from")]
    public virtual string? CacheFrom { get; set; }

    [CommandSwitch("--cgroup-parent")]
    public virtual string? CgroupParent { get; set; }

    [BooleanCommandSwitch("--compress")]
    public virtual bool? Compress { get; set; }

    [CommandSwitch("--cpu-period")]
    public virtual string? CpuPeriod { get; set; }

    [CommandSwitch("--cpu-quota")]
    public virtual string? CpuQuota { get; set; }

    [CommandSwitch("--cpu-shares")]
    public virtual string? CpuShares { get; set; }

    [CommandSwitch("--cpuset-cpus")]
    public virtual string? CpusetCpus { get; set; }

    [CommandSwitch("--cpuset-mems")]
    public virtual string? CpusetMems { get; set; }

    [BooleanCommandSwitch("--disable-content-trust")]
    public virtual bool? DisableContentTrust { get; set; }

    [CommandSwitch("--file")]
    public virtual string? File { get; set; }

    [BooleanCommandSwitch("--force-rm")]
    public virtual bool? ForceRm { get; set; }

    [CommandSwitch("--iidfile")]
    public virtual string? Iidfile { get; set; }

    [BooleanCommandSwitch("--isolation")]
    public virtual bool? Isolation { get; set; }

    [CommandSwitch("--label")]
    public virtual string? Label { get; set; }

    [CommandSwitch("--memory")]
    public virtual string? Memory { get; set; }

    [CommandSwitch("--memory-swap")]
    public virtual string? MemorySwap { get; set; }

    [CommandSwitch("--network")]
    public virtual string? Network { get; set; }

    [BooleanCommandSwitch("--no-cache")]
    public virtual bool? NoCache { get; set; }

    [CommandSwitch("--platform")]
    public virtual string? Platform { get; set; }

    [BooleanCommandSwitch("--pull")]
    public virtual bool? Pull { get; set; }

    [BooleanCommandSwitch("--quiet")]
    public virtual bool? Quiet { get; set; }

    [BooleanCommandSwitch("--rm")]
    public virtual bool? Rm { get; set; }

    [CommandSwitch("--security-opt")]
    public virtual string? SecurityOpt { get; set; }

    [CommandSwitch("--shm-size")]
    public virtual string? ShmSize { get; set; }

    [BooleanCommandSwitch("--squash")]
    public virtual bool? Squash { get; set; }

    [CommandSwitch("--tag")]
    public virtual string? Tag { get; set; }

    [CommandSwitch("--target")]
    public virtual string? Target { get; set; }

    [CommandSwitch("--ulimit")]
    public virtual string? Ulimit { get; set; }
}
