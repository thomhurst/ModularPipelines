using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datastream", "objects", "lookup")]
public record GcloudDatastreamObjectsLookupOptions(
[property: CommandSwitch("--mysql-database")] string MysqlDatabase,
[property: CommandSwitch("--mysql-table")] string MysqlTable,
[property: CommandSwitch("--oracle-schema")] string OracleSchema,
[property: CommandSwitch("--oracle-table")] string OracleTable,
[property: CommandSwitch("--postgresql-schema")] string PostgresqlSchema,
[property: CommandSwitch("--postgresql-table")] string PostgresqlTable,
[property: CommandSwitch("--stream")] string Stream,
[property: CommandSwitch("--location")] string Location
) : GcloudOptions;