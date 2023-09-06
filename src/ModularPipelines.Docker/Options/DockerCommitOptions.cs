using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("commit")]
[ExcludeFromCodeCoverage]
public record DockerCommitOptions([property: PositionalArgument(Position = Position.AfterArguments)] string Container) : DockerOptions
{
    [PositionalArgument(Position = Position.AfterArguments)]
    public string? Repository { get; set; }

    [CommandSwitch("--author")]
    public string? Author { get; set; }

    [CommandSwitch("--message")]
    public string? Message { get; set; }

    [BooleanCommandSwitch("--pause")]
    public bool? Pause { get; set; }

    [CommandSwitch("--change")]
    public string? Change { get; set; }
}
