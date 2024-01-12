using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("spanner", "rows", "delete")]
public record GcloudSpannerRowsDeleteOptions(
[property: CommandSwitch("--keys")] string[] Keys,
[property: CommandSwitch("--table")] string Table,
[property: CommandSwitch("--database")] string Database,
[property: CommandSwitch("--instance")] string Instance
) : GcloudOptions;