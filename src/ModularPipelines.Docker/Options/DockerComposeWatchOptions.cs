using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("compose", "watch")]
[ExcludeFromCodeCoverage]
public record DockerComposeWatchOptions : DockerOptions
{
    [PositionalArgument(Position = Position.AfterSwitches)]
    public IEnumerable<string>? Service { get; set; }

    [CommandSwitch("--no-up")]
    public virtual string? NoUp { get; set; }

    [BooleanCommandSwitch("--quiet")]
    public virtual bool? Quiet { get; set; }
}
