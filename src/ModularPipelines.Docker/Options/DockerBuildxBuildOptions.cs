using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("buildx build")]
[ExcludeFromCodeCoverage]
public record DockerBuildxBuildOptions([property: PositionalArgument(Position = Position.AfterArguments)] string Path) : DockerOptions
{
    [BooleanCommandSwitch("--detach")]
    public bool? Detach { get; set; }

    [CommandSwitch("--iidfile")]
    public string? Iidfile { get; set; }

    [CommandSwitch("--invoke")]
    public string? Invoke { get; set; }

    [CommandSwitch("--label")]
    public string? Label { get; set; }

    [CommandSwitch("--network")]
    public string? Network { get; set; }

    [BooleanCommandSwitch("--no-cache")]
    public bool? NoCache { get; set; }

    [CommandSwitch("--no-cache-filter")]
    public string? NoCacheFilter { get; set; }

    [CommandSwitch("--print")]
    public string? Print { get; set; }

    [BooleanCommandSwitch("--pull")]
    public bool? Pull { get; set; }

    [BooleanCommandSwitch("--quiet")]
    public bool? Quiet { get; set; }

    [CommandSwitch("--root")]
    public string? Root { get; set; }

    [CommandSwitch("--server-config")]
    public string? ServerConfig { get; set; }

    [CommandSwitch("--add-host")]
    public string? AddHost { get; set; }

    [CommandSwitch("--allow")]
    public string? Allow { get; set; }

    [CommandSwitch("--attest")]
    public string? Attest { get; set; }

    [CommandSwitch("--build-arg")]
    public KeyValueVariables? BuildArgs { get; set; }

    [CommandSwitch("--build-context")]
    public string? BuildContext { get; set; }

    [CommandSwitch("--cache-from")]
    public string? CacheFrom { get; set; }

    [CommandSwitch("--cache-to")]
    public string? CacheTo { get; set; }

    [CommandSwitch("--cgroup-parent")]
    public string? CgroupParent { get; set; }

    [CommandSwitch("--file")]
    public string? File { get; set; }

    [CommandSwitch("--load")]
    public string? Load { get; set; }

    [CommandSwitch("--metadata-file")]
    public string? MetadataFile { get; set; }

    [CommandSwitch("--platform")]
    public string? Platform { get; set; }

    [CommandSwitch("--progress")]
    public string? Progress { get; set; }

    [CommandSwitch("--provenance")]
    public string? Provenance { get; set; }

    [BooleanCommandSwitch("--push")]
    public bool? Push { get; set; }

    [BooleanCommandSwitch("--sbom")]
    public bool? Sbom { get; set; }

    [CommandSwitch("--secret")]
    public string? Secret { get; set; }

    [CommandSwitch("--shm-size")]
    public string? ShmSize { get; set; }

    [CommandSwitch("--ssh")]
    public string? Ssh { get; set; }

    [CommandSwitch("--tag")]
    public string? Tag { get; set; }

    [CommandSwitch("--target")]
    public string? Target { get; set; }

    [CommandSwitch("--ulimit")]
    public string? Ulimit { get; set; }

    [CommandSwitch("--builder")]
    public string? Builder { get; set; }
}
