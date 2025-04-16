using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

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
    public virtual string? Author { get; set; }

    [CommandSwitch("--change")]
    public virtual string? Change { get; set; }

    [CommandSwitch("--message")]
    public virtual string? Message { get; set; }

    [BooleanCommandSwitch("--pause")]
    public virtual bool? Pause { get; set; }
}
