using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("image import")]
public record DockerImageImportOptions : DockerOptions
{
    [CommandLongSwitch("change")]
    public string? Change { get; set; }

    [CommandLongSwitch("message")]
    public string? Message { get; set; }

    [CommandLongSwitch("platform")]
    public string? Platform { get; set; }

}