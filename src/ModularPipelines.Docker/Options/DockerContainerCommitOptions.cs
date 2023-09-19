using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("container commit")]
[ExcludeFromCodeCoverage]
public record DockerContainerCommitOptions([property: PositionalArgument(Position = Position.AfterSwitches)] string Container) : DockerOptions
{
    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? Repository { get; set; }

    [CommandSwitch("--author")]
    public string? Author { get; set; }

    [CommandSwitch("--change")]
    public string? Change { get; set; }

    [CommandSwitch("--message")]
    public string? Message { get; set; }

    [BooleanCommandSwitch("--pause")]
    public bool? Pause { get; set; }
}
