using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[ExcludeFromCodeCoverage]
public record DockerNodeUpdateOptions : DockerOptions
{
    public DockerNodeUpdateOptions(
        string node
    )
    {
        CommandParts = ["node", "update"];

        Node = node;
    }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? Node { get; set; }

    [CommandSwitch("--availability")]
    public virtual string? Availability { get; set; }

    [CommandSwitch("--label-add")]
    public virtual string? LabelAdd { get; set; }

    [CommandSwitch("--label-rm")]
    public virtual string? LabelRm { get; set; }

    [CommandSwitch("--role")]
    public virtual string? Role { get; set; }
}
