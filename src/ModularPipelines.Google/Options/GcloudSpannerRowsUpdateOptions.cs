using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("spanner", "rows", "update")]
public record GcloudSpannerRowsUpdateOptions(
[property: CommandSwitch("--data")] string[] Data,
[property: CommandSwitch("--table")] string Table,
[property: CommandSwitch("--database")] string Database,
[property: CommandSwitch("--instance")] string Instance
) : GcloudOptions;