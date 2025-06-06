using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("compose", "events")]
[ExcludeFromCodeCoverage]
public record DockerComposeEventsOptions : DockerOptions
{
    [PositionalArgument(Position = Position.AfterSwitches)]
    public IEnumerable<string>? Service { get; set; }

    [CommandSwitch("--json")]
    public virtual string? Json { get; set; }
}
