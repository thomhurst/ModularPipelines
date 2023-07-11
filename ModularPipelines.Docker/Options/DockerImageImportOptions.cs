using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("image import")]
public record DockerImageImportOptions : DockerOptions
{
    [PositionalArgument(Position = Position.AfterArguments)]
    public string Repository { get; set; }
    [CommandSwitch("--change")]
    public string? Change { get; set; }

    [CommandSwitch("--message")]
    public string? Message { get; set; }

    [CommandSwitch("--platform")]
    public string? Platform { get; set; }

}