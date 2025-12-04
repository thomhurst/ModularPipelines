using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sql", "db", "list-editions")]
public record AzSqlDbListEditionsOptions(
[property: CliOption("--location")] string Location
) : AzOptions
{
    [CliFlag("--available")]
    public bool? Available { get; set; }

    [CliOption("--dtu")]
    public string? Dtu { get; set; }

    [CliOption("--edition")]
    public string? Edition { get; set; }

    [CliOption("--service-objective")]
    public string? ServiceObjective { get; set; }

    [CliOption("--show-details")]
    public string? ShowDetails { get; set; }

    [CliOption("--vcores")]
    public string? Vcores { get; set; }
}