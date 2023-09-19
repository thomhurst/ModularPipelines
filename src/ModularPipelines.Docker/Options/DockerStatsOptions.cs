using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("stats")]
[ExcludeFromCodeCoverage]
public record DockerStatsOptions : DockerOptions
{
    [PositionalArgument(Position = Position.AfterSwitches)]
    public IEnumerable<string>? Container { get; set; }
    [BooleanCommandSwitch("--all")]
    public bool? All { get; set; }

    [CommandSwitch("--no-stream")]
    public string? NoStream { get; set; }

    [BooleanCommandSwitch("--no-trunc")]
    public bool? NoTrunc { get; set; }

    [CommandSwitch("--format")]
    public string? Format { get; set; }
}
