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

    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? Name { get; set; }

    [CommandSwitch("--format")]
    public virtual string? Format { get; set; }

    [BooleanCommandSwitch("--raw")]
    public virtual bool? Raw { get; set; }
}
