using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datastream", "streams", "create")]
public record GcloudDatastreamStreamsCreateOptions(
[property: CliArgument] string Stream,
[property: CliArgument] string Location,
[property: CliOption("--display-name")] string DisplayName,
[property: CliFlag("--backfill-none")] bool BackfillNone,
[property: CliFlag("--backfill-all")] bool BackfillAll,
[property: CliOption("--mysql-excluded-objects")] string MysqlExcludedObjects,
[property: CliOption("--oracle-excluded-objects")] string OracleExcludedObjects,
[property: CliOption("--postgresql-excluded-objects")] string PostgresqlExcludedObjects,
[property: CliOption("--destination")] string Destination,
[property: CliOption("--bigquery-destination-config")] string BigqueryDestinationConfig,
[property: CliOption("--gcs-destination-config")] string GcsDestinationConfig,
[property: CliOption("--source")] string Source,
[property: CliOption("--mysql-source-config")] string MysqlSourceConfig,
[property: CliOption("--oracle-source-config")] string OracleSourceConfig,
[property: CliOption("--postgresql-source-config")] string PostgresqlSourceConfig
) : GcloudOptions
{
    [CliOption("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CliFlag("--force")]
    public bool? Force { get; set; }

    [CliFlag("--validate-only")]
    public bool? ValidateOnly { get; set; }
}