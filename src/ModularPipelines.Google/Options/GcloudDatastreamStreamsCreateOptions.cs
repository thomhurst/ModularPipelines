using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datastream", "streams", "create")]
public record GcloudDatastreamStreamsCreateOptions(
[property: PositionalArgument] string Stream,
[property: PositionalArgument] string Location,
[property: CommandSwitch("--display-name")] string DisplayName,
[property: BooleanCommandSwitch("--backfill-none")] bool BackfillNone,
[property: BooleanCommandSwitch("--backfill-all")] bool BackfillAll,
[property: CommandSwitch("--mysql-excluded-objects")] string MysqlExcludedObjects,
[property: CommandSwitch("--oracle-excluded-objects")] string OracleExcludedObjects,
[property: CommandSwitch("--postgresql-excluded-objects")] string PostgresqlExcludedObjects,
[property: CommandSwitch("--destination")] string Destination,
[property: CommandSwitch("--bigquery-destination-config")] string BigqueryDestinationConfig,
[property: CommandSwitch("--gcs-destination-config")] string GcsDestinationConfig,
[property: CommandSwitch("--source")] string Source,
[property: CommandSwitch("--mysql-source-config")] string MysqlSourceConfig,
[property: CommandSwitch("--oracle-source-config")] string OracleSourceConfig,
[property: CommandSwitch("--postgresql-source-config")] string PostgresqlSourceConfig
) : GcloudOptions
{
    [CommandSwitch("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [BooleanCommandSwitch("--force")]
    public bool? Force { get; set; }

    [BooleanCommandSwitch("--validate-only")]
    public bool? ValidateOnly { get; set; }
}