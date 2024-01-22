using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Docker.Options;

[ExcludeFromCodeCoverage]
public record DockerNodeInspectOptions : DockerOptions
{
    public DockerNodeInspectOptions(
        string selfNode
    )
    {
        CommandParts = ["node", "inspect"];

        SelfNode = selfNode;
    }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? SelfNode { get; set; }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public IEnumerable<string>? Node { get; set; }

    [CommandSwitch("--format")]
    public string? Format { get; set; }

    [CommandSwitch("--pretty")]
    public string? Pretty { get; set; }
}
