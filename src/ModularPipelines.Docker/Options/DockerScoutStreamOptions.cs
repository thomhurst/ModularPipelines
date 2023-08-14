using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("scout stream")]
public record DockerScoutStreamOptions([property: PositionalArgument(Position = Position.AfterArguments)] string Stream, [property: PositionalArgument(Position = Position.AfterArguments)] string Image) : DockerOptions
{
    [CommandSwitch("--app")]
    public string? App { get; set; }

    [CommandSwitch("--platform")]
    public string? Platform { get; set; }
}