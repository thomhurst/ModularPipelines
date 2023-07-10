using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("compose ps")]
public record DockerComposePsOptions : DockerOptions
{
    [CommandLongSwitch("all")]
    public string? All { get; set; }

    [CommandLongSwitch("quiet")]
    public string? Quiet { get; set; }

    [CommandLongSwitch("services")]
    public string? Services { get; set; }

    [CommandLongSwitch("dry-run")]
    public string? DryRun { get; set; }

}