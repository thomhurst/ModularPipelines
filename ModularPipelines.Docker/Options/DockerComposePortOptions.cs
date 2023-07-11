using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("compose port")]
public record DockerComposePortOptions([property: PositionalArgument(Position = Position.AfterArguments)] string Service, [property: PositionalArgument(Position = Position.AfterArguments)] string Privateport) : DockerOptions
{

    [CommandSwitch("--index")]
    public string? Index { get; set; }

    [CommandSwitch("--protocol")]
    public string? Protocol { get; set; }

    [BooleanCommandSwitch("--dry-run")]
    public bool? DryRun { get; set; }

}