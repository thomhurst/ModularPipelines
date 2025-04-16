using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("compose", "restart")]
[ExcludeFromCodeCoverage]
public record DockerComposeRestartOptions : DockerOptions
{
    [PositionalArgument(Position = Position.AfterSwitches)]
    public IEnumerable<string>? Service { get; set; }

    [BooleanCommandSwitch("--no-deps")]
    public virtual bool? NoDeps { get; set; }

    [CommandSwitch("--timeout")]
    public virtual string? Timeout { get; set; }
}
