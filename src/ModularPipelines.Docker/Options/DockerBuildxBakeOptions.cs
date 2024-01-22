using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("buildx", "bake")]
[ExcludeFromCodeCoverage]
public record DockerBuildxBakeOptions : DockerOptions
{
    [PositionalArgument(Position = Position.AfterSwitches)]
    public IEnumerable<string>? Target { get; set; }

    [CommandSwitch("--file")]
    public string? File { get; set; }

    [CommandSwitch("--load")]
    public string? Load { get; set; }

    [CommandSwitch("--metadata-file")]
    public string? MetadataFile { get; set; }

    [BooleanCommandSwitch("--no-cache")]
    public bool? NoCache { get; set; }

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

    [BooleanCommandSwitch("--sbom")]
    public bool? Sbom { get; set; }

    [CommandSwitch("--set")]
    public string? Set { get; set; }
}
