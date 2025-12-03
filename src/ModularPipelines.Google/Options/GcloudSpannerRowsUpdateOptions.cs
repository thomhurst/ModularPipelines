using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("spanner", "rows", "update")]
public record GcloudSpannerRowsUpdateOptions(
[property: CliOption("--data")] string[] Data,
[property: CliOption("--table")] string Table,
[property: CliOption("--database")] string Database,
[property: CliOption("--instance")] string Instance
) : GcloudOptions;