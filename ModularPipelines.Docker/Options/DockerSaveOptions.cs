using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("save")]
public record DockerSaveOptions([property: PositionalArgument] string Image) : DockerOptions
{
    [CommandLongSwitch("output")]
    public string? Output { get; set; }
}