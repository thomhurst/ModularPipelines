using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[ExcludeFromCodeCoverage]
public record DockerContainerCpOptions : DockerOptions
{
    public DockerContainerCpOptions(
        string container,
        string destPath,
        string cp,
        string srcPath
    )
    {
        CommandParts = ["container", "cp"];

        Container = container;

        DestPath = destPath;

        Cp = cp;

        SrcPath = srcPath;
    }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? Container { get; set; }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? DestPath { get; set; }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? Cp { get; set; }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? Options { get; set; }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? SrcPath { get; set; }

    [CommandSwitch("--archive")]
    public virtual string? Archive { get; set; }

    [CommandSwitch("--follow-link")]
    public virtual string? FollowLink { get; set; }

    [BooleanCommandSwitch("--quiet")]
    public virtual bool? Quiet { get; set; }
}
