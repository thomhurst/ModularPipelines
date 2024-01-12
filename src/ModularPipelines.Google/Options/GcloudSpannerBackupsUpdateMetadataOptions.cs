using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("spanner", "backups", "update-metadata")]
public record GcloudSpannerBackupsUpdateMetadataOptions(
[property: PositionalArgument] string Backup,
[property: PositionalArgument] string Instance,
[property: CommandSwitch("--expiration-date")] string ExpirationDate,
[property: CommandSwitch("--retention-period")] string RetentionPeriod
) : GcloudOptions;