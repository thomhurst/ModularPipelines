using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Docker.Options;

[ExcludeFromCodeCoverage]
public record DockerContainerCommitOptions : DockerOptions
{
    public DockerContainerCommitOptions(
        string container
    )
    {
        CommandParts = ["container", "commit"];

        Container = container;
    }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? Container { get; set; }

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
