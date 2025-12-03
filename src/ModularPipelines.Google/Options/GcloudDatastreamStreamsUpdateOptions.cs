using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datastream", "streams", "update")]
public record GcloudDatastreamStreamsUpdateOptions(
[property: CliArgument] string Stream,
[property: CliArgument] string Location
) : GcloudOptions
{
    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliOption("--state")]
    public string? State { get; set; }

    [CliOption("--update-labels")]
    public IEnumerable<KeyValue>? UpdateLabels { get; set; }

    [CliOption("--update-mask")]
    public string? UpdateMask { get; set; }

    [CliFlag("--backfill-none")]
    public bool? BackfillNone { get; set; }

    [CliFlag("--backfill-all")]
    public bool? BackfillAll { get; set; }

    [CliOption("--mysql-excluded-objects")]
    public string? MysqlExcludedObjects { get; set; }

    [CliOption("--oracle-excluded-objects")]
    public string? OracleExcludedObjects { get; set; }

    [CliOption("--postgresql-excluded-objects")]
    public string? PostgresqlExcludedObjects { get; set; }

    [CliFlag("--clear-labels")]
    public bool? ClearLabels { get; set; }

    [CliOption("--remove-labels")]
    public string[]? RemoveLabels { get; set; }

    [CliOption("--destination")]
    public string? Destination { get; set; }

    [CliOption("--bigquery-destination-config")]
    public string? BigqueryDestinationConfig { get; set; }

    [CliOption("--gcs-destination-config")]
    public string? GcsDestinationConfig { get; set; }

    [CliFlag("--force")]
    public bool? Force { get; set; }

    [CliFlag("--validate-only")]
    public bool? ValidateOnly { get; set; }

    [CliOption("--source")]
    public string? Source { get; set; }

    [CliOption("--mysql-source-config")]
    public string? MysqlSourceConfig { get; set; }

    [CliOption("--oracle-source-config")]
    public string? OracleSourceConfig { get; set; }

    [CliOption("--postgresql-source-config")]
    public string? PostgresqlSourceConfig { get; set; }
}