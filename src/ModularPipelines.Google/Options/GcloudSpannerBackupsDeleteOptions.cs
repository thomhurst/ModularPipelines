using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("spanner", "backups", "delete")]
public record GcloudSpannerBackupsDeleteOptions(
[property: CliArgument] string Backup,
[property: CliArgument] string Instance
) : GcloudOptions;