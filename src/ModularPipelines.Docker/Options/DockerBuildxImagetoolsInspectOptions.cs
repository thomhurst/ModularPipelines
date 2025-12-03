using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[ExcludeFromCodeCoverage]
public record DockerBuildxImagetoolsInspectOptions : DockerOptions
{
    public DockerBuildxImagetoolsInspectOptions(
        string name
    )
    {
        CommandParts = ["buildx", "imagetools", "inspect"];

        Name = name;
    }

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public string? Name { get; set; }

    [CliOption("--format")]
    public virtual string? Format { get; set; }

    [CliFlag("--raw")]
    public virtual bool? Raw { get; set; }
}
