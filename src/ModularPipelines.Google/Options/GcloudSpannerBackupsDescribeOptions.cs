using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("spanner", "backups", "describe")]
public record GcloudSpannerBackupsDescribeOptions(
[property: PositionalArgument] string Backup,
[property: PositionalArgument] string Instance
) : GcloudOptions;