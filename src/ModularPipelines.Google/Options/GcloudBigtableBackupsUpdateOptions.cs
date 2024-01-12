using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("bigtable", "backups", "update")]
public record GcloudBigtableBackupsUpdateOptions(
[property: PositionalArgument] string Backup,
[property: PositionalArgument] string Cluster,
[property: PositionalArgument] string Instance,
[property: CommandSwitch("--expiration-date")] string ExpirationDate,
[property: CommandSwitch("--retention-period")] string RetentionPeriod
) : GcloudOptions;