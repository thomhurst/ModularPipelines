using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[ExcludeFromCodeCoverage]
public record DockerNodeInspectOptions : DockerOptions
{
    public DockerNodeInspectOptions(
        string selfOrNode
    )
    {
        CommandParts = ["node", "inspect"];

        SelfOrNode = selfOrNode;
    }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? SelfOrNode { get; set; }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public IEnumerable<string>? Node { get; set; }

    [CommandSwitch("--format")]
    public string? Format { get; set; }

    [CommandSwitch("--pretty")]
    public string? Pretty { get; set; }
}
