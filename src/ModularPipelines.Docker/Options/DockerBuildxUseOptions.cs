using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[ExcludeFromCodeCoverage]
public record DockerBuildxUseOptions : DockerOptions
{
    public DockerBuildxUseOptions(
        string name
    )
    {
        CommandParts = ["buildx", "use"];

        Name = name;
    }

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public virtual string? Name { get; set; }

    [CliOption("--default")]
    public virtual string? Default { get; set; }

    [CliOption("--global")]
    public virtual string? Global { get; set; }
}
