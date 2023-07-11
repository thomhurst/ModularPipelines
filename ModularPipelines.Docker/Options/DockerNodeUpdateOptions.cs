using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("node update")]
public record DockerNodeUpdateOptions([property: PositionalArgument(Position = Position.AfterArguments)] string Node) : DockerOptions
{
    [CommandSwitch("--availability")]
    public string? Availability { get; set; }

    [CommandSwitch("--label-rm")]
    public string? LabelRm { get; set; }

    [CommandSwitch("--role")]
    public string? Role { get; set; }

    [CommandSwitch("--label-add")]
    public string? LabelAdd { get; set; }

}