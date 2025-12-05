using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[ExcludeFromCodeCoverage]
public record DockerContextUpdateOptions : DockerOptions
{
    public DockerContextUpdateOptions(
        string context
    )
    {
        CommandParts = ["context", "update"];

        UpdateContext = context;
    }

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public virtual string? UpdateContext { get; set; }

    [CliOption("--description")]
    public virtual string? Description { get; set; }

    [CliOption("--docker")]
    public virtual string? Docker { get; set; }
}
