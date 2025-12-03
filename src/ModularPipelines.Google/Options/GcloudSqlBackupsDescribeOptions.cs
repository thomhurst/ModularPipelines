using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sql", "backups", "describe")]
public record GcloudSqlBackupsDescribeOptions(
[property: CliArgument] string Id,
[property: CliOption("--instance")] string Instance
) : GcloudOptions;