using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("trust", "inspect")]
[ExcludeFromCodeCoverage]
public record DockerTrustInspectOptions : DockerOptions
{
    [PositionalArgument(Position = Position.AfterSwitches)]
    public IEnumerable<string>? Image { get; set; }

    [CommandSwitch("--pretty")]
    public virtual string? Pretty { get; set; }
}
