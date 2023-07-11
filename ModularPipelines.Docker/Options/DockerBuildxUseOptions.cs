using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("buildx use")]
public record DockerBuildxUseOptions([property: PositionalArgument(Position = Position.AfterArguments)] string Name) : DockerOptions
{

    [CommandSwitch("--default")]
    public string? Default { get; set; }

    [CommandSwitch("--global")]
    public string? Global { get; set; }

    [CommandSwitch("--builder")]
    public string? Builder { get; set; }
}