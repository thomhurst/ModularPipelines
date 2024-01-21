using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Docker.Options;

[ExcludeFromCodeCoverage]
public record DockerSearchOptions : DockerOptions
{
    public DockerSearchOptions(
        string term
    )
    {
        CommandParts = ["search"];

        Term = term;
    }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? Term { get; set; }

    [CommandSwitch("--filter")]
    public string? Filter { get; set; }

    [CommandSwitch("--format")]
    public string? Format { get; set; }

    [CommandSwitch("--limit")]
    public string? Limit { get; set; }

    [BooleanCommandSwitch("--no-trunc")]
    public bool? NoTrunc { get; set; }
}
