using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("search")]
[ExcludeFromCodeCoverage]
public record DockerSearchOptions([property: PositionalArgument(Position = Position.AfterSwitches)] string Term) : DockerOptions
{
    [CommandSwitch("--filter")]
    public string? Filter { get; set; }

    [CommandSwitch("--format")]
    public string? Format { get; set; }

    [CommandSwitch("--limit")]
    public string? Limit { get; set; }

    [BooleanCommandSwitch("--no-trunc")]
    public bool? NoTrunc { get; set; }
}
