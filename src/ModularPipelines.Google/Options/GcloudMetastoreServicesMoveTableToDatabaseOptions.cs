using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("metastore", "services", "move-table-to-database")]
public record GcloudMetastoreServicesMoveTableToDatabaseOptions(
[property: CliArgument] string Service,
[property: CliArgument] string Location,
[property: CliOption("--destination_db_name")] string DestinationDbName,
[property: CliOption("--source_db_name")] string SourceDbName,
[property: CliOption("--table_name")] string TableName
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }
}