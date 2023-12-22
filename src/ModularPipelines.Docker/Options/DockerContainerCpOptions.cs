using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("container cp")]
[ExcludeFromCodeCoverage]
public record DockerContainerCpOptions([property: PositionalArgument(Position = Position.AfterSwitches)] string SrcPath, [property: PositionalArgument(Position = Position.AfterSwitches)] string DestPath) : DockerOptions
{
    [CommandSwitch("--archive")]
    public string? Archive { get; set; }

    [CommandSwitch("--follow-link")]
    public string? FollowLink { get; set; }

    [BooleanCommandSwitch("--quiet")]
    public bool? Quiet { get; set; }
}