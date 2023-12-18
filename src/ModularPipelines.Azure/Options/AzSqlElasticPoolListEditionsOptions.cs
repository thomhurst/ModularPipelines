using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql", "elastic-pool", "list-editions")]
public record AzSqlElasticPoolListEditionsOptions(
[property: CommandSwitch("--location")] string Location
) : AzOptions
{
    [BooleanCommandSwitch("--available")]
    public bool? Available { get; set; }

    [CommandSwitch("--dtu")]
    public string? Dtu { get; set; }

    [CommandSwitch("--edition")]
    public string? Edition { get; set; }

    [CommandSwitch("--show-details")]
    public string? ShowDetails { get; set; }

    [CommandSwitch("--vcores")]
    public string? Vcores { get; set; }
}