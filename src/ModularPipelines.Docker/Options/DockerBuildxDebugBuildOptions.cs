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

    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? PathOrUrl { get; set; }

    [CommandSwitch("--add-host")]
    public string? AddHost { get; set; }

    [CommandSwitch("--allow")]
    public string? Allow { get; set; }

    [CommandSwitch("--annotation")]
    public string? Annotation { get; set; }

    [CommandSwitch("--attest")]
    public string? Attest { get; set; }

    [CommandSwitch("--build-arg")]
    public IEnumerable<KeyValue>? BuildArg { get; set; }

    [CommandSwitch("--build-context")]
    public string? BuildContext { get; set; }

    [CommandSwitch("--cache-from")]
    public string? CacheFrom { get; set; }

    [CommandSwitch("--cache-to")]
    public string? CacheTo { get; set; }

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

    [BooleanCommandSwitch("--detach")]
    public bool? Detach { get; set; }

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

    [CommandSwitch("--load")]
    public string? Load { get; set; }

    [CommandSwitch("--memory")]
    public string? Memory { get; set; }

    [CommandSwitch("--memory-swap")]
    public string? MemorySwap { get; set; }

    [CommandSwitch("--metadata-file")]
    public string? MetadataFile { get; set; }

    [CommandSwitch("--network")]
    public string? Network { get; set; }

    [BooleanCommandSwitch("--no-cache")]
    public bool? NoCache { get; set; }

    [CommandSwitch("--no-cache-filter")]
    public string? NoCacheFilter { get; set; }

    [CommandSwitch("--output")]
    public string? Output { get; set; }

    [CommandSwitch("--platform")]
    public string? Platform { get; set; }

    [CommandSwitch("--print")]
    public string? Print { get; set; }

    [CommandSwitch("--progress")]
    public string? Progress { get; set; }

    [CommandSwitch("--provenance")]
    public string? Provenance { get; set; }

    [BooleanCommandSwitch("--pull")]
    public bool? Pull { get; set; }

    [BooleanCommandSwitch("--push")]
    public bool? Push { get; set; }

    [BooleanCommandSwitch("--quiet")]
    public bool? Quiet { get; set; }

    [BooleanCommandSwitch("--rm")]
    public bool? Rm { get; set; }

    [CommandSwitch("--root")]
    public string? Root { get; set; }

    [BooleanCommandSwitch("--sbom")]
    public bool? Sbom { get; set; }

    [CommandSwitch("--secret")]
    public string? Secret { get; set; }

    [CommandSwitch("--security-opt")]
    public string? SecurityOpt { get; set; }

    [CommandSwitch("--server-config")]
    public string? ServerConfig { get; set; }

    [CommandSwitch("--shm-size")]
    public string? ShmSize { get; set; }

    [BooleanCommandSwitch("--squash")]
    public bool? Squash { get; set; }

    [CommandSwitch("--ssh")]
    public string? Ssh { get; set; }

    [CommandSwitch("--tag")]
    public string? Tag { get; set; }

    [CommandSwitch("--target")]
    public string? Target { get; set; }

    [CommandSwitch("--ulimit")]
    public string? Ulimit { get; set; }
}
