using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sql", "databases", "delete")]
public record GcloudSqlDatabasesDeleteOptions(
[property: CliArgument] string Database,
[property: CliOption("--instance")] string Instance
) : GcloudOptions;