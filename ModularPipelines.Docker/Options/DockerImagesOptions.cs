using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("images")]
public record DockerImagesOptions : DockerOptions
{
    [CommandLongSwitch("all")]
    public string? All { get; set; }

    [CommandLongSwitch("quiet")]
    public string? Quiet { get; set; }

}