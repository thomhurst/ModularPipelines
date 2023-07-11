using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("buildx bake")]
public record DockerBuildxBakeOptions : DockerOptions
{
    [PositionalArgument(Position = Position.AfterArguments)]
    public IEnumerable<string> Target { get; set; }
    [CommandSwitch("--load")]
    public string? Load { get; set; }

    [CommandSwitch("--metadata-file")]
    public string? MetadataFile { get; set; }

    [BooleanCommandSwitch("--push")]
    public bool? Push { get; set; }

    [CommandSwitch("--file")]
    public string? File { get; set; }

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

    [BooleanCommandSwitch("--sbom")]
    public bool? Sbom { get; set; }

    [CommandSwitch("--set")]
    public string? Set { get; set; }

    [CommandSwitch("--builder")]
    public string? Builder { get; set; }

}