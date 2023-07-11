using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("image save")]
public record DockerImageSaveOptions([property: PositionalArgument(Position = Position.AfterArguments)] IEnumerable<string> Image) : DockerOptions
{
    [CommandSwitch("--output")]
    public string? Output { get; set; }

}