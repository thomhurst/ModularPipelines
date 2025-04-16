using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[ExcludeFromCodeCoverage]
public record DockerImageHistoryOptions : DockerOptions
{
    public DockerImageHistoryOptions(
        string image
    )
    {
        CommandParts = ["image", "history"];

        Image = image;
    }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? Image { get; set; }

    [CommandSwitch("--format")]
    public virtual string? Format { get; set; }

    [BooleanCommandSwitch("--human")]
    public virtual bool? Human { get; set; }

    [BooleanCommandSwitch("--no-trunc")]
    public virtual bool? NoTrunc { get; set; }

    [BooleanCommandSwitch("--quiet")]
    public virtual bool? Quiet { get; set; }
}
