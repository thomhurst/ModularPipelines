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

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public string? SelfOrNode { get; set; }

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public IEnumerable<string>? Node { get; set; }

    [CliOption("--format")]
    public virtual string? Format { get; set; }

    [CliOption("--pretty")]
    public virtual string? Pretty { get; set; }
}
