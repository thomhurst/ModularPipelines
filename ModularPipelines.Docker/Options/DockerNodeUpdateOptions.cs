using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("node update")]
public record DockerNodeUpdateOptions : DockerOptions
{
    [CommandLongSwitch("availability")]
    public string? Availability { get; set; }

    [CommandLongSwitch("label-rm")]
    public string? LabelRm { get; set; }

    [CommandLongSwitch("role")]
    public string? Role { get; set; }

}