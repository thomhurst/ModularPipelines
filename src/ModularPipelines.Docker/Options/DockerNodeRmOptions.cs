using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[ExcludeFromCodeCoverage]
public record DockerNodeRmOptions : DockerOptions
{
    public DockerNodeRmOptions(
        IEnumerable<string> node
    )
    {
        CommandParts = ["node", "rm"];

        Node = node;
    }

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public IEnumerable<string>? Node { get; set; }

    [CliFlag("--force")]
    public virtual bool? Force { get; set; }
}
