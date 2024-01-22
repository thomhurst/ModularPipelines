using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

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

    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? Name { get; set; }

    [CommandSwitch("--format")]
    public string? Format { get; set; }

    [BooleanCommandSwitch("--raw")]
    public bool? Raw { get; set; }
}
