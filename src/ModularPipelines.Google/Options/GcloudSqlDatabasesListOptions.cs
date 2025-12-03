using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sql", "databases", "list")]
public record GcloudSqlDatabasesListOptions(
[property: CliOption("--instance")] string Instance
) : GcloudOptions;