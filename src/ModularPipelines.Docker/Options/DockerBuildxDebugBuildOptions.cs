using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Docker.Options;

[ExcludeFromCodeCoverage]
public record DockerBuildxDebugBuildOptions : DockerOptions
{
    public DockerBuildxDebugBuildOptions(
        string pathOrUrl
    )
    {
        CommandParts = ["buildx", "debug_build"];

        PathOrUrl = pathOrUrl;
    }

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public virtual string? PathOrUrl { get; set; }

    [CliOption("--add-host")]
    public virtual string? AddHost { get; set; }

    [CliOption("--allow")]
    public virtual string? Allow { get; set; }

    [CliOption("--annotation")]
    public virtual string? Annotation { get; set; }

    [CliOption("--attest")]
    public virtual string? Attest { get; set; }

    [CliOption("--build-arg")]
    public virtual IEnumerable<KeyValue>? BuildArg { get; set; }

    [CliOption("--build-context")]
    public virtual string? BuildContext { get; set; }

    [CliOption("--cache-from")]
    public virtual string? CacheFrom { get; set; }

    [CliOption("--cache-to")]
    public virtual string? CacheTo { get; set; }

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

    [CliFlag("--detach")]
    public virtual bool? Detach { get; set; }

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

    [CliOption("--load")]
    public virtual string? Load { get; set; }

    [CliOption("--memory")]
    public virtual string? Memory { get; set; }

    [CliOption("--memory-swap")]
    public virtual string? MemorySwap { get; set; }

    [CliOption("--metadata-file")]
    public virtual string? MetadataFile { get; set; }

    [CliOption("--network")]
    public virtual string? Network { get; set; }

    [CliFlag("--no-cache")]
    public virtual bool? NoCache { get; set; }

    [CliOption("--no-cache-filter")]
    public virtual string? NoCacheFilter { get; set; }

    [CliOption("--output")]
    public virtual string? Output { get; set; }

    [CliOption("--platform")]
    public virtual string? Platform { get; set; }

    [CliOption("--print")]
    public virtual string? Print { get; set; }

    [CliOption("--progress")]
    public virtual string? Progress { get; set; }

    [CliOption("--provenance")]
    public virtual string? Provenance { get; set; }

    [CliFlag("--pull")]
    public virtual bool? Pull { get; set; }

    [CliFlag("--push")]
    public virtual bool? Push { get; set; }

    [CliFlag("--quiet")]
    public virtual bool? Quiet { get; set; }

    [CliFlag("--rm")]
    public virtual bool? Rm { get; set; }

    [CliOption("--root")]
    public virtual string? Root { get; set; }

    [CliFlag("--sbom")]
    public virtual bool? Sbom { get; set; }

    [CliOption("--secret")]
    public virtual string? Secret { get; set; }

    [CliOption("--security-opt")]
    public virtual string? SecurityOpt { get; set; }

    [CliOption("--server-config")]
    public virtual string? ServerConfig { get; set; }

    [CliOption("--shm-size")]
    public virtual string? ShmSize { get; set; }

    [CliFlag("--squash")]
    public virtual bool? Squash { get; set; }

    [CliOption("--ssh")]
    public virtual string? Ssh { get; set; }

    [CliOption("--tag")]
    public virtual string? Tag { get; set; }

    [CliOption("--target")]
    public virtual string? Target { get; set; }

    [CliOption("--ulimit")]
    public virtual string? Ulimit { get; set; }
}
