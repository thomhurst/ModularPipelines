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

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public string? Node { get; set; }

    [CliOption("--availability")]
    public virtual string? Availability { get; set; }

    [CliOption("--label-add")]
    public virtual string? LabelAdd { get; set; }

    [CliOption("--label-rm")]
    public virtual string? LabelRm { get; set; }

    [CliOption("--role")]
    public virtual string? Role { get; set; }
}
