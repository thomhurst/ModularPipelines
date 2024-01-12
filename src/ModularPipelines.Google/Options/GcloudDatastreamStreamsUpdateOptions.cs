using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datastream", "streams", "update")]
public record GcloudDatastreamStreamsUpdateOptions(
[property: PositionalArgument] string Stream,
[property: PositionalArgument] string Location
) : GcloudOptions
{
    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }

    [CommandSwitch("--state")]
    public string? State { get; set; }

    [CommandSwitch("--update-labels")]
    public IEnumerable<KeyValue>? UpdateLabels { get; set; }

    [CommandSwitch("--update-mask")]
    public string? UpdateMask { get; set; }

    [BooleanCommandSwitch("--backfill-none")]
    public bool? BackfillNone { get; set; }

    [BooleanCommandSwitch("--backfill-all")]
    public bool? BackfillAll { get; set; }

    [CommandSwitch("--mysql-excluded-objects")]
    public string? MysqlExcludedObjects { get; set; }

    [CommandSwitch("--oracle-excluded-objects")]
    public string? OracleExcludedObjects { get; set; }

    [CommandSwitch("--postgresql-excluded-objects")]
    public string? PostgresqlExcludedObjects { get; set; }

    [BooleanCommandSwitch("--clear-labels")]
    public bool? ClearLabels { get; set; }

    [CommandSwitch("--remove-labels")]
    public string[]? RemoveLabels { get; set; }

    [CommandSwitch("--destination")]
    public string? Destination { get; set; }

    [CommandSwitch("--bigquery-destination-config")]
    public string? BigqueryDestinationConfig { get; set; }

    [CommandSwitch("--gcs-destination-config")]
    public string? GcsDestinationConfig { get; set; }

    [BooleanCommandSwitch("--force")]
    public bool? Force { get; set; }

    [BooleanCommandSwitch("--validate-only")]
    public bool? ValidateOnly { get; set; }

    [CommandSwitch("--source")]
    public string? Source { get; set; }

    [CommandSwitch("--mysql-source-config")]
    public string? MysqlSourceConfig { get; set; }

    [CommandSwitch("--oracle-source-config")]
    public string? OracleSourceConfig { get; set; }

    [CommandSwitch("--postgresql-source-config")]
    public string? PostgresqlSourceConfig { get; set; }
}