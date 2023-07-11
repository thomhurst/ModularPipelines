using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("buildx inspect")]
public record DockerBuildxInspectOptions : DockerOptions
{
    [PositionalArgument(Position = Position.AfterArguments)]
    public string Name { get; set; }

    [CommandSwitch("--bootstrap")]
    public string? Bootstrap { get; set; }


    [CommandSwitch("--builder")]
    public string? Builder { get; set; }

}