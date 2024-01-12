using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("spanner", "backups", "delete")]
public record GcloudSpannerBackupsDeleteOptions(
[property: PositionalArgument] string Backup,
[property: PositionalArgument] string Instance
) : GcloudOptions;