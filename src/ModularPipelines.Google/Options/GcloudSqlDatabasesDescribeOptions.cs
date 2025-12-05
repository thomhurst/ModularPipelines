using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sql", "databases", "describe")]
public record GcloudSqlDatabasesDescribeOptions(
[property: CliArgument] string Database,
[property: CliOption("--instance")] string Instance
) : GcloudOptions;