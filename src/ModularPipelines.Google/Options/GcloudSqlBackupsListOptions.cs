using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sql", "backups", "list")]
public record GcloudSqlBackupsListOptions(
[property: CliOption("--instance")] string Instance
) : GcloudOptions;