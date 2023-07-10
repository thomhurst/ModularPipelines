using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("compose rm")]
public record DockerComposeRmOptions : DockerOptions
{
    [CommandLongSwitch("force")]
    public string? Force { get; set; }

    [CommandLongSwitch("stop")]
    public string? Stop { get; set; }

    [CommandLongSwitch("volumes")]
    public string? Volumes { get; set; }

    [CommandLongSwitch("dry-run")]
    public string? DryRun { get; set; }

}