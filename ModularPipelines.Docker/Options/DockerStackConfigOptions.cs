using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("stack config")]
public record DockerStackConfigOptions : DockerOptions
{
    [CommandLongSwitch("compose-file")]
    public string? ComposeFile { get; set; }

    [CommandLongSwitch("skip-interpolation")]
    public string? SkipInterpolation { get; set; }

}