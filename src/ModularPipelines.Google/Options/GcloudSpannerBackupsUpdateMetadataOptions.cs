using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("spanner", "backups", "update-metadata")]
public record GcloudSpannerBackupsUpdateMetadataOptions(
[property: CliArgument] string Backup,
[property: CliArgument] string Instance,
[property: CliOption("--expiration-date")] string ExpirationDate,
[property: CliOption("--retention-period")] string RetentionPeriod
) : GcloudOptions;