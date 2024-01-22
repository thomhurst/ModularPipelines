using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

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
    public string? Availability { get; set; }

    [CommandSwitch("--label-add")]
    public string? LabelAdd { get; set; }

    [CommandSwitch("--label-rm")]
    public string? LabelRm { get; set; }

    [CommandSwitch("--role")]
    public string? Role { get; set; }
}
