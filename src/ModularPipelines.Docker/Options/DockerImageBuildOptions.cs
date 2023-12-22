using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("image build")]
[ExcludeFromCodeCoverage]
public record DockerImageBuildOptions([property: PositionalArgument(Position = Position.AfterSwitches)] string Path) : DockerOptions
{
    [CommandSwitch("--add-host")]
    public string? AddHost { get; set; }

    [CommandSwitch("--build-arg")]
    public IEnumerable<KeyValue>? BuildArgs { get; set; }

    [CommandSwitch("--cache-from")]
    public string? CacheFrom { get; set; }

    [CommandSwitch("--cgroup-parent")]
    public string? CgroupParent { get; set; }

    [BooleanCommandSwitch("--compress")]
    public bool? Compress { get; set; }

    [CommandSwitch("--cpu-period")]
    public string? CpuPeriod { get; set; }

    [CommandSwitch("--cpu-quota")]
    public string? CpuQuota { get; set; }

    [CommandSwitch("--cpu-shares")]
    public string? CpuShares { get; set; }

    [CommandSwitch("--cpuset-cpus")]
    public string? CpusetCpus { get; set; }

    [CommandSwitch("--cpuset-mems")]
    public string? CpusetMems { get; set; }

    [BooleanCommandSwitch("--disable-content-trust")]
    public bool? DisableContentTrust { get; set; }

    [CommandSwitch("--file")]
    public string? File { get; set; }

    [BooleanCommandSwitch("--force-rm")]
    public bool? ForceRm { get; set; }

    [CommandSwitch("--iidfile")]
    public string? Iidfile { get; set; }

    [BooleanCommandSwitch("--isolation")]
    public bool? Isolation { get; set; }

    [CommandSwitch("--label")]
    public string? Label { get; set; }

    [CommandSwitch("--memory")]
    public string? Memory { get; set; }

    [CommandSwitch("--memory-swap")]
    public string? MemorySwap { get; set; }

    [CommandSwitch("--network")]
    public string? Network { get; set; }

    [BooleanCommandSwitch("--no-cache")]
    public bool? NoCache { get; set; }

    [CommandSwitch("--platform")]
    public string? Platform { get; set; }

    [BooleanCommandSwitch("--pull")]
    public bool? Pull { get; set; }

    [BooleanCommandSwitch("--quiet")]
    public bool? Quiet { get; set; }

    [BooleanCommandSwitch("--rm")]
    public bool? Rm { get; set; }

    [CommandSwitch("--security-opt")]
    public string? SecurityOpt { get; set; }

    [CommandSwitch("--shm-size")]
    public string? ShmSize { get; set; }

    [BooleanCommandSwitch("--squash")]
    public bool? Squash { get; set; }

    [CommandSwitch("--tag")]
    public string? Tag { get; set; }

    [CommandSwitch("--target")]
    public string? Target { get; set; }

    [CommandSwitch("--ulimit")]
    public string? Ulimit { get; set; }
}