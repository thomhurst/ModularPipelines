using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("spanner", "backups", "describe")]
public record GcloudSpannerBackupsDescribeOptions(
[property: CliArgument] string Backup,
[property: CliArgument] string Instance
) : GcloudOptions;