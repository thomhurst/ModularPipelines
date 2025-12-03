using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sql", "elastic-pool", "list-editions")]
public record AzSqlElasticPoolListEditionsOptions(
[property: CliOption("--location")] string Location
) : AzOptions
{
    [CliFlag("--available")]
    public bool? Available { get; set; }

    [CliOption("--dtu")]
    public string? Dtu { get; set; }

    [CliOption("--edition")]
    public string? Edition { get; set; }

    [CliOption("--show-details")]
    public string? ShowDetails { get; set; }

    [CliOption("--vcores")]
    public string? Vcores { get; set; }
}