using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("compose version")]
public record DockerComposeVersionOptions : DockerOptions
{
    [CommandLongSwitch("format")]
    public string? Format { get; set; }

    [CommandLongSwitch("short")]
    public string? Short { get; set; }

    [CommandLongSwitch("dry-run")]
    public string? DryRun { get; set; }

}