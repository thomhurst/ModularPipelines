using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("ps")]
public record DockerPsOptions : DockerOptions
{
    [CommandLongSwitch("last")]
    public string? Last { get; set; }

    [CommandLongSwitch("latest")]
    public string? Latest { get; set; }

    [CommandLongSwitch("quiet")]
    public string? Quiet { get; set; }

}