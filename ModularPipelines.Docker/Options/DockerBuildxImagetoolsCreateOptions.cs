using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("buildx imagetools create")]
public record DockerBuildxImagetoolsCreateOptions : DockerOptions
{
    [PositionalArgument(Position = Position.AfterArguments)]
    public IEnumerable<string> Source { get; set; }

    [CommandSwitch("--progress")]
    public string? Progress { get; set; }

    [CommandSwitch("--append")]
    public string? Append { get; set; }

    [BooleanCommandSwitch("--dry-run")]
    public bool? DryRun { get; set; }

    [CommandSwitch("--file")]
    public string? File { get; set; }

    [CommandSwitch("--tag")]
    public string? Tag { get; set; }

    [CommandSwitch("--builder")]
    public string? Builder { get; set; }

}