using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("scout", "environment")]
[ExcludeFromCodeCoverage]
public record DockerScoutEnvironmentOptions : DockerOptions
{
    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? Environment { get; set; }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? Image { get; set; }

    [CommandSwitch("--org")]
    public string? Org { get; set; }

    [CommandSwitch("--output")]
    public string? Output { get; set; }

    [CommandSwitch("--platform")]
    public string? Platform { get; set; }
}
