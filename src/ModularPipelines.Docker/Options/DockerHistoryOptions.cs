using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("history")]
[ExcludeFromCodeCoverage]
public record DockerHistoryOptions([property: PositionalArgument(Position = Position.AfterSwitches)] string Image) : DockerOptions
{
    [BooleanCommandSwitch("--human")]
    public bool? Human { get; set; }

    [BooleanCommandSwitch("--no-trunc")]
    public bool? NoTrunc { get; set; }

    [BooleanCommandSwitch("--quiet")]
    public bool? Quiet { get; set; }

    [CommandSwitch("--format")]
    public string? Format { get; set; }
}