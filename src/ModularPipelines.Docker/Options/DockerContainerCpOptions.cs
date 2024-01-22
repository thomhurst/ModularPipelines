using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Docker.Options;

[ExcludeFromCodeCoverage]
public record DockerContainerCpOptions : DockerOptions
{
    public DockerContainerCpOptions(
        string container,
        string destPathDocker,
        string cp,
        string srcPath
    )
    {
        CommandParts = ["container", "cp"];

        Container = container;

        DestPathDocker = destPathDocker;

        Cp = cp;

        SrcPath = srcPath;
    }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? Container { get; set; }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? DestPathDocker { get; set; }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? Cp { get; set; }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? Options { get; set; }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? SrcPath { get; set; }

    [CommandSwitch("--archive")]
    public string? Archive { get; set; }

    [CommandSwitch("--follow-link")]
    public string? FollowLink { get; set; }

    [BooleanCommandSwitch("--quiet")]
    public bool? Quiet { get; set; }
}
