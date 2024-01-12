using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("metastore", "services", "move-table-to-database")]
public record GcloudMetastoreServicesMoveTableToDatabaseOptions(
[property: PositionalArgument] string Service,
[property: PositionalArgument] string Location,
[property: CommandSwitch("--destination_db_name")] string DestinationDbName,
[property: CommandSwitch("--source_db_name")] string SourceDbName,
[property: CommandSwitch("--table_name")] string TableName
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }
}