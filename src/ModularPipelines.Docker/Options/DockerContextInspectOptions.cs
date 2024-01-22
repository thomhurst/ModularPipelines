using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("context", "inspect")]
[ExcludeFromCodeCoverage]
public record DockerContextInspectOptions : DockerOptions
{
    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? InspectContext { get; set; }

    [CommandSwitch("--format")]
    public string? Format { get; set; }
}
