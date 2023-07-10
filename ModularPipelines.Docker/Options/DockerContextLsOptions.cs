using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("context ls")]
public record DockerContextLsOptions : DockerOptions
{
    [CommandLongSwitch("format")]
    public string? Format { get; set; }

    [CommandLongSwitch("quiet")]
    public string? Quiet { get; set; }

}