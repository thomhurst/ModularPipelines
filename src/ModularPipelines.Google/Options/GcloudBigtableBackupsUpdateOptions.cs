using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("bigtable", "backups", "update")]
public record GcloudBigtableBackupsUpdateOptions(
[property: CliArgument] string Backup,
[property: CliArgument] string Cluster,
[property: CliArgument] string Instance,
[property: CliOption("--expiration-date")] string ExpirationDate,
[property: CliOption("--retention-period")] string RetentionPeriod
) : GcloudOptions;