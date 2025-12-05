using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datastream", "objects", "lookup")]
public record GcloudDatastreamObjectsLookupOptions(
[property: CliOption("--mysql-database")] string MysqlDatabase,
[property: CliOption("--mysql-table")] string MysqlTable,
[property: CliOption("--oracle-schema")] string OracleSchema,
[property: CliOption("--oracle-table")] string OracleTable,
[property: CliOption("--postgresql-schema")] string PostgresqlSchema,
[property: CliOption("--postgresql-table")] string PostgresqlTable,
[property: CliOption("--stream")] string Stream,
[property: CliOption("--location")] string Location
) : GcloudOptions;