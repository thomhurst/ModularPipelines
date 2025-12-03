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

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public string? PathOrUrl { get; set; }

    [CliOption("--add-host")]
    public virtual string? AddHost { get; set; }

    [CliOption("--build-arg")]
    public virtual IEnumerable<KeyValue>? BuildArg { get; set; }

    [CliOption("--cache-from")]
    public virtual string? CacheFrom { get; set; }

    [CliOption("--cgroup-parent")]
    public virtual string? CgroupParent { get; set; }

    [CliFlag("--compress")]
    public virtual bool? Compress { get; set; }

    [CliOption("--cpu-period")]
    public virtual string? CpuPeriod { get; set; }

    [CliOption("--cpu-quota")]
    public virtual string? CpuQuota { get; set; }

    [CliOption("--cpu-shares")]
    public virtual string? CpuShares { get; set; }

    [CliOption("--cpuset-cpus")]
    public virtual string? CpusetCpus { get; set; }

    [CliOption("--cpuset-mems")]
    public virtual string? CpusetMems { get; set; }

    [CliFlag("--disable-content-trust")]
    public virtual bool? DisableContentTrust { get; set; }

    [CliOption("--file")]
    public virtual string? File { get; set; }

    [CliFlag("--force-rm")]
    public virtual bool? ForceRm { get; set; }

    [CliOption("--iidfile")]
    public virtual string? Iidfile { get; set; }

    [CliFlag("--isolation")]
    public virtual bool? Isolation { get; set; }

    [CliOption("--label")]
    public virtual string? Label { get; set; }

    [CliOption("--memory")]
    public virtual string? Memory { get; set; }

    [CliOption("--memory-swap")]
    public virtual string? MemorySwap { get; set; }

    [CliOption("--network")]
    public virtual string? Network { get; set; }

    [CliFlag("--no-cache")]
    public virtual bool? NoCache { get; set; }

    [CliOption("--platform")]
    public virtual string? Platform { get; set; }

    [CliFlag("--pull")]
    public virtual bool? Pull { get; set; }

    [CliFlag("--quiet")]
    public virtual bool? Quiet { get; set; }

    [CliFlag("--rm")]
    public virtual bool? Rm { get; set; }

    [CliOption("--security-opt")]
    public virtual string? SecurityOpt { get; set; }

    [CliOption("--shm-size")]
    public virtual string? ShmSize { get; set; }

    [CliFlag("--squash")]
    public virtual bool? Squash { get; set; }

    [CliOption("--tag")]
    public virtual string? Tag { get; set; }

    [CliOption("--target")]
    public virtual string? Target { get; set; }

    [CliOption("--ulimit")]
    public virtual string? Ulimit { get; set; }
}
