using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("compose images")]
public record DockerComposeImagesOptions : DockerOptions
{
    [CommandLongSwitch("format")]
    public string? Format { get; set; }

    [CommandLongSwitch("quiet")]
    public string? Quiet { get; set; }

    [CommandLongSwitch("dry-run")]
    public string? DryRun { get; set; }

}